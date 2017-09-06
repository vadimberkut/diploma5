using System;
using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using diploma5_csharp.Models;
using System.Linq;
using diploma5_csharp.Helpers;

namespace diploma5_csharp
{
    public class Fog
    {
        #region DarkChannelPrior (Patch-based DCP)

        public Image<Emgu.CV.Structure.Bgr, Byte> RemoveFogUsingDarkChannelPrior(Image<Bgr, Byte> image, out Image<Gray, Byte> transmission, FogRemovalParams _params)
        {
            Image<Bgr, Byte> result = image.Clone();
            Image<Bgr, Byte> imgFog = image.Clone();
            Image<Gray, Byte> imgDarkChannel = new Image<Gray, byte>(image.Size);
            Image<Gray, Byte> T = new Image<Gray, byte>(image.Size);
            Image<Bgr, Byte> fogfree = image.Clone();

            int Airlight;

            //int patchSize = 5;
            int patchSize = 7;
            imgDarkChannel = GetDarkChannel(imgFog, patchSize);
            Airlight = EstimateAirlight(imgDarkChannel, imgFog);
            T = EstimateTransmission(imgDarkChannel, Airlight);
            fogfree = RemoveFog(imgFog, T, Airlight);

            //Return out params
            transmission = T;

            if (_params.ShowWindows)
            {
                EmguCvWindowManager.Display(imgDarkChannel, "1 imgDarkChannel darkChannel MDCP");
                EmguCvWindowManager.Display(T, "2 estimateTransmission");
                EmguCvWindowManager.Display(imgFog, "3 imgFog");
                EmguCvWindowManager.Display(fogfree, "4 fogfree");
            }

            return fogfree;
        }

        private Image<Gray, Byte> GetDarkChannel(Image<Bgr, Byte> sourceImg, int patchSize)
        {
            //            Mat rgbMinImg = Mat::zeros(sourceImg.rows, sourceImg.cols, CV_8UC1);
            //            Mat MDCP;
            Image<Gray, Byte> rgbMinImg = new Image<Gray, byte>(sourceImg.Size);
            Image<Gray, Byte> MDCP = new Image<Gray, byte>(sourceImg.Size);

            for (int m = 0; m < sourceImg.Rows; m++)
            {
                for (int n = 0; n < sourceImg.Cols; n++)
                {
                    Bgr pixel = sourceImg[m, n];
                    double intensity = Math.Min(Math.Min(pixel.Blue, pixel.Green), pixel.Red);
                    rgbMinImg[m, n] = new Gray(intensity);
                }
            }
            CvInvoke.MedianBlur(rgbMinImg, MDCP, patchSize);
            return MDCP;
        }

        //estimate airlight by the 0.1% brightest pixels in dark channel
        private int EstimateAirlight(Image<Gray, Byte> DC, Image<Bgr, Byte> inputImage)
        {
            Image<Gray, Byte> rgbMinImg = new Image<Gray, byte>(inputImage.Size);
            Image<Gray, Byte> MDCP = new Image<Gray, byte>(inputImage.Size);
            Image<Lab, Byte> inputImageLab = new Image<Lab, byte>(inputImage.Size);

            double minDC = 0;
            double maxDC = 0;
            System.Drawing.Point minDCLoc = new Point();
            System.Drawing.Point maxDCLoc = new Point();
            int size = DC.Rows * DC.Cols;
            double requiredPercent = 0.001; //0.1%
            double requiredAmount = size * requiredPercent; //

            ////////////////////
            CvInvoke.MinMaxLoc(DC, ref minDC, ref maxDC, ref minDCLoc, ref maxDCLoc);
            double max = maxDC;
//            std::vector<int*> brightestDarkChannelPixels;
            List<List<int>> brightestDarkChannelPixels = new List<List<int>>();
            for (int k = 0; k < requiredAmount && max >= 0; max--)
            {
                for (int i = 0; i < DC.Rows; i++)
                {
                    bool _break = false;
                    for (int j = 0; j < DC.Cols; j++)
                    {
//                        uchar val = DC.at<uchar>(i, j);
                        Gray val = DC[i, j];
                        if (val.Intensity == max)
                            brightestDarkChannelPixels.Add(new List<int>() { i, j });

                        if (brightestDarkChannelPixels.Count >= requiredAmount - 1)
                        {
                            _break = true;
                            break;
                        }
                    }
                    if (_break)
                        break;
                }

                if (brightestDarkChannelPixels.Count >= requiredAmount)
                    break;
            }

            //take pixels with highest intensity in the input image
            CvInvoke.CvtColor(inputImage, inputImageLab, ColorConversion.Bgr2Lab);
            int airlight = -1;
            for (int r = 0; r != brightestDarkChannelPixels.Count; r++)
            {
                int i = brightestDarkChannelPixels[r][0];
                int j = brightestDarkChannelPixels[r][1];

                Lab pixel = inputImageLab[i, j];

                double L = pixel.X;
                double intensity = L; //take Lab L as insetsity
                if (intensity > airlight)
                    airlight = (int)intensity;
            }

            /////////////////

            return airlight;
        }

        //estimate transmission map
        private Image<Gray, Byte> EstimateTransmission(Image<Gray, Byte> DC, int airlight) //DC - darkChannel
        {
            double w = 0.75;
            //double w = 0.95;
            Image<Gray, Byte> transmission = DC.Clone();
            MCvScalar intensity;

            for (int m = 0; m < DC.Rows; m++)
            {
                for (int n = 0; n < DC.Cols; n++)
                {
                    intensity = DC[m, n].MCvScalar;
                    transmission[m, n] = new Gray((1 - w * (intensity.V0 / airlight)) * 255);
                }
            }
            return transmission;
        }

        //dehazing foggy image
        private Image<Bgr, Byte> RemoveFog(Image<Bgr, Byte> sourceImg, Image<Gray, Byte> transmissionImg, int airlight)
        {
            double t0 = 0.1;
            double tmax;

            int A = airlight;//airlight
//            Scalar t; //transmission
            Gray t; //transmission
            Bgr I; //I(x) - source image pixel
                     //            Mat dehazed = Mat::zeros(sourceImg.rows, sourceImg.cols, CV_8UC3);
            Image<Bgr, Byte> dehazed = new Image<Bgr, Byte>(sourceImg.Size);

            for (int i = 0; i < sourceImg.Rows; i++)
            {
                for (int j = 0; j < sourceImg.Cols; j++)
                {
                    t = transmissionImg[i, j];
                    I = sourceImg[i, j];
                    tmax = (t.Intensity / 255) < t0 ? t0 : (t.Intensity / 255);

                    double B = Math.Abs((I.Blue - A) / tmax + A) > 255 ? 255 : Math.Abs((I.Blue - A) / tmax + A);
                    double G = Math.Abs((I.Green - A) / tmax + A) > 255 ? 255 : Math.Abs((I.Green - A) / tmax + A);
                    double R = Math.Abs((I.Red - A) / tmax + A) > 255 ? 255 : Math.Abs((I.Red - A) / tmax + A);

                    dehazed[i,j] = new Bgr(B, G, R);
                }
            }
            return dehazed;
        }

        #endregion 

        #region RobbyTanMethod

        // Articles review - http://www.ijcea.com/wp-content/uploads/2014/06/RUCHIKA_SHARMA_et_al.pdf
        // Article - http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.329.7924&rep=rep1&type=pdf
        public Image<Emgu.CV.Structure.Bgr, Byte> RemoveUsingRobbyTanMethod(Image<Bgr, Byte> image, FogRemovalParams _params)
        {
            Image<Bgr, Byte> I = image.Clone();
            Image<Bgr, Byte> result = image.Clone();

            // 1 - Estimate atmospheric light L_oo
            // find spot with highest initsity in image I
            double[] L = new double[3];
            double[] minValues;
            double[] maxValues;
            Point[] minLocations;
            Point[] maxLocations;
            image.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);
            L[0] = maxValues[0];
            L[1] = maxValues[1];
            L[2] = maxValues[2];

            // 2 - Compute light chromaticity alpha from L_oo (eq. 3)
            double[] alpha = new double[3];
            alpha[0] = L[0] / (L[0] + L[1] + L[2]);
            alpha[1] = L[1] / (L[0] + L[1] + L[2]);
            alpha[2] = L[2] / (L[0] + L[1] + L[2]);

            // 3 - Remove illumination color of I
            Image<Bgr, Byte> I_ = new Image<Bgr, byte>(image.Size);
            for (int i = 0; i < I.Rows; i++)
            {
                for (int j = 0; j < I.Cols; j++)
                {
                    Bgr currentPixel = I[i, j];
                    Bgr newPixel = new Bgr(
                            currentPixel.Blue / alpha[0],
                            currentPixel.Green / alpha[1],
                            currentPixel.Red / alpha[2]
                        );
                    I_[i, j] = newPixel;
                }
            }

            // 4 - Compute data cost from I_
            double LSum = L[0] + L[1] + L[2];
            int k = 20;

            RobbyTanPixelPhi[,] pixels_phis = new RobbyTanPixelPhi[I_.Rows, I_.Cols]; // for each pixel return vector with LSum - k dimension. (as in shelf - 2x2 -> 1 -> 1

            for (int i = 0; i < I_.Rows; i++)
            {
                for (int j = 0; j < I_.Cols; j++)
                {
                    Bgr pixel = I_[i, j];

                    // 4.1 - crop an nxn patch, p_x, from I_ centered at x
                    int patchSize = 5;
                    //int patchSize = 7;
                    Bgr[,] patch = GetImagePatch(patchSize, new Point(j, i), I_); // p_x
                    Bgr[] patchArray = this.ToArray<Bgr>(patch);

                    // 4.2 - for each value of A (A* is concrete value of A) 
                    for (int A_starred = 0; A_starred <= (int)(LSum - k); A_starred++)
                    {
                        // 4.2.1 - compute direct attenuation for the path [DGamma_]^*_x (using eq. 13) from A* and patch (p_x)
                        int A = A_starred;

                        // power (e^(-Beta d(x)) 
                        double power = (LSum - A) / LSum;
                        power = Math.Pow(power, -1);

                        double[,] DGamma_ = new double[patchArray.GetLength(0),3];
                        for (int s = 0; s < patchArray.GetLength(0); s++)
                        {
                            Bgr pixel2 = patchArray[s];
                            DGamma_[s, 0] = Math.Pow(pixel2.Blue - A, power);
                            DGamma_[s, 1] = Math.Pow(pixel2.Blue - A, power);
                            DGamma_[s, 2] = Math.Pow(pixel2.Blue - A, power);
                        }

                        // 4.2.2 - compute data cost phi(p_x|A_x) (eq. 18)
                        double[,] C_edges = new double[DGamma_.GetLength(0) - 1, 3];
                        double[,] phi = new double[DGamma_.GetLength(0) - 1, 3];
                        for (int s1 = 0, s2 = 1; s2 < patchArray.GetLength(0); s1++, s2++)
                        {
                            C_edges[s1, 0] = power * (DGamma_[s2,0] - DGamma_[s2 - 1, 0]); // Blue
                            C_edges[s1, 1] = power * (DGamma_[s2, 1] - DGamma_[s2 - 1, 1]); //Green
                            C_edges[s1, 2] = power * (DGamma_[s2, 2] - DGamma_[s2 - 1, 2]); //Red
                        }

                        double m = Max(C_edges); // constant to normalize C_edges, so that 0<=phi(p_x|A_x)<=1; m depends on size of the patch p_x
                        for (int s = 0; s < C_edges.GetLength(0); s++)
                        {
                            phi[s, 0] = C_edges[s,0] / m;
                            phi[s, 1] = C_edges[s,0] / m;
                            phi[s, 2] = C_edges[s,0] / m;
                        }

                        // return DataCost (phi)
                        // TODO - check this
                        pixels_phis[i, j] = pixels_phis[i, j] ?? new RobbyTanPixelPhi();
                        pixels_phis[i, j].PixelPhi.Add(phi);
                    }
                }
            }


            if (_params.ShowWindows)
            {
                EmguCvWindowManager.Display(image, "0 image");
                EmguCvWindowManager.Display(I_, "3 - Remove illumination color of I");
                EmguCvWindowManager.Display(result, "100 result");
            }

            return result;
        }

        private Bgr[,] GetImagePatch(int patchSize, Point centerPixel, Image<Bgr, Byte> image)
        {
            if (patchSize % 2 == 0 || patchSize <= 1 || patchSize >= Math.Min(image.Rows, image.Cols))
                throw new ArgumentException("patchSize must be odd number, great or equal than 3, less than image size");

            Bgr[,] patch = new Bgr[patchSize, patchSize];

            // save center pixel
            int center = (int)patchSize / (int)2;
            patch[center, center] = image[centerPixel.Y, centerPixel.X];

            for (int m = 0, i = centerPixel.Y - center; i <= centerPixel.Y + center; m++, i++)
            {
                for (int n = 0, j = centerPixel.X - center; j <= centerPixel.X + center; n++, j++)
                {
                    // check current location belongs to image
                    if(i >= 0 && i < image.Rows && j >= 0 && j < image.Cols)
                    {
                        Bgr pixel = image[i, j];
                        patch[m, n] = new Bgr(pixel.Blue, pixel.Green, pixel.Red);
                    }
                    else
                    {
                        // save zero pixel
                        patch[m, n] = new Bgr(0, 0, 0);
                    }
                }
            }

            return patch;
        }

        private T[] ToArray<T>(T[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            int length = rows * cols;
            T[] array = new T[length];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    int index = i * cols + i;
                    array[index] = matrix[i, j];
                }
            }
            return array;
        }

        private double RowMax(double [,] arr, int rowIndex)
        {
            return (from col in Enumerable.Range(0, arr.GetLength(1))
                    select arr[rowIndex, col]).Max();
        }

        private double[] RowsMax(double[,] arr)
        {
            return (from row in Enumerable.Range(0, arr.GetLength(0))
                    let cols = Enumerable.Range(0, arr.GetLength(1))
                    select cols.Max(col => arr[row, col])).ToArray();
        }

        private double Max(double[,] arr)
        {
            return RowsMax(arr).Max();
        }

        #endregion

        #region PixelBasedMedianChannelPrior (Pixel-based MCP)

        // Source - http://onlinepresent.org/proceedings/vol98_2015/31.pdf
        public Image<Emgu.CV.Structure.Bgr, Byte> RemoveFogUsingMedianChannelPrior(Image<Bgr, Byte> image, out Image<Gray, Byte> transmission, FogRemovalParams _params)
        {
            Image<Bgr, Byte> imgFog = image.Clone();
            Image<Gray, Byte> imgDarkChannel = new Image<Gray, byte>(image.Size);
            Image<Bgr, Byte> result = new Image<Bgr, byte>(image.Size);

            // 1 - median channel
            //Image<Gray, Byte> J_median = new Image<Gray, byte>(image.Size);
            //for (int m = 0; m < image.Rows; m++)
            //{
            //    for (int n = 0; n < image.Cols; n++)
            //    {
            //        Bgr pixel = image[m, n];
            //        J_median[m, n] = new Gray((pixel.Blue + pixel.Green + pixel.Red) / 3);
            //    }
            //}

            //double[] J_median = new double[3];
            //for (int m = 0; m < image.Rows; m++)
            //{
            //    for (int n = 0; n < image.Cols; n++)
            //    {
            //        Bgr pixel = image[m, n];
            //        J_median[0] += pixel.Blue;
            //        J_median[1] += pixel.Green;
            //        J_median[2] += pixel.Red;
            //    }
            //}
            //J_median[0] = J_median[0] / (image.Rows * image.Cols);
            //J_median[1] = J_median[1] / (image.Rows * image.Cols);
            //J_median[2] = J_median[2] / (image.Rows * image.Cols);

            // 2 - airlight
            double Airlight;
            double[] minValues = new double[3];
            double[] maxValues = new double[3];
            Point[] minLocations = new Point[3];
            Point[] maxLocations = new Point[3];
            image.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);
            Airlight = Math.Max(maxValues[0], Math.Max(maxValues[1], maxValues[2]));

            // 3 - transmission map
            double w = 0.75; // 0 < w <= 1
            Image<Gray, Byte> T = new Image<Gray, byte>(image.Size);
            for (int m = 0; m < image.Rows; m++)
            {
                for (int n = 0; n < image.Cols; n++)
                {
                    Bgr pixel = image[m, n];
                    double B = pixel.Blue / Airlight;
                    double G = pixel.Green / Airlight;
                    double R = pixel.Red / Airlight;
                    double transmission_ = 1 - w * ((B + G + R) / 3);
                    T[m, n] = new Gray(transmission_ * 255);
                }
            }
            transmission = T; // pass transmission out of function

            // 4 - recover image
            double A = Airlight;
            for (int m = 0; m < image.Rows; m++)
            {
                for (int n = 0; n < image.Cols; n++)
                {
                    double t = T[m, n].Intensity / 255;
                    Bgr I = image[m, n];

                    double B = (I.Blue - A) / t + A;
                    double G = (I.Green - A) / t + A;
                    double R = (I.Red - A) / t + A;

                    B = B > 255 ? 255 : B;
                    G = G > 255 ? 255 : G;
                    R = R > 255 ? 255 : R;

                    result[m, n] = new Bgr(B, G, R);
                }
            }

            if (_params.ShowWindows)
            {
                EmguCvWindowManager.Display(image, "1 image");
                // EmguCvWindowManager.Display(J_median, "2 J_median");
                EmguCvWindowManager.Display(T, "3 T");
                EmguCvWindowManager.Display(result, "4 result");
            }

            return result;
        }

        #endregion

        #region NEW INTEGRATED FOG REMOVAL ALGORITHM IDCP WITH CLAHE

        // Source - http://iraj.in/journal/journal_file/journal_pdf/4-54-140014656845-51.pdf
        public Image<Emgu.CV.Structure.Bgr, Byte> RemoveFogUsingIdcpWithClahe(Image<Bgr, Byte> image, out Image<Gray, Byte> transmission, FogRemovalParams _params)
        {
            Image<Lab, Byte> lab;
            Image<Bgr, Byte> clahe = new Image<Bgr, byte>(image.Size);
            Image<Gray, Byte> estimatedTransmissionDCP = new Image<Gray, byte>(image.Size);
            Image<Bgr, Byte> resultDCP;
            Image<Bgr, Byte> resultAdaptiveGammaCorrect;
            Image<Bgr, Byte> resultEqualizeHist;
            Image<Bgr, Byte> resultGammaCorrectt;
            Image<Bgr, Byte> result = new Image<Bgr, byte>(image.Size);

            // 1 - apply CLAHE
            lab = ImageHelper.ToLab(image); // convert to LAB
            Image <Gray, Byte>[] parts = lab.Split(); // split channels
            Image<Gray, Byte> LChannel = parts[0]; // get L channel
            Image<Gray, Byte> claheLChannel = new Image<Gray, byte>(image.Size); // CLAHE result
            CvInvoke.CLAHE(src: LChannel, clipLimit: 2, tileGridSize: new Size(8, 8), dst: claheLChannel);
            parts[0] = claheLChannel; // replace L with CLAHE
            lab = new Image<Lab, Byte>(parts); // save image
            clahe = ImageHelper.ToBgr(lab);

            // 2 - apply DCP
            resultDCP = RemoveFogUsingDarkChannelPrior(clahe, out estimatedTransmissionDCP, new FogRemovalParams() { ShowWindows = false });
            transmission = estimatedTransmissionDCP;

            // 3 - apply adaptive gamma correction
            // TODO
            resultAdaptiveGammaCorrect = GammaCorrection.Adaptive(resultDCP);

            // aplly gamma correction
            resultGammaCorrectt = resultDCP.Clone();
            resultGammaCorrectt._GammaCorrect(1.9);

            // apply histogram equalization
            resultEqualizeHist = resultDCP.Clone();
            resultEqualizeHist._EqualizeHist();


            result = resultAdaptiveGammaCorrect;

            if (_params.ShowWindows)
            {
                EmguCvWindowManager.Display(image, "1 image");
                EmguCvWindowManager.Display(clahe, "2 clahe");
                EmguCvWindowManager.Display(resultDCP, "3 resultDCP");
                EmguCvWindowManager.Display(resultAdaptiveGammaCorrect, "5 resultAdaptiveGammaCorrect");
                EmguCvWindowManager.Display(resultGammaCorrectt, "5.2 resultGammaCorrectt");
                EmguCvWindowManager.Display(resultEqualizeHist, "5.3 resultEqualizeHist");
                EmguCvWindowManager.Display(result, "5 result");
            }

            return result;
        }

        #endregion
    }

    class RobbyTanPixelPhi
    {
        public List<double[,]> PixelPhi{ get; set; }

        public RobbyTanPixelPhi()
        {
            PixelPhi = new List<double[,]>();
        }
        public RobbyTanPixelPhi(List<double[,]> pixelPhi)
        {
            PixelPhi = pixelPhi;
        }
    }
}