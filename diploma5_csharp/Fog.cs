using System;
using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using diploma5_csharp.Models;
using System.Linq;
using diploma5_csharp.Helpers;
using System.Diagnostics;

namespace diploma5_csharp
{
    public class Fog
    {
        #region DarkChannelPrior (Patch-based DCP)

        public BaseMethodResponse RemoveFogUsingDarkChannelPrior(Image<Bgr, Byte> image, FogRemovalParams _params)
        {
            Image<Bgr, Byte> imgFog = image.Clone();
            Image<Gray, Byte> imgDarkChannel = new Image<Gray, byte>(image.Size);
            Image<Gray, Byte> T = new Image<Gray, byte>(image.Size);
            Image<Gray, Byte> transmission;
            Image<Bgr, Byte> result = image.Clone();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            int Airlight;

            //int patchSize = 5;
            int patchSize = 7;
            imgDarkChannel = GetDarkChannel(imgFog, patchSize);
            Airlight = EstimateAirlight(imgDarkChannel, imgFog);
            T = EstimateTransmission(imgDarkChannel, Airlight);
            result = RemoveFog(imgFog, T, Airlight);

            //Return out params
            transmission = T;

            stopwatch.Stop();

            if (_params.ShowWindows)
            {
                EmguCvWindowManager.Display(imgDarkChannel, "1 imgDarkChannel darkChannel MDCP");
                EmguCvWindowManager.Display(T, "2 estimateTransmission");
                EmguCvWindowManager.Display(imgFog, "3 imgFog");
                EmguCvWindowManager.Display(result, "4 result");
            }

            var Metrics = ImageMetricHelper.ComputeAll(image.Convert<Bgr, double>(), result.Convert<Bgr, double>());
            return new BaseMethodResponse
            {
                EnhancementResult = result,
                DetectionResult = transmission,
                Metrics = Metrics,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
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

                    //double B = Math.Abs((I.Blue - A) / tmax + A) > 255 ? 255 : Math.Abs((I.Blue - A) / tmax + A);
                    //double G = Math.Abs((I.Green - A) / tmax + A) > 255 ? 255 : Math.Abs((I.Green - A) / tmax + A);
                    //double R = Math.Abs((I.Red - A) / tmax + A) > 255 ? 255 : Math.Abs((I.Red - A) / tmax + A);

                    double B = (I.Blue - A) / tmax + A;
                    double G = (I.Green - A) / tmax + A;
                    double R = (I.Red - A) / tmax + A;

                    dehazed[i,j] = new Bgr(B, G, R);
                }
            }
            return dehazed;
        }

        #endregion 

        #region RobbyTanMethod

        // Articles review - http://www.ijcea.com/wp-content/uploads/2014/06/RUCHIKA_SHARMA_et_al.pdf
        // Article - http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.329.7924&rep=rep1&type=pdf
        public BaseMethodResponse RemoveUsingRobbyTanMethod(Image<Bgr, Byte> image, FogRemovalParams _params)
        {
            Image<Bgr, Byte> I = image.Clone();
            Image<Gray, Byte> transmission = new Image<Gray, byte>(image.Size);
            Image<Bgr, Byte> result = image.Clone();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

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

            stopwatch.Stop();

            if (_params.ShowWindows)
            {
                EmguCvWindowManager.Display(image, "0 image");
                EmguCvWindowManager.Display(I_, "3 - Remove illumination color of I");
                EmguCvWindowManager.Display(result, "100 result");
            }

            var Metrics = ImageMetricHelper.ComputeAll(image.Convert<Bgr, double>(), result.Convert<Bgr, double>());
            return new BaseMethodResponse
            {
                EnhancementResult = result,
                DetectionResult = transmission,
                Metrics = Metrics,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
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
        public BaseMethodResponse RemoveFogUsingMedianChannelPrior(Image<Bgr, Byte> image, FogRemovalParams _params)
        {
            Image<Bgr, Byte> imgFog = image.Clone();
            Image<Gray, Byte> imgDarkChannel = new Image<Gray, byte>(image.Size);
            Image<Gray, Byte> transmission = new Image<Gray, byte>(image.Size);
            Image<Bgr, Byte> result = new Image<Bgr, byte>(image.Size);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

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

            stopwatch.Stop();

            if (_params.ShowWindows)
            {
                EmguCvWindowManager.Display(image, "1 image");
                // EmguCvWindowManager.Display(J_median, "2 J_median");
                EmguCvWindowManager.Display(T, "3 T");
                EmguCvWindowManager.Display(result, "4 result");
            }

            var Metrics = ImageMetricHelper.ComputeAll(image.Convert<Bgr, double>(), result.Convert<Bgr, double>());
            return new BaseMethodResponse
            {
                EnhancementResult = result,
                DetectionResult = transmission,
                Metrics = Metrics,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }

        #endregion

        #region NEW INTEGRATED FOG REMOVAL ALGORITHM IDCP WITH CLAHE

        // Source - http://iraj.in/journal/journal_file/journal_pdf/4-54-140014656845-51.pdf
        public BaseMethodResponse RemoveFogUsingIdcpWithClahe(Image<Bgr, Byte> image, FogRemovalParams _params)
        {
            Image<Lab, Byte> lab;
            Image<Bgr, Byte> clahe = new Image<Bgr, byte>(image.Size);
            Image<Bgr, Byte> resultAdaptiveGammaCorrect;
            Image<Bgr, Byte> resultEqualizeHist;
            Image<Bgr, Byte> resultGammaCorrectt;
            Image<Gray, Byte> transmission = new Image<Gray, byte>(image.Size);
            Image<Bgr, Byte> result = new Image<Bgr, byte>(image.Size);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

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
            var resultDCP = RemoveFogUsingDarkChannelPrior(clahe, new FogRemovalParams() { ShowWindows = false });
            transmission = resultDCP.DetectionResult;

            // 3 - apply adaptive gamma correction
            resultAdaptiveGammaCorrect = GammaCorrection.Adaptive(resultDCP.EnhancementResult);

            // aplly gamma correction
            resultGammaCorrectt = resultDCP.EnhancementResult.Clone();
            resultGammaCorrectt._GammaCorrect(1.9);

            // apply histogram equalization
            resultEqualizeHist = resultDCP.EnhancementResult.Clone();
            resultEqualizeHist._EqualizeHist();


            result = resultAdaptiveGammaCorrect;

            stopwatch.Stop();

            if (_params.ShowWindows)
            {
                EmguCvWindowManager.Display(image, "1 image");
                EmguCvWindowManager.Display(clahe, "2 clahe");
                EmguCvWindowManager.Display(resultDCP.EnhancementResult, "3 resultDCP");
                EmguCvWindowManager.Display(resultAdaptiveGammaCorrect, "5 resultAdaptiveGammaCorrect");
                EmguCvWindowManager.Display(resultGammaCorrectt, "5.2 resultGammaCorrectt");
                EmguCvWindowManager.Display(resultEqualizeHist, "5.3 resultEqualizeHist");
                EmguCvWindowManager.Display(result, "5 result");
            }

            var Metrics = ImageMetricHelper.ComputeAll(image.Convert<Bgr, double>(), result.Convert<Bgr, double>());
            return new BaseMethodResponse
            {
                EnhancementResult = result,
                DetectionResult = transmission,
                Metrics = Metrics,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }

        #endregion

        #region Robby T. Tan - Visibility enhacement for roads with foggy or hazy scenes
        
        // Source - http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.112.6005&rep=rep1&type=pdf
        public BaseMethodResponse EnhaceVisibilityUsingRobbyTanMethodForRoads(Image<Bgr, Byte> image, FogRemovalParams _params)
        {
            Image<Bgr, Byte> E = image;
            Image<Bgr, double> E_normilized = new Image<Bgr, double>(image.Size);
            Image<Gray, double> F = new Image<Gray, double>(image.Size);
            Image<Gray, double> e_beta_dx = new Image<Gray, double>(image.Size);
            Image<Gray, Byte> transmission = new Image<Gray, Byte>(image.Size);
            Image<Bgr, Byte> result = new Image<Bgr, Byte>(image.Size);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // 1 - compute environmental light (as max values)
            double I_b, I_g, I_r;
            double[] minValues = new double[3];
            double[] maxValues = new double[3];
            Point[] minLocations = new Point[3];
            Point[] maxLocations = new Point[3];
            image.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);
            I_b = maxValues[0];
            I_g = maxValues[1];
            I_r = maxValues[2];

            double Airlight = Math.Max(maxValues[0], Math.Max(maxValues[1], maxValues[2]));
            //double Airlight = (maxValues[0] + maxValues[1] + maxValues[2]) / 3.0;

            // compute Г_c
            //double G_b = I_b / (I_b + I_g + I_r);
            //double G_g = I_g / (I_b + I_g + I_r);
            //double G_r = I_r / (I_b + I_g + I_r);

            double G_b = I_b / 255;
            double G_g = I_g / 255;
            double G_r = I_r / 255;

            //double G_b = I_b / Airlight;
            //double G_g = I_g / Airlight;
            //double G_r = I_r / Airlight;

            // compute normilized input images E'_c
            for (int m = 0; m < E.Rows; m++)
            {
                for (int n = 0; n < E.Cols; n++)
                {
                    Bgr e = E[m, n];

                    double B = E[m, n].Blue / G_b;
                    double G = E[m, n].Green / G_g;
                    double R = E[m, n].Red / G_r;

                    //double B = E[m, n].Blue * G_b;
                    //double G = E[m, n].Green * G_g;
                    //double R = E[m, n].Red * G_r;

                    // take mod to cut hight value (my assumption)
                    B = B % 255;
                    G = G % 255;
                    R = R % 255;

                    E_normilized[m, n] = new Bgr(B, G, R);
                }
            }

            // compute F based on the intensity value of the YIQ color model
            for (int m = 0; m < E_normilized.Rows; m++)
            {
                for (int n = 0; n < E_normilized.Cols; n++)
                {
                    //double Y = 0.257 * E_normilized[m, n].Blue + 0.504 * E_normilized[m, n].Green + 0.098 * E_normilized[m, n].Red;
                    double Y = 0.257 * E_normilized[m, n].Red + 0.504 * E_normilized[m, n].Green + 0.098 * E_normilized[m, n].Blue;
                    F[m, n] = new Gray(Y);
                }
            }

            // values of Y are approximated values, thus to create
            // a better approximation, we diffuse these values by using
            // Gaussian blur
            F = F.SmoothGaussian(3);

            // estimate the approximated value of (1 - e^(-beta*d(x))) by using Eq. (7)
            // Eq. (7): F = (I_r+I_g+I_b)(1 - e^(-beta*d(x))) =>
            // (1 - e^(-beta*d(x))) = F / (I_r+I_g+I_b) =>
            // e^(-beta*d(x)) = 1 - F / (I_r+I_g+I_b)
            for (int m = 0; m < E_normilized.Rows; m++)
            {
                for (int n = 0; n < E_normilized.Cols; n++)
                {
                    //double e_beta_dx_ = 1 - (F[m,n].Intensity / (I_b + I_g + I_r));
                    double e_beta_dx_ = Math.Pow(1 - (F[m, n].Intensity / 255), -1);
                    e_beta_dx[m, n] = new Gray(e_beta_dx_);
                }
            }

            // enhance the visibility of the input image
            for (int m = 0; m < E_normilized.Rows; m++)
            {
                for (int n = 0; n < E_normilized.Cols; n++)
                {
                    double B = (E[m,n].Blue - F[m,n].Intensity * G_b) * e_beta_dx[m,n].Intensity;
                    double G = (E[m,n].Green - F[m,n].Intensity * G_g) * e_beta_dx[m,n].Intensity;
                    double R = (E[m,n].Red - F[m,n].Intensity * G_r) * e_beta_dx[m,n].Intensity;

                    result[m, n] = new Bgr(B, G, R);
                }
            }

            var result2 = GammaCorrection.Adaptive(result);
            var result3 = result.Clone();
            result3._EqualizeHist();

            stopwatch.Stop();

            if (_params.ShowWindows)
            {
                EmguCvWindowManager.Display(image, "1 image");
                EmguCvWindowManager.Display(E_normilized.Convert<Bgr, Byte>(), "2 E_normilized");
                EmguCvWindowManager.Display(F.Convert<Bgr, Byte>(), "3 F");
                EmguCvWindowManager.Display(e_beta_dx.Convert<Bgr, Byte>(), "4 e_beta_dx");
                EmguCvWindowManager.Display(result, "5 result");
                EmguCvWindowManager.Display(result2, "5.2 result2");
                EmguCvWindowManager.Display(result3, "5.3 result3");
            }

            var Metrics = ImageMetricHelper.ComputeAll(image.Convert<Bgr, double>(), result.Convert<Bgr, double>());
            return new BaseMethodResponse
            {
                EnhancementResult = result,
                DetectionResult = transmission,
                Metrics = Metrics,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }

        #endregion

        #region An approach which is based on Fast Fourier Transform

        // Source - http://www.sciencedirect.com/science/article/pii/S1877050915013812
        public BaseMethodResponse RemoveFogUsingDCPAndDFT(Image<Bgr, Byte> image, FogRemovalParams _params)
        {
            Image<Bgr, Single> imageSingle = image.Convert<Bgr, Single>();
            Image<Gray, Byte> DC;
            Image<Gray, Single> DFT = new Image<Gray, Single>(image.Size);
            Image<Gray, Single> DFT_inverse = new Image<Gray, Single>(image.Size);
            Image<Gray, Single> DFT_lowPassFilter = new Image<Gray, Single>(image.Size);
            Image<Gray, Single> DFT_highPassFilter = new Image<Gray, Single>(image.Size);
            Image<Gray, Single> transmission_ = new Image<Gray, Single>(image.Size);
            Image<Gray, Byte> transmission;
            Image<Bgr, Byte> result = new Image<Bgr, Byte>(image.Size);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // compute DCP
            DC = GetDarkChannel(image, patchSize: 7);

            // apply fast fourier transform (FFT)
            // for digital images use discrete fourier transform (DFT)
            CvInvoke.Dft(src: DC.Convert<Gray, Single>(), dst: DFT, flags: DxtType.Forward, nonzeroRows: -1);
            CvInvoke.Dft(src: DFT, dst: DFT_inverse, flags: DxtType.Inverse, nonzeroRows: -1);

            float[,] lowPassMatrixKernel = new float[3, 3] {
                    { 1.0f / 9.0f, 1.0f / 9.0f, 1.0f / 9.0f},
                    { 1.0f / 9.0f, 1.0f / 9.0f, 1.0f / 9.0f},
                    { 1.0f / 9.0f, 1.0f / 9.0f, 1.0f / 9.0f},
                };
            ConvolutionKernelF lowPassMatrix = new ConvolutionKernelF(lowPassMatrixKernel);
            float[,] highPassMatrixKernel = new float[3, 3] {
                    { 0, - 1.0f / 4.0f , 0},
                    { - 1.0f / 4.0f, 2 , - 1.0f / 4.0f},
                    { 0,  - 1.0f / 4.0f, 0},
                };
            ConvolutionKernelF highPassMatrix = new ConvolutionKernelF(highPassMatrixKernel);
            CvInvoke.Filter2D(src: DFT, dst: DFT_lowPassFilter, kernel: lowPassMatrix, anchor: new Point(-1, -1));
            CvInvoke.Filter2D(src: DFT, dst: DFT_highPassFilter, kernel: highPassMatrix, anchor: new Point(-1, -1));

            for (int m = 0; m < DFT.Rows; m++)
            {
                for (int n = 0; n < DFT.Cols; n++)
                {
                    double sum = DFT_lowPassFilter[m, n].Intensity + DFT_highPassFilter[m, n].Intensity;
                    transmission_[m, n] = new Gray(sum);
                }
            }

            // compute transmission map
            transmission = new Image<Gray, byte>(image.Size);

            // estimate airlight
            double airlight = EstimateAirlight(DC, image);

            // restore image


            stopwatch.Stop();

            if (_params.ShowWindows)
            {
                EmguCvWindowManager.Display(image, "1 image");
                EmguCvWindowManager.Display(DC, "2 DC");
                EmguCvWindowManager.Display(DFT, "3 DFT");
                EmguCvWindowManager.Display(DFT_inverse, "4 DFT_inverse");
                EmguCvWindowManager.Display(DFT_lowPassFilter, "5.1 DFT_lowPassFilter");
                EmguCvWindowManager.Display(DFT_highPassFilter, "5.2 DFT_highPassFilter");
                EmguCvWindowManager.Display(transmission_, "6 transmission_");
                EmguCvWindowManager.Display(result, "9 result");
            }

            var Metrics = ImageMetricHelper.ComputeAll(image.Convert<Bgr, double>(), result.Convert<Bgr, double>());
            return new BaseMethodResponse
            {
                EnhancementResult = result,
                DetectionResult = transmission,
                Metrics = Metrics,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }

        #endregion

        #region Single Image Fog Removal Based on Local Extrema

        // Source - http://html.rhhz.net/ieee-jas/html/20150205.htm
        public BaseMethodResponse RemoveFogUsingLocalExtremaMethod(Image<Bgr, Byte> image, FogRemovalParams _params)
        {
            Image<Bgr, Byte> I = image;
            Image<Gray, Byte> DC;
            Image<Gray, Byte> transmission;
            Image<Bgr, Byte> result;

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // 1. compute dark channel
            DC = GetDarkChannel(I, patchSize: 7);

            // 2. Skylight Estimation and White Balance Skylight A is typically assumed to be a global constant
            // In order to solve A,we choose the 0.1% brightest pixels of the dark channel as their preferred region
            double takePercent = 0.001;
            int takeAmount = (int)Math.Round((DC.Rows * DC.Cols) * takePercent);
            var pixelsWithPositions = ImageHelper.GetImagePixelsWithPositions(DC);
            var brightestPixels = pixelsWithPositions
                .OrderByDescending(x => x.Intensity)
                .Take(takeAmount)
                .ToArray();

            // take according pixels in original image
            var brightestOriginalPixels = brightestPixels.Select(x => I[x.Position.X, x.Position.Y]).ToArray();

            // compute A_mean for each channel
            double A_mean_b = brightestOriginalPixels.Average(x => x.Blue);
            double A_mean_g = brightestOriginalPixels.Average(x => x.Green);
            double A_mean_r = brightestOriginalPixels.Average(x => x.Red);
            double A_mean = (A_mean_b + A_mean_g + A_mean_r) / 3.0;

            // compute skylight A
            double A_mean_max = StatisticsHelper.Max(A_mean_b, A_mean_g, A_mean_r);
            double A = A_mean / A_mean_max;

            // apply white balance to image I to obtain corrected image I'
            Image<Bgr, double> I_corrected = new Image<Bgr, double>(I.Size);
            for (int m = 0; m < I.Rows; m++)
            {
                for (int n = 0; n < I.Cols; n++)
                {
                    double B2 = I[m, n].Blue / A;
                    double G2 = I[m, n].Green / A;
                    double R2 = I[m, n].Red / A;

                    double B = I[m, n].Blue / (A_mean_b / A_mean_max);
                    double G = I[m, n].Green / (A_mean_g / A_mean_max);
                    double R = I[m, n].Red / (A_mean_r / A_mean_max);

                    I_corrected[m, n] = new Bgr(B, G, R);
                }
            }

            // Coarse Estimation of Atmospheric Veil (estimate transmission)
            Image<Gray, double> V = new Image<Gray, double>(I_corrected.Size);
            for (int m = 0; m < I.Rows; m++)
            {
                for (int n = 0; n < I.Cols; n++)
                {
                    double B = I_corrected[m, n].Blue;
                    double G = I_corrected[m, n].Green;
                    double R = I_corrected[m, n].Red;
                    double min = StatisticsHelper.Min(B, G, R);
                    V[m, n] = new Gray(255 - min);
                }
            }

            // apply an edge-preserving smoothing approach based on the local extrema for refinement
            //V.SmoothBilatral();
            V = V.SmoothGaussian(5);

            transmission = V.Convert<Gray, Byte>();

            // remove fog
            double q = 0.95; // q id used for regulating the degree of defogging
            Image<Bgr, Byte> R_image = new Image<Bgr, Byte>(I.Size);
            for (int m = 0; m < I.Rows; m++)
            {
                for (int n = 0; n < I.Cols; n++)
                {
                    double v = V[m, n].Intensity / 255.0;
                    //double v = V[m, n].Intensity;

                    //double B_ = I[m, n].Blue / 255.0;
                    //double G_ = I[m, n].Green / 255.0;
                    //double R_ = I[m, n].Red / 255.0;

                    double B_ = I[m, n].Blue;
                    double G_ = I[m, n].Green;
                    double R_ = I[m, n].Red;

                    //double B = ((B_ - q * v) / (1 - v)) * A;
                    //double G = ((G_ - q * v) / (1 - v)) * A;
                    //double R = ((R_ - q * v) / (1 - v)) * A;

                    double B = ((I_corrected[m,n].Blue / 255 - q * v) / (1 - v));
                    double G = ((I_corrected[m, n].Green / 255 - q * v) / (1 - v));
                    double R = ((I_corrected[m, n].Red / 255 - q * v) / (1 - v));

                    B = (B * 255);
                    G = (G * 255);
                    R = (R * 255);

                    //B = (B % 255);
                    //G = (G % 255);
                    //R = (R % 255);

                    if (B > 255) B = 255;
                    if (G > 255) G = 255;
                    if (R > 255) R = 255;

                    if (B < 0) B = 0;
                    if (G < 0) G = 0;
                    if (R < 0) R = 0;

                    R_image[m, n] = new Bgr(B, G, R);
                }
            }
            result = GammaCorrection.Adaptive(R_image);

            stopwatch.Stop();

            if (_params.ShowWindows)
            {
                EmguCvWindowManager.Display(image, "1 image");
                EmguCvWindowManager.Display(DC, "2 DC");
                EmguCvWindowManager.Display(I_corrected.Convert<Bgr, Byte>(), "3 I_corrected");
                EmguCvWindowManager.Display(V.Convert<Gray, Byte>(), "4 V");
                EmguCvWindowManager.Display(R_image, "5 R_image");
                EmguCvWindowManager.Display(result, "9 result");
            }

            var Metrics = ImageMetricHelper.ComputeAll(image.Convert<Bgr, double>(), result.Convert<Bgr, double>());
            return new BaseMethodResponse
            {
                EnhancementResult = result,
                DetectionResult = transmission,
                Metrics = Metrics,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }

        #endregion

        #region Physics-based Fast Single Image Fog Removal 

        // Source - https://pdfs.semanticscholar.org/dfb8/39c695604ee2b0419a545eb9986be7a6d51d.pdf
        public BaseMethodResponse RemoveFogUsingPhysicsBasedMethod(Image<Bgr, Byte> image, FogRemovalParams _params)
        {
            Image<Bgr, Byte> I = image;
            Image<Gray, Byte> DC;
            Image<Gray, Byte> transmission = null;
            Image<Bgr, Byte> result = new Image<Bgr, Byte>(image.Size);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // 1. compute dark channel
            DC = GetDarkChannel(I, patchSize: 7);

            // 2. Skylight Estimation and White Balance Skylight A is typically assumed to be a global constant
            // In order to solve A,we choose the 0.1% brightest pixels of the dark channel as their preferred region
            double takePercent = 0.001;
            int takeAmount = (int)Math.Round((DC.Rows * DC.Cols) * takePercent);
            var pixelsWithPositions = ImageHelper.GetImagePixelsWithPositions(DC);
            var brightestPixels = pixelsWithPositions
                .OrderByDescending(x => x.Intensity)
                .Take(takeAmount)
                .ToArray();

            // take according pixels in original image
            var brightestOriginalPixels = brightestPixels.Select(x => I[x.Position.X, x.Position.Y]).ToArray();

            // compute A_mean for each channel
            double A_mean_b = brightestOriginalPixels.Average(x => x.Blue);
            double A_mean_g = brightestOriginalPixels.Average(x => x.Green);
            double A_mean_r = brightestOriginalPixels.Average(x => x.Red);
            double A_mean = (A_mean_b + A_mean_g + A_mean_r) / 3.0;

            // compute skylight A
            double A_mean_max = StatisticsHelper.Max(A_mean_b, A_mean_g, A_mean_r);
            double A = A_mean / A_mean_max;

            // apply white balance to image I to obtain corrected image I'
            Image<Bgr, double> I_corrected = new Image<Bgr, double>(I.Size);
            for (int m = 0; m < I.Rows; m++)
            {
                for (int n = 0; n < I.Cols; n++)
                {
                    double B2 = I[m, n].Blue / A;
                    double G2 = I[m, n].Green / A;
                    double R2 = I[m, n].Red / A;

                    double B = I[m, n].Blue / (A_mean_b / A_mean_max);
                    double G = I[m, n].Green / (A_mean_g / A_mean_max);
                    double R = I[m, n].Red / (A_mean_r / A_mean_max);

                    I_corrected[m, n] = new Bgr(B, G, R);
                }
            }

            // Coarse Estimation of Atmospheric Veil (estimate transmission)
            Image<Gray, double> V = new Image<Gray, double>(I_corrected.Size);
            for (int m = 0; m < I.Rows; m++)
            {
                for (int n = 0; n < I.Cols; n++)
                {
                    double B = I_corrected[m, n].Blue;
                    double G = I_corrected[m, n].Green;
                    double R = I_corrected[m, n].Red;
                    double min = StatisticsHelper.Min(B, G, R);
                    V[m, n] = new Gray(min);
                }
            }

            // apply an edge-preserving smoothing approach based on the local extrema for refinement
            //V.SmoothBilatral();
            V = V.SmoothGaussian(5);

            transmission = V.Convert<Gray, Byte>();

            // remove fog
            double q = 0.95; // q id used for regulating the degree of defogging
            Image<Bgr, Byte> R_image = new Image<Bgr, Byte>(I.Size);
            for (int m = 0; m < I.Rows; m++)
            {
                for (int n = 0; n < I.Cols; n++)
                {
                    //double v = V[m, n].Intensity / 255.0;
                    double v = V[m, n].Intensity;

                    //double B_ = I[m, n].Blue / 255.0;
                    //double G_ = I[m, n].Green / 255.0;
                    //double R_ = I[m, n].Red / 255.0;

                    double B_ = I[m, n].Blue;
                    double G_ = I[m, n].Green;
                    double R_ = I[m, n].Red;

                    double B = ((I_corrected[m, n].Blue / 255 - q * v) / (1 - v));
                    double G = ((I_corrected[m, n].Green / 255 - q * v) / (1 - v));
                    double R = ((I_corrected[m, n].Red / 255 - q * v) / (1 - v));

                    B = (B * 255);
                    G = (G * 255);
                    R = (R * 255);

                    B = (B % 255);
                    G = (G % 255);
                    R = (R % 255);

                    if (B > 255) B = 255;
                    if (G > 255) G = 255;
                    if (R > 255) R = 255;

                    if (B < 0) B = 0;
                    if (G < 0) G = 0;
                    if (R < 0) R = 0;

                    R_image[m, n] = new Bgr(B, G, R);
                }
            }
            result = R_image.Convert<Bgr, Byte>();

            stopwatch.Stop();

            if (_params.ShowWindows)
            {
                EmguCvWindowManager.Display(image, "1 image");
                EmguCvWindowManager.Display(DC, "2 DC");
                EmguCvWindowManager.Display(I_corrected, "3 I_corrected double");
                EmguCvWindowManager.Display(I_corrected.Convert<Bgr, Byte>(), "3 I_corrected");
                EmguCvWindowManager.Display(V.Convert<Gray, Byte>(), "4 V");
                EmguCvWindowManager.Display(result, "9 result");
            }

            var Metrics = ImageMetricHelper.ComputeAll(image.Convert<Bgr, double>(), result.Convert<Bgr, double>());
            return new BaseMethodResponse
            {
                EnhancementResult = result,
                DetectionResult = transmission,
                Metrics = Metrics,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }

        #endregion

        #region My Fog removal method

        public BaseMethodResponse RemoveFogUsingCustomMethod(Image<Bgr, Byte> image, FogRemovalParams _params)
        {
            Image<Bgr, float> convultionResult = new Image<Bgr, float>(image.Size);
            Image<Bgr, Byte> filter2DResult = new Image<Bgr, Byte>(image.Size);
            Image<Gray, Byte> transmission = null;
            Image<Bgr, Byte> result = new Image<Bgr, Byte>(image.Size);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // try filter2d
            float[,] matrixKernel = new float[3, 3] {
                { 0,-1, 0 },
                {-1, 5,-1 },
                { 0,-1, 0 }
            };
            ConvolutionKernelF matrix = new ConvolutionKernelF(matrixKernel);
            //convultionResult = image.Convolution(matrix);
            //Image<Bgr, Byte> BGRResult = convultionResult.Convert<Bgr, Byte>();
            //Image<Bgr, Byte> BGRResult2 = convultionResult.ConvertScale<byte>(1, 0);
            CvInvoke.Filter2D(image, filter2DResult, matrix, new Point(0, 0));
            //EmguCvWindowManager.Display(convultionResult, "1 convultionResult");
            //EmguCvWindowManager.Display(BGRResult, "1 BGRResult");
            //EmguCvWindowManager.Display(BGRResult2, "1 BGRResult2");
            EmguCvWindowManager.Display(filter2DResult, "1 filter2DResult");

            transmission = new Image<Gray, byte>(image.Size);

            stopwatch.Stop();

            if (_params.ShowWindows)
            {
                EmguCvWindowManager.Display(image, "1 image");
                EmguCvWindowManager.Display(result, "9 result");
            }

            var Metrics = ImageMetricHelper.ComputeAll(image.Convert<Bgr, double>(), result.Convert<Bgr, double>());
            return new BaseMethodResponse
            {
                EnhancementResult = result,
                DetectionResult = transmission,
                Metrics = Metrics,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
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