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
using Kaliko.ImageLibrary;
using Kaliko.ImageLibrary.Filters;
using System.IO;
using Accord.Imaging.Filters;

namespace diploma5_csharp
{
    public class Fog
    {
        #region DarkChannelPrior (Patch-based DCP)

        public BaseMethodResponse RemoveFogUsingDarkChannelPrior(Image<Bgr, Byte> image, FogRemovalParams _params)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            int patchSize = 7;
            //var DC = GetDarkChannel(image, patchSize);
            //int Airlight = EstimateAirlight(DC, image);
            //var T = EstimateTransmission(DC, Airlight);
            //var result = RemoveFog(image, T, Airlight);

            // try vendor (better results)
            var DC = DehazeDarkChannel.getMedianDarkChannel(image, patchSize);
            int Airlight = EstimateAirlight(DC, image);
            var T = DehazeDarkChannel.estimateTransmission(DC, Airlight);
            var result = DehazeDarkChannel.getDehazed(image, T, Airlight);

            stopwatch.Stop();

            if (_params.ShowWindows)
            {
                EmguCvWindowManager.Display(image, "3 image");
                EmguCvWindowManager.Display(DC, "1 DC");
                EmguCvWindowManager.Display(T, "2 T");
                EmguCvWindowManager.Display(result, "4 result");
            }

            return new BaseMethodResponse
            {
                EnhancementResult = result,
                DetectionResult = T,
                DetailedResults = new List<IInputArray> { image, DC, T, result },
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }

        private Image<Gray, Byte> GetDarkChannel(Image<Bgr, Byte> sourceImg, int patchSize)
        {
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
            rgbMinImg.Dispose();
            return MDCP;
        }

        //estimate airlight by the 0.1% brightest pixels in dark channel
//        private int EstimateAirlight(Image<Gray, Byte> DC, Image<Bgr, Byte> inputImage)
//        {
//            Image<Lab, Byte> inputImageLab = new Image<Lab, byte>(inputImage.Size);

//            double minDC = 0;
//            double maxDC = 0;
//            Point minDCLoc = new Point();
//            Point maxDCLoc = new Point();
//            int size = DC.Rows * DC.Cols;
//            double requiredPercent = 0.001; //0.1%
//            double requiredAmount = size * requiredPercent; //

//            CvInvoke.MinMaxLoc(DC, ref minDC, ref maxDC, ref minDCLoc, ref maxDCLoc);
//            double max = maxDC;
//            List<List<int>> brightestDarkChannelPixels = new List<List<int>>();
//            for (int k = 0; k < requiredAmount && max >= 0; max--)
//            {
//                for (int i = 0; i < DC.Rows; i++)
//                {
//                    bool _break = false;
//                    for (int j = 0; j < DC.Cols; j++)
//                    {
////                        uchar val = DC.at<uchar>(i, j);
//                        Gray val = DC[i, j];
//                        if (val.Intensity == max)
//                            brightestDarkChannelPixels.Add(new List<int>() { i, j });

//                        if (brightestDarkChannelPixels.Count >= requiredAmount - 1)
//                        {
//                            _break = true;
//                            break;
//                        }
//                    }
//                    if (_break)
//                        break;
//                }

//                if (brightestDarkChannelPixels.Count >= requiredAmount)
//                    break;
//            }

//            //take pixels with highest intensity in the input image
//            CvInvoke.CvtColor(inputImage, inputImageLab, ColorConversion.Bgr2Lab);
//            int airlight = -1;
//            for (int r = 0; r != brightestDarkChannelPixels.Count; r++)
//            {
//                int i = brightestDarkChannelPixels[r][0];
//                int j = brightestDarkChannelPixels[r][1];

//                Lab pixel = inputImageLab[i, j];

//                double L = pixel.X;
//                double intensity = L; //take Lab L as insetsity
//                if (intensity > airlight)
//                    airlight = (int)intensity;
//            }

//            /////////////////

//            return airlight;
//        }

        /// <summary>
        /// More elegant (and propeer) code to estimate A
        /// </summary>
        /// <param name="DC"></param>
        /// <param name="inputImage"></param>
        /// <returns></returns>
        private int EstimateAirlight(Image<Gray, Byte> DC, Image<Bgr, Byte> inputImage)
        {
            var lab = ImageHelper.ToLab(inputImage);
            double takePercent = 0.001; // take most dark transmission's pixels
            double takeAmount = (DC.Rows * DC.Cols) * takePercent;
            var brightestPixels = ImageHelper.GetImagePixelsWithCoords(DC).OrderByDescending(x => x.Intensity).Take((int)takeAmount);
            var mostBrightesDCPixelInImage = brightestPixels.OrderByDescending(x => {
                return lab[x.Coords.Row, x.Coords.Col].X;
            }).First();
            var A = lab[mostBrightesDCPixelInImage.Coords.Row, mostBrightesDCPixelInImage.Coords.Col].X; // take most dark
            lab.Dispose();
            return (int)A;
        }

        /// <summary>
        /// Estimates A from transmission that was obtained without using of Dark Channel
        /// </summary>
        /// <param name="T"></param>
        /// <param name="inputImage"></param>
        /// <returns></returns>
        private int EstimateAirlightByTransmission(Image<Gray, Byte> T, Image<Bgr, Byte> inputImage)
        {
            var lab = ImageHelper.ToLab(inputImage);
            double takePercent = 0.001; // take most dark transmission's pixels
            double takeAmount = (T.Rows * T.Cols) * takePercent;
            var mostDarkPixels = ImageHelper.GetImagePixelsWithCoords(T).OrderBy(x => x.Intensity).Take((int)takeAmount);
            var mostBrightesTransmissionPixelInImage = mostDarkPixels.OrderByDescending(x => {
                return lab[x.Coords.Row, x.Coords.Col].X;
            }).First();
            var A = lab[mostBrightesTransmissionPixelInImage.Coords.Row, mostBrightesTransmissionPixelInImage.Coords.Col].X; // take most dark

            lab.Dispose();

            return (int)A;

        }

        //estimate transmission map
        private Image<Gray, Byte> EstimateTransmission(Image<Gray, Byte> DC, int airlight) //DC - darkChannel
        {
            double w = 0.75;
            // double w = 0.95;
            Image<Gray, Byte> transmission = new Image<Gray, Byte>(DC.Size);

            for (int m = 0; m < DC.Rows; m++)
            {
                for (int n = 0; n < DC.Cols; n++)
                {
                    transmission[m, n] = new Gray((1 - w * (DC[m, n].Intensity / airlight)) * 255);
                }
            }
            return transmission;
        }

        //dehazing foggy image
        private Image<Bgr, Byte> RemoveFog(Image<Bgr, Byte> sourceImg, Image<Gray, Byte> transmissionImg, int airlight)
        {
            double t;
            double t0 = 0.1;

            int A = airlight;
            Bgr I; //I(x) - source image pixel
            Image<Bgr, Byte> dehazed = new Image<Bgr, Byte>(sourceImg.Size);

            for (int i = 0; i < sourceImg.Rows; i++)
            {
                for (int j = 0; j < sourceImg.Cols; j++)
                {
                    t = transmissionImg[i, j].Intensity / 255;
                    t = Math.Max(t, t0);
                    I = sourceImg[i, j];

                    double B = (I.Blue - A) / t + A;
                    double G = (I.Green - A) / t + A;
                    double R = (I.Red - A) / t + A;

                    B = B > 255 ? 255 : Math.Abs(B);
                    G = G > 255 ? 255 : Math.Abs(G);
                    R = R > 255 ? 255 : Math.Abs(R);

                    dehazed[i,j] = new Bgr(B, G, R);
                }
            }
            return dehazed;
        }

        #endregion 

        #region Robby T. Tan - Visibility enhacement for roads with foggy or hazy scenes

        // Source - http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.112.6005&rep=rep1&type=pdf
        public BaseMethodResponse EnhaceVisibilityUsingRobbyTanMethodForRoads(Image<Bgr, Byte> image, FogRemovalParams _params)
        {
            Image<Bgr, Byte> E = image;
            

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // 1 - compute environmental light (as max values)
            var minmax = ImageHelper.ImageMinMax(image);
            double I_b = minmax.MaxValues[0];
            double I_g = minmax.MaxValues[1];
            double I_r = minmax.MaxValues[2];

            double Airlight = Math.Max(minmax.MaxValues[0], Math.Max(minmax.MaxValues[1], minmax.MaxValues[2]));

            // compute Г_c
            //double G_b = I_b / (I_b + I_g + I_r);
            //double G_g = I_g / (I_b + I_g + I_r);
            //double G_r = I_r / (I_b + I_g + I_r);

            double G_b = I_b / 255.0;
            double G_g = I_g / 255.0;
            double G_r = I_r / 255.0;

            //double G_b = I_b / Airlight;
            //double G_g = I_g / Airlight;
            //double G_r = I_r / Airlight;

            // compute normilized input images E'_c
            Image<Bgr, double> E_normilized = new Image<Bgr, double>(image.Size);
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

            #region Stuff from original method to remove fog, but for dust we stop here

            //// compute F based on the intensity value of the YIQ color model
            //Image<Gray, double> F = new Image<Gray, double>(image.Size);
            //for (int m = 0; m < E_normilized.Rows; m++)
            //{
            //    for (int n = 0; n < E_normilized.Cols; n++)
            //    {
            //        //double Y = 0.257 * E_normilized[m, n].Blue + 0.504 * E_normilized[m, n].Green + 0.098 * E_normilized[m, n].Red;
            //        double Y = 0.257 * E_normilized[m, n].Red + 0.504 * E_normilized[m, n].Green + 0.098 * E_normilized[m, n].Blue;
            //        F[m, n] = new Gray(Y);
            //    }
            //}

            //// values of Y are approximated values, thus to create
            //// a better approximation, we diffuse these values by using
            //// Gaussian blur
            //F = F.SmoothGaussian(5);

            //// estimate the approximated value of (1 - e^(-beta*d(x))) by using Eq. (7)
            //// Eq. (7): F = (I_r+I_g+I_b)(1 - e^(-beta*d(x))) =>
            //// (1 - e^(-beta*d(x))) = F / (I_r+I_g+I_b) =>
            //// e^(-beta*d(x)) = 1 - F / (I_r+I_g+I_b)
            //Image<Gray, double> e_beta_dx = new Image<Gray, double>(image.Size);
            //for (int m = 0; m < E_normilized.Rows; m++)
            //{
            //    for (int n = 0; n < E_normilized.Cols; n++)
            //    {
            //        double e_beta_dx_ = 1 - (F[m, n].Intensity / (I_b + I_g + I_r));
            //        //double e_beta_dx_ = Math.Pow(1 - (F[m, n].Intensity / 255), -1);
            //        e_beta_dx[m, n] = new Gray(e_beta_dx_ * 255);
            //    }
            //}

            //// enhance the visibility of the input image
            //Image<Bgr, Byte> processed = new Image<Bgr, Byte>(image.Size);
            //for (int m = 0; m < E_normilized.Rows; m++)
            //{
            //    for (int n = 0; n < E_normilized.Cols; n++)
            //    {
            //        double e_beta_dx_ = e_beta_dx[m, n].Intensity / 255.0;
            //        double F_ = F[m, n].Intensity;

            //        double B = (E[m, n].Blue - F_ * G_b) * e_beta_dx_;
            //        double G = (E[m, n].Green - F_ * G_g) * e_beta_dx_;
            //        double R = (E[m, n].Red - F_ * G_r) * e_beta_dx_;

            //        B = B > 255 ? 255 : B < 0 ? 0 : B;
            //        G = G > 255 ? 255 : G < 0 ? 0 : G;
            //        R = R > 255 ? 255 : R < 0 ? 0 : R;

            //        processed[m, n] = new Bgr(B, G, R);
            //    }
            //}

            //// my modification
            //var postProcessed = ImageHelper.AdjustContrast(processed);

            #endregion

            //var E_normilized_AGC = GammaCorrection.Adaptive(E_normilized.Convert<Bgr, Byte>());
            //var E_normilized_AdjustContast = ImageHelper.AdjustContrast(E_normilized.Convert<Bgr, Byte>(), 5);
            var E_normilized_Gamma = E_normilized.Convert<Bgr, Byte>().Clone();
            E_normilized_Gamma._GammaCorrect(1.4);
            //EmguCvWindowManager.Display(E_normilized_AGC, "E_normilized_AGC");
            //EmguCvWindowManager.Display(E_normilized_AdjustContast, "E_normilized_AdjustContast");
            //EmguCvWindowManager.Display(E_normilized_Gamma, "E_normilized_Gamma");

            stopwatch.Stop();

            if (_params.ShowWindows)
            {
                EmguCvWindowManager.Display(image, "1 image");
                EmguCvWindowManager.Display(E_normilized.Convert<Bgr, Byte>(), "2 E_normilized");
                //EmguCvWindowManager.Display(F, "3 F");
                //EmguCvWindowManager.Display(F.Convert<Bgr, Byte>(), "3 F.Convert<Bgr, Byte>()");
                //EmguCvWindowManager.Display(e_beta_dx, "4 e_beta_dx");
                //EmguCvWindowManager.Display(e_beta_dx.Convert<Bgr, Byte>(), "4 e_beta_dx");
                //EmguCvWindowManager.Display(processed, "processed");
                //EmguCvWindowManager.Display(postProcessed, "postProcessed");
                EmguCvWindowManager.Display(E_normilized_Gamma, "E_normilized_Gamma");
            }

            return new BaseMethodResponse
            {
                EnhancementResult = E_normilized_Gamma,
                DetectionResult = new Image<Gray, Byte>(image.Size),
                DetailedResults = new List<IInputArray> { image, E_normilized.Convert<Bgr, Byte>(), E_normilized_Gamma },
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }

        #endregion

        #region PixelBasedMedianChannelPrior (Pixel-based MCP)

        // Source - http://onlinepresent.org/proceedings/vol98_2015/31.pdf
        public BaseMethodResponse RemoveFogUsingMedianChannelPrior(Image<Bgr, Byte> image, FogRemovalParams _params)
        {
            Image<Bgr, Byte> result = new Image<Bgr, byte>(image.Size);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // 1 - median channel
            Image<Gray, Byte> J_median = new Image<Gray, byte>(image.Size);
            for (int m = 0; m < image.Rows; m++)
            {
                for (int n = 0; n < image.Cols; n++)
                {
                    Bgr pixel = image[m, n];
                    J_median[m, n] = new Gray((pixel.Blue + pixel.Green + pixel.Red) / 3);
                }
            }

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
                    double transmission_ = 1 - w * ((B + G + R) / 3.0);
                    T[m, n] = new Gray(transmission_ * 255);
                }
            }

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
                EmguCvWindowManager.Display(J_median, "2 J_median");
                EmguCvWindowManager.Display(T, "3 T");
                EmguCvWindowManager.Display(result, "4 result");
            }

            return new BaseMethodResponse
            {
                EnhancementResult = result,
                DetectionResult = T,
                DetailedResults = new List<IInputArray> { image, J_median, T, result },
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }

        #endregion

        #region NEW INTEGRATED FOG REMOVAL ALGORITHM IDCP WITH CLAHE

        // Source - http://iraj.in/journal/journal_file/journal_pdf/4-54-140014656845-51.pdf
        public BaseMethodResponse RemoveFogUsingIdcpWithClahe(Image<Bgr, Byte> image, FogRemovalParams _params)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // 1 - apply CLAHE
            var lab = ImageHelper.ToLab(image); // convert to LAB
            Image <Gray, Byte>[] parts = lab.Split(); // split channels
            Image<Gray, Byte> LChannel = parts[0]; // get L channel
            Image<Gray, Byte> claheLChannel = new Image<Gray, byte>(image.Size); // CLAHE result
            CvInvoke.CLAHE(src: LChannel, clipLimit: 2, tileGridSize: new Size(8, 8), dst: claheLChannel);
            parts[0] = claheLChannel; // replace L with CLAHE
            lab.Dispose();
            lab = new Image<Lab, Byte>(parts); // save image
            var clahe = ImageHelper.ToBgr(lab);

            // 2 - apply DCP
            var resultDCP = RemoveFogUsingDarkChannelPrior(clahe, new FogRemovalParams() { ShowWindows = false });
            var transmission = resultDCP.DetectionResult;

            // 3 - apply adaptive gamma correction
            var resultAdaptiveGammaCorrect = GammaCorrection.Adaptive(resultDCP.EnhancementResult);

            var result = resultAdaptiveGammaCorrect;

            stopwatch.Stop();

            if (_params.ShowWindows)
            {
                EmguCvWindowManager.Display(image, "1 image");
                EmguCvWindowManager.Display(clahe, "2 clahe");
                EmguCvWindowManager.Display(transmission, "3 transmission");
                EmguCvWindowManager.Display(resultDCP.EnhancementResult, "4 resultDCP");
                EmguCvWindowManager.Display(resultAdaptiveGammaCorrect, "5 resultAdaptiveGammaCorrect");
                EmguCvWindowManager.Display(result, "5 result");
            }

            lab.Dispose();
            parts.ToList().ForEach(x => x.Dispose());
            LChannel.Dispose();
            claheLChannel.Dispose();

            return new BaseMethodResponse
            {
                EnhancementResult = result,
                DetectionResult = transmission,
                DetailedResults = new List<IInputArray> { image, clahe, transmission, resultDCP.EnhancementResult, result },
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }

        #endregion


        #region An approach which is based on Fast Fourier Transform

        // Source - http://www.sciencedirect.com/science/article/pii/S1877050915013812
        public BaseMethodResponse RemoveFogUsingDCPAndDFT(Image<Bgr, Byte> image, FogRemovalParams _params)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var DC = GetDarkChannel(image, patchSize: 7);
            var gray = DC;

            //var idealLowPassFilter = ImageHelper.IdealLowPassFilter(gray);
            //var idealHightPassFilter = ImageHelper.IdealHightPassFilter(gray);
            //var idealPassFiltersSum = idealLowPassFilter + idealHightPassFilter;

            //var butterworthLowPassFilter = ImageHelper.ButterworthLowPassFilter(gray);
            //var butterworthHightPassFilter = ImageHelper.ButterworthHightPassFilter(gray);
            //var butterworthPassFiltersSum = butterworthLowPassFilter + butterworthHightPassFilter;

            var gaussianLowPassFilter = ImageHelper.GaussianLowPassFilter(gray);
            var gaussianHightPassFilter = ImageHelper.GaussianHightPassFilter(gray);
            var gaussianPassFiltersSum = gaussianLowPassFilter + gaussianHightPassFilter;

            // compute transmission map
            //var transmission = butterworthPassFiltersSum.Convert<Gray, Byte>().Resize(image.Width, image.Height, Inter.Linear);
            var transmission = gaussianPassFiltersSum.Convert<Gray, Byte>().Resize(image.Width, image.Height, Inter.Linear);

            // inverse
            transmission = ImageHelper.Inverse(transmission);

            // smooth with median filter
            transmission = transmission.SmoothMedian(5);

            // estimate airlight
            int airlight = EstimateAirlight(DC, image);

            // restore image with DC
            var dcResult = RemoveFog(image, transmission, airlight);
            var result = dcResult;

            // use custom improvement
            var lab = ImageHelper.ToLab(image);
            var channels = lab.Split();
            var l = channels[0]; var lRes = new Image<Gray, Byte>(lab.Size);
            var a = channels[1];
            var b = channels[2];
            for (int i = 0; i < image.Rows; i++)
            {
                for (int j = 0; j < image.Cols; j++)
                {
                    var t = transmission[i, j].Intensity / 255.0;
                    t = Math.Max(t, 0.2);

                    double L = (l[i, j].Intensity - airlight) / t + airlight;
                    L = L > 255 ? 255 : Math.Abs(L);
                    lRes[i, j] = new Gray(L);
                }
            }

            var labRes = new Image<Lab, Byte>(new Image<Gray, Byte>[]{ lRes, a, b });
            var resultImproved = ImageHelper.ToBgr(labRes);

            // calibrate colors
            resultImproved = ImageHelper.CalibrateColorsWithHistogramScratching(resultImproved);

            stopwatch.Stop();

            if (_params.ShowWindows)
            {
                //EmguCvWindowManager.Display(idealLowPassFilter.Convert<Gray, Byte>(), "idealLowPassFilter");
                //EmguCvWindowManager.Display(idealHightPassFilter.Convert<Gray, Byte>(), "idealHightPassFilter");
                //EmguCvWindowManager.Display(idealPassFiltersSum.Convert<Gray, Byte>(), "passFiltersSum");

                //EmguCvWindowManager.Display(butterworthLowPassFilter.Convert<Gray, Byte>(), "butterworthLowPassFilter");
                //EmguCvWindowManager.Display(butterworthHightPassFilter.Convert<Gray, Byte>(), "butterworthHightPassFilter");
                //EmguCvWindowManager.Display(butterworthPassFiltersSum.Convert<Gray, Byte>(), "butterworthPassFiltersSum");

                EmguCvWindowManager.Display(gaussianLowPassFilter.Convert<Gray, Byte>(), "gaussianLowPassFilter");
                EmguCvWindowManager.Display(gaussianHightPassFilter.Convert<Gray, Byte>(), "gaussianHightPassFilter");
                EmguCvWindowManager.Display(gaussianPassFiltersSum.Convert<Gray, Byte>(), "gaussianPassFiltersSum");

                EmguCvWindowManager.Display(image, "image");
                EmguCvWindowManager.Display(transmission, "transmission");
                EmguCvWindowManager.Display(result, "result");
                EmguCvWindowManager.Display(resultImproved, "resultImproved");
            }

            return new BaseMethodResponse
            {
                EnhancementResult = resultImproved,
                DetectionResult = transmission,
                DetailedResults = new List<IInputArray> { image, transmission, result, resultImproved },
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
                    V[m, n] = new Gray(min);
                }
            }

            // apply an edge-preserving smoothing approach based on the local extrema for refinement
            V = V.SmoothGaussian(5);

            transmission = V.Convert<Gray, Byte>();

            // remove fog
            double q = 0.95; // q id used for regulating the degree of defogging
            Image<Bgr, Byte> R_image = new Image<Bgr, Byte>(I.Size);
            for (int m = 0; m < I.Rows; m++)
            {
                for (int n = 0; n < I.Cols; n++)
                {
                    // 1 approach - original (bad results)
                    //double v = V[m, n].Intensity / 255.0;

                    //double B_ = I[m, n].Blue / 255.0;
                    //double G_ = I[m, n].Green / 255.0;
                    //double R_ = I[m, n].Red / 255.0;

                    //double B = ((B_ - q * v) / (1 - v)) * A;
                    //double G = ((G_ - q * v) / (1 - v)) * A;
                    //double R = ((R_ - q * v) / (1 - v)) * A;

                    //B = (B * 255);
                    //G = (G * 255);
                    //R = (R * 255);

                    //B = (B % 255);
                    //G = (G % 255);
                    //R = (R % 255);

                    // 2 approach - modificated (too dark)
                    double v = V[m, n].Intensity / 255.0;

                    double B_ = I_corrected[m, n].Blue / 255.0;
                    double G_ = I_corrected[m, n].Green / 255.0;
                    double R_ = I_corrected[m, n].Red / 255.0;

                    double B = ((B_ - q * v) / (1 - v)) * A;
                    double G = ((G_ - q * v) / (1 - v)) * A;
                    double R = ((R_ - q * v) / (1 - v)) * A;

                    B = (B * 255);
                    G = (G * 255);
                    R = (R * 255);

                    if (B > 255) B = 255;
                    if (G > 255) G = 255;
                    if (R > 255) R = 255;

                    if (B < 0) B = 0;
                    if (G < 0) G = 0;
                    if (R < 0) R = 0;

                    R_image[m, n] = new Bgr(B, G, R);
                }
            }
            result = R_image;

            stopwatch.Stop();

            if (_params.ShowWindows)
            {
                EmguCvWindowManager.Display(image, "1 image");
                EmguCvWindowManager.Display(DC, "2 DC");
                EmguCvWindowManager.Display(I_corrected.Convert<Bgr, Byte>(), "3 I_corrected");
                EmguCvWindowManager.Display(V.Convert<Gray, Byte>(), "4 V");
                EmguCvWindowManager.Display(result, "5 result");
            }

            return new BaseMethodResponse
            {
                EnhancementResult = result,
                DetectionResult = transmission,
                DetailedResults = new List<IInputArray> { image, DC, I_corrected.Convert<Bgr, Byte>(), V.Convert<Gray, Byte>(), result },
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
                EmguCvWindowManager.Display(I_corrected.Convert<Bgr, Byte>(), "3 I_corrected");
                EmguCvWindowManager.Display(V.Convert<Gray, Byte>(), "4 V");
                EmguCvWindowManager.Display(result, "5 result");
            }

            return new BaseMethodResponse
            {
                EnhancementResult = result,
                DetectionResult = transmission,
                DetailedResults = new List<IInputArray> { image, DC, I_corrected.Convert<Bgr, Byte>(), V.Convert<Gray, Byte>(), result },
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }

        #endregion

        #region Real Time Image Haze Removal on Multi-core DSP
        // Source: http://www.sciencedirect.com/science/article/pii/S1877705814036467

        public BaseMethodResponse RemoveFogUsingMultiCoreDSPMethod(Image<Bgr, Byte> image, FogRemovalParams _params)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // down sample input image
            // more about interpolation types - https://stackoverflow.com/questions/3112364/how-do-i-choose-an-image-interpolation-method-emgu-opencv
            double scale = 0.75;
            if (image.Width * image.Height < 600 * 600) scale = 0.5;
            else scale = 0.33;
            Inter interpolationType = Inter.Linear;
            var downsampled = image.Resize(scale, interpolationType);

            // compute dark channel
            // TODO - there is faster way to compute DC using max-min filter
            //int patchSize = 15; // used in article
            int patchSize = 7;
            var DC = GetDarkChannel(downsampled, patchSize: patchSize);

            // estimate airlight
            int A = EstimateAirlight(DC, downsampled);

            // compute transmission
            var T = EstimateTransmission(DC, A);

            // improve T with guided filter
            Image<Gray, byte> improvedT;
            //int guidedRadius = 30; // used in article
            int guidedRadius = 7;
            try
            {
                improvedT = ImageHelper.GuidedFilterBy_clarkzjw(guideImage: downsampled, inputImage: T, radius: guidedRadius, eps: 0.02);
            }
            catch(Exception ex)
            {
                // use default EmguCv implementation
                improvedT = ImageHelper.GuidedFilterEmguCv(guideImage: downsampled, inputImage: T, radius: guidedRadius, eps: 0.02);
            }
            var transmission = improvedT;

            // get high resolution T (resize to origin size)
            var upsampledT = improvedT.Resize(image.Width, image.Height, interpolationType);

            // apply formula for fog removal
            var hazeFree = new Image<Bgr, byte>(image.Size);
            double t;
            double t0 = 0.1;
            Bgr I; double b, g, r;
            for (int m = 0; m < image.Rows; m++)
            {
                for (int n = 0; n < image.Cols; n++)
                {
                    t = upsampledT[m, n].Intensity / 255;
                    t = Math.Max(t, t0);
                    I = image[m, n];

                    b = (I.Blue - A) / t + A;
                    g = (I.Green - A) / t + A;
                    r = (I.Red - A) / t + A;

                    b = b > 255 ? 255 : Math.Abs(b);
                    g = g > 255 ? 255 : Math.Abs(g);
                    r = r > 255 ? 255 : Math.Abs(r);

                    hazeFree[m, n] = new Bgr(b, g, r);
                }
            }

            // apply color calibration to enhace global intensity, contrast, color (use histogram scratching)
            var colorCalibrated = ImageHelper.CalibrateColorsWithHistogramScratching(hazeFree);

            // obtain result
            var result = colorCalibrated;

            stopwatch.Stop();

            if (_params.ShowWindows)
            {
                EmguCvWindowManager.Display(image, "image");
                EmguCvWindowManager.Display(downsampled, "downsampled");
                EmguCvWindowManager.Display(T, "T");
                EmguCvWindowManager.Display(improvedT, "improvedT");
                EmguCvWindowManager.Display(upsampledT, "upsampledT");
                EmguCvWindowManager.Display(hazeFree, "hazeFree");
                EmguCvWindowManager.Display(colorCalibrated, "colorCalibrated");
            }

            return new BaseMethodResponse
            {
                EnhancementResult = result,
                DetectionResult = transmission,
                DetailedResults = new List<IInputArray> { image, downsampled, T, improvedT, upsampledT, hazeFree, colorCalibrated },
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }

        #endregion

        #region My Fog removal method

        public BaseMethodResponse RemoveFogUsingCustomMethod(Image<Bgr, Byte> image, FogRemovalParams _params)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // 1. Compute transmission using high/low pass filters
            var gray = ImageHelper.ToGray(image);
            var hpF = ImageHelper.ButterworthHightPassFilter(gray);
            var lpF = ImageHelper.ButterworthLowPassFilter(gray);
            var T_float = hpF + lpF;
            var T = T_float.Convert<Gray, byte>();
            T = ImageHelper.Inverse(T);
            T = T.SmoothMedian(5);

            // stretch to [0;1]
            double[] minValues, maxValues; Point[] minLocations, maxLocations;
            T.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);
            double Tmin = minValues[0];
            double Tmax = maxValues[0];
            for (int i = 0; i < T.Rows; i++)
            {
                for (int j = 0; j < T.Cols; j++)
                {

                    var newT = ((T[i, j].Intensity - Tmin) / (Tmax - Tmin)) * 255;
                    T[i, j] = new Gray(newT);
                }
            }

            // etimate airlight using T
            int A = EstimateAirlightByTransmission(T, image);

            // remove fog
            var processed = CustomRemoveFog(image, T, A);

            stopwatch.Stop();

            if (_params.ShowWindows)
            {
                EmguCvWindowManager.Display(image, "image");
                EmguCvWindowManager.Display(T, "T");
                EmguCvWindowManager.Display(processed, "processed");
            }

            return new BaseMethodResponse
            {
                EnhancementResult = processed,
                DetectionResult = T,
                DetailedResults = new List<IInputArray> { image, T, processed },
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }

        private Image<Bgr, byte> CustomRemoveFog(Image<Bgr, byte> image, Image<Gray, byte> T, int Airlight)
        {
            var hsv = ImageHelper.ToHsv(image);
            double H_, S_, V_;
            for (int i = 0; i < image.Rows; i++)
            {
                for (int j = 0; j < image.Cols; j++)
                {
                    var t = T[i, j].Intensity / 255.0;
                    t = Math.Max(t, 0.1);

                    H_ = hsv[i, j].Hue;
                    S_ = hsv[i, j].Satuation;
                    V_ = hsv[i, j].Value;

                    V_ = CustomSmartAdd(V_, (V_ - Airlight) * (1 - t));

                    if (V_ < 0 || V_ > 255)
                    {
                        throw new OverflowException();
                    }

                    hsv.Data[i, j, 2] = (byte)V_;
                }
            }
            var processed = ImageHelper.ToBgr(hsv);
            var postProcessed = ImageHelper.AdjustContrast(processed);

            KalikoImage imageK = new KalikoImage(postProcessed.Bitmap);
            imageK.ApplyFilter(new UnsharpMaskFilter(radius: 2.5f, amount: 0.7f, threshold: 0));
            var postProcessed2 = new Image<Bgr, byte>(imageK.GetAsBitmap());
            return postProcessed2;
        }

        private double CustomSmartAdd(double intensity, double addVal, int callCount = 0)
        {
            if(callCount > 25)
            {
                throw new OverflowException("Max call stack overflow");
            }
            byte intesityB = (byte)intensity;
            if (intesityB == 255 || intesityB == 0)
            {
                return intensity;
            }
            callCount += 1;
            double sum = intensity + addVal;
            if (sum < 0 || sum > 255)
            {
                double reducePercent = 0.25;
                double r2 = addVal * (1 - reducePercent);
                double res = CustomSmartAdd(intensity, r2, callCount);
                return res;
            }
            return sum;
        }

        public BaseMethodResponse RemoveFogUsingCustomMethodWithDepthEstimation(Image<Bgr, Byte> image, FogRemovalParams _params)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Try python depth estimation
            // Load depth map image
            var imagePath = _params.InputImageFileName;
            var imageFileName = Path.GetFileName(_params.InputImageFileName);
            var imageFileNameWithoutExtension = Path.GetFileNameWithoutExtension(_params.InputImageFileName);
            var imageFileExtension = Path.GetExtension(_params.InputImageFileName);
            var imageFolder = Path.GetDirectoryName(_params.InputImageFileName);
            string modelName = @"model_cityscapes";
            var cmd = @"""D:\Google Drive\Diploma5\pythonDepthEstimation\monodepth-master\monodepth_simple.py"" --image_path ""{0}"" --checkpoint_path ""D:\Google Drive\Diploma5\pythonDepthEstimation\monodepth-master\models\{1}""";
            cmd = String.Format(cmd, _params.InputImageFileName, modelName);
            var args = "";
            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = @"D:\ProgramFiles\Anaconda3\python.exe";
            start.Arguments = string.Format("{0} {1}", cmd, args);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result_ = reader.ReadToEnd();
                    Console.Write(result_);
                }
            }
            // when finished read file with depth map
            var depthResultPath = Path.Combine(imageFolder, $"{imageFileNameWithoutExtension}_disp{imageFileExtension}");
            var depthNpyResultPath = Path.Combine(imageFolder, $"{imageFileNameWithoutExtension}_disp.npy");
            var depthMapColor = new Image<Bgr, Byte>(depthResultPath);
            var depthMapGray = ImageHelper.ToGray(depthMapColor);

            // delete generated files
            if (File.Exists(depthResultPath)) File.Delete(depthResultPath);
            if (File.Exists(depthNpyResultPath)) File.Delete(depthNpyResultPath);

            int patchSize = 7;
            var imgDarkChannel = GetDarkChannel(image, patchSize);
            var Airlight = EstimateAirlight(imgDarkChannel, image);
            var T = depthMapGray;

            // reduce T
            for (int m = 0; m < T.Rows; m++)
            {
                for (int n = 0; n < T.Cols; n++)
                {
                    var pixel = T[m, n];
                    T[m, n] = new Gray(1.25 * pixel.Intensity);
                }
            }

            var improvedT = ImageHelper.GuidedFilterBy_clarkzjw(guideImage: image, inputImage: T, radius: 7, eps: 0.02);

            // Use standart model
            var fogModelResult = RemoveFog(image, improvedT, Airlight);

            // use custom approach
            var result = CustomRemoveFog(image, improvedT, Airlight);

            // apply correction only to Lightnes
            var lab = ImageHelper.ToLab(image);
            var channels = lab.Split();
            var l = channels[0]; var lRes = new Image<Gray, Byte>(lab.Size);
            var a = channels[1];
            var b = channels[2];
            for (int i = 0; i < image.Rows; i++)
            {
                for (int j = 0; j < image.Cols; j++)
                {
                    var t = improvedT[i, j].Intensity / 255.0;
                    t = Math.Max(t, 0.1);

                    double L = (l[i, j].Intensity - Airlight) / t + Airlight;
                    L = L > 255 ? 255 : Math.Abs(L);
                    lRes[i, j] = new Gray(L);
                }
            }
            var labRes = new Image<Lab, Byte>(new Image<Gray, Byte>[] { lRes, a, b });
            var alternativeResult = ImageHelper.ToBgr(labRes);

            var transmission = improvedT;

            stopwatch.Stop();

            if (_params.ShowWindows)
            {
                EmguCvWindowManager.Display(image, "image");
                EmguCvWindowManager.Display(depthMapColor, "depthMapColor");
                EmguCvWindowManager.Display(T, "T");
                EmguCvWindowManager.Display(improvedT, "improvedT");
                EmguCvWindowManager.Display(result, "result");
                EmguCvWindowManager.Display(fogModelResult, "fogModelResult");
                EmguCvWindowManager.Display(alternativeResult, "alternativeResult");
            }

            return new BaseMethodResponse
            {
                EnhancementResult = result,
                DetectionResult = transmission,
                DetailedResults = new List<IInputArray> { image, depthMapColor, T, improvedT, fogModelResult, alternativeResult, result },
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