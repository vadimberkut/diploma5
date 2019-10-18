using System;
using diploma5_csharp.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Diagnostics;

namespace diploma5_csharp.Helpers
{
    public static class ImageHelper
    {
        public static double Max(Image<Gray, Byte> image)
        {
            double[] minValues, maxValues;
            Point[] minLocations, maxLocations;
            image.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);
            return maxValues[0];
        }

        public static double Min(Image<Gray, Byte> image)
        {
            double[] minValues, maxValues;
            Point[] minLocations, maxLocations;
            image.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);
            return minValues[0];
        }

        public static double Max(Image<Gray, double> image)
        {
            double[] minValues, maxValues;
            Point[] minLocations, maxLocations;
            image.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);
            return maxValues[0];
        }

        public static double Min(Image<Gray, double> image)
        {
            double[] minValues, maxValues;
            Point[] minLocations, maxLocations;
            image.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);
            return minValues[0];
        }

        public static double Sum(Image<Gray, Byte> image)
        {
            double sum = 0;
            for (int m = 0; m < image.Rows; m++)
            {
                for (int n = 0; n < image.Cols; n++)
                {
                    sum += image[m, n].Intensity;
                }
            }
            return sum;
        }

        public static double Sum(Image<Gray, double> image)
        {
            double sum = 0;
            for (int m = 0; m < image.Rows; m++)
            {
                for (int n = 0; n < image.Cols; n++)
                {
                    sum += image[m, n].Intensity;
                }
            }
            return sum;
        }

        public static Image<Lab, Byte> ToLab(Image<Bgr, Byte> image)
        {
            Image<Lab, Byte> result = new Image<Lab, byte>(image.Size);
            CvInvoke.CvtColor(image, result, ColorConversion.Bgr2Lab);
            return result;
        }

        public static Image<Bgr, Byte> ToBgr(Image<Lab, Byte> image)
        {
            Image<Bgr, Byte> result = new Image<Bgr, byte>(image.Size);
            CvInvoke.CvtColor(image, result, ColorConversion.Lab2Bgr);
            return result;
        }

        public static Image<Bgr, Byte> ToBgr(Image<Gray, Byte> image)
        {
            Image<Bgr, Byte> result = new Image<Bgr, byte>(image.Size);
            CvInvoke.CvtColor(image, result, ColorConversion.Gray2Bgr);
            return result;
        }


        public static Image<Hsv, Byte> ToHsv(Image<Bgr, Byte> image)
        {
            Image<Hsv, Byte> result = new Image<Hsv, byte>(image.Size);
            CvInvoke.CvtColor(image, result, ColorConversion.Bgr2Hsv);
            return result;
        }

        public static Image<Bgr, Byte> ToBgr(Image<Hsv, Byte> image)
        {
            Image<Bgr, Byte> result = new Image<Bgr, byte>(image.Size);
            CvInvoke.CvtColor(image, result, ColorConversion.Hsv2Bgr);
            return result;
        }


        public static Image<Gray, Byte> ToGray(Image<Bgr, Byte> image)
        {
            Image<Gray, Byte> result = new Image<Gray, byte>(image.Size);
            CvInvoke.CvtColor(image, result, ColorConversion.Bgr2Gray);
            return result;
        }


        public static Image<Ycc, Byte> ToYCrCb(Image<Bgr, Byte> image)
        {
            Image<Ycc, Byte> result = new Image<Ycc, byte>(image.Size);
            CvInvoke.CvtColor(image, result, ColorConversion.Bgr2YCrCb);
            return result;
        }

        public static Image<Bgr, Byte> ToBgr(Image<Ycc, Byte> image)
        {
            Image<Bgr, Byte> result = new Image<Bgr, byte>(image.Size);
            CvInvoke.CvtColor(image, result, ColorConversion.YCrCb2Bgr);
            return result;
        }

        // To YCbCr using formula
        public static Image<Bgr, Byte> ToYCrCbUsingFormula(Image<Bgr, Byte> image)
        {
            Image<Bgr, Byte> result = new Image<Bgr, byte>(image.Size);
            double R, G, B;
            double Y, Cb, Cr;
            for (int m = 0; m < image.Rows; m++)
            {
                for (int n = 0; n < image.Cols; n++)
                {
                    Bgr pixel = image[m, n];

                    B = pixel.Blue;
                    G = pixel.Green;
                    R = pixel.Red;

                    Y = 0.299 * R + 0.587 * G + 0.114 * B;
                    Cb = -0.299 * R - 0.587 * G + 0.886 * B; // = B - Y
                    Cr = 0.701 * R - 0.587 * G - 0.114 * B; // = R - Y

                    result[m, n] = new Bgr(Y, Cb, Cr);
                }
            }
            return result;
        }

        public static Image<Bgr, Byte> ToBgrFromYCrCbUsingFormula(Image<Bgr, Byte> image)
        {
            Image<Bgr, Byte> result = new Image<Bgr, byte>(image.Size);
            double R, G, B;
            double Y, Cb, Cr;
            for (int m = 0; m < image.Rows; m++)
            {
                for (int n = 0; n < image.Cols; n++)
                {
                    Bgr pixel = image[m, n];

                    Y = pixel.Blue;
                    Cb = pixel.Green;
                    Cr = pixel.Red;

                    R = Y + Cr;
                    G = Y - 0.194 * Cb - 0.509 * Cr;
                    B = Y + Cb;

                    result[m, n] = new Bgr(B, G, R);
                }
            }
            return result;
        }


        public static Image<Hls, Byte> ToHLS(Image<Bgr, Byte> image)
        {
            Image<Hls, Byte> result = new Image<Hls, byte>(image.Size);
            CvInvoke.CvtColor(image, result, ColorConversion.Bgr2Hls);
            return result;
        }

        public static Image<Bgr, Byte> ToHLS(Image<Hls, Byte> image)
        {
            Image<Bgr, Byte> result = new Image<Bgr, byte>(image.Size);
            CvInvoke.CvtColor(image, result, ColorConversion.Hls2Bgr);
            return result;
        }


        //converts to Hsi but saves in Bgr (no Hsi in EmguCv)
        public static Image<Bgr, double> ToHSI(Image<Bgr, Byte> image)
        {
            Image<Bgr, Byte> input = image;
            Image<Bgr, double> result = new Image<Bgr, double>(image.Size);

            //Algorithm from http://angeljohnsy.blogspot.com/2013/05/converting-rgb-image-to-hsi.html

            //Normalize RGB to [0,1]
            //for (int m = 0; m < input.Rows; m++)
            //{
            //    for (int n = 0; n < input.Cols; n++)
            //    {
            //        Bgr pixel = input[m, n];

            //        double B = pixel.Blue / 255;
            //        double G = pixel.Green / 255;
            //        double R = pixel.Red / 255;

            //        input[m, n] = new Bgr(B, G, R);
            //    }
            //}

            //Find HSI components
            for (int m = 0; m < input.Rows; m++)
            {
                for (int n = 0; n < input.Cols; n++)
                {
                    Bgr pixel = input[m, n];

                    ////Normalize RGB to [0,1]
                    double B = pixel.Blue / 255;
                    double G = pixel.Green / 255;
                    double R = pixel.Red / 255;

                    //Hue
                    double nominator = (1.0 / 2.0) * ((R - G) + (R - B));
                    double denominator = Math.Pow((Math.Pow((R - G), 2) + (R - B) * (G - B)) , 1.0 / 2.0); //!!

                    //To avoid divide by zero exception add a small number in the denominator
                    double theta = Math.Pow(Math.Cos(nominator / (denominator + 0.000001)), -1); //!!

                    //If B>G then H= 360-Theta
                    double H = 0;
                    if (B <= G)
                        H = theta;
                    if (B > G)
                        H = 360 - theta;

                    //Normalize to the range [0 1]
                    H = H / 360.0;

                    //Saturation
                    double min = StatisticsHelper.Min(R, G, B);
                    double S = 1 - (3.0 / (R + G + B)) * min;

                    //Intensity
                    double I = (1.0 / 3.0) * (R + G + B);

                    H = H * 255;
                    S = S * 255;
                    I = I * 255;

                    //HSI
                    result[m, n] = new Bgr(H, S, I);
                }
            }

            return result;
        }

        //2 variant from github 
        //https://gist.github.com/rzhukov/9129585
        public static Image<Bgr, double> ToHSIGitHub(Image<Bgr, Byte> image)
        {
            Image<Bgr, Byte> input = image.Clone();
            Image<Bgr, double> result = new Image<Bgr, double>(image.Size);

            //Find HSI components
            for (int m = 0; m < input.Rows; m++)
            {
                for (int n = 0; n < input.Cols; n++)
                {
                    Bgr pixel = input[m, n];

                    ////Normalize RGB to [0,1]
                    double B = pixel.Blue;
                    double G = pixel.Green;
                    double R = pixel.Red;

                    double I = (R + G + B) / 3.0;

                    double rn = R / (R + G + B);
                    double gn = G / (R + G + B);
                    double bn = B / (R + G + B);

                    double H = Math.Acos((0.5 * ((rn - gn) + (rn - bn))) / (Math.Sqrt((rn - gn) * (rn - gn) + (rn - bn) * (gn - bn))));
                    if (B > G)
                    {
                        H = 2 * Math.PI - H;
                    }

                    double S = 1 - 3 * Math.Min(rn, Math.Min(gn, bn));

                    //HSI
                    result[m, n] = new Bgr(H, S, I);
                }
            }

            return result;
        }

        public static Image<Hls, Byte> ToHSICustom(Image<Bgr, Byte> image)
        {
            Image<Hls, Byte> result = new Image<Hls, byte>(image.Size);
            CvInvoke.CvtColor(image, result, ColorConversion.Bgr2Hls);
            return result;
        }

        public static double CalcGrayChannelAverage(Image<Gray, Byte> image)
        {
            double result = 0;
            for (int i = 0; i < image.Rows; i += 1)
            {
                for (int j = 0; j < image.Cols; j += 1)
                {
                    result += image[i, j].Intensity;

                }
            }
            result = result / (image.Rows * image.Cols);
            return result;
        }


        public static BgrChannelAverageResult CalcBgrChannelAverage(Image<Bgr, Byte> image)
        {
            BgrChannels channels = GetBgrChannels(image);

            BgrChannelAverageResult result = new BgrChannelAverageResult()
            {
                R = StatisticsHelper.Average(channels.B),
                G = StatisticsHelper.Average(channels.G),
                B = StatisticsHelper.Average(channels.R)
            };
            return result;
        }

        public static LabChannelAverageResult CalcLabChannelAverage(Image<Lab, Byte> image)
        {
            double L_avg = 0;
            double A_avg = 0;
            double B_avg = 0;

            for (int i = 0; i < image.Rows; i += 1)
            {
                for (int j = 0; j < image.Cols; j += 1)
                {
                    Lab color = image[i,j];
                    L_avg += color.X;
                    A_avg += color.Y;
                    B_avg += color.Z;

                }
            }
            int count = image.Rows*image.Cols;

            L_avg /= count;
            A_avg /= count;
            B_avg /= count;
            LabChannelAverageResult result = new LabChannelAverageResult()
            {
                L = L_avg,
                A = A_avg,
                B = B_avg
            };
            return result;
        }


        public static BgrChannels GetBgrChannels(Image<Bgr, Byte> image)
        {
            BgrChannels result = new BgrChannels(image.Rows * image.Cols);

            for (int i = 0; i < image.Rows; i += 1)
            {
                for (int j = 0; j < image.Cols; j += 1)
                {
                    Bgr color = image[i, j];

                    result.B[i * image.Cols + j] = color.Blue;
                    result.G[i * image.Cols + j] = color.Green;
                    result.R[i * image.Cols + j] = color.Red;
                }
            }

            return result;
        }

        public static BgrChannelsByte GetBgrChannelsAsByte(Image<Bgr, Byte> image)
        {
            BgrChannelsByte result = new BgrChannelsByte(image.Rows * image.Cols);

            for (int i = 0; i < image.Rows; i += 1)
            {
                for (int j = 0; j < image.Cols; j += 1)
                {
                    Bgr color = image[i, j];

                    result.B[i * image.Cols + j] = (byte)color.Blue;
                    result.G[i * image.Cols + j] = (byte)color.Green;
                    result.R[i * image.Cols + j] = (byte)color.Red;
                }
            }

            return result;
        }

        public static LabChannels GetLabChannels(Image<Lab, Byte> image)
        {
            LabChannels result = new LabChannels(image.Rows * image.Cols);

            for (int i = 0; i < image.Rows; i += 1)
            {
                for (int j = 0; j < image.Cols; j += 1)
                {
                    Lab color = image[i, j];

                    result.L[i*image.Cols + j] = color.X;
                    result.A[i*image.Cols + j] = color.Y;
                    result.B[i*image.Cols + j] = color.Z;
                }
            }

            return result;
        }

        public static GrayChannel GetGrayChannel(Image<Gray, Byte> image)
        {
            GrayChannel result = new GrayChannel(image.Rows * image.Cols);

            for (int i = 0; i < image.Rows; i += 1)
            {
                for (int j = 0; j < image.Cols; j += 1)
                {
                    Gray color = image[i, j];

                    result.Intensity[i * image.Cols + j] = color.Intensity;
                }
            }

            return result;
        }


        public static void SetPixel(ref Image<Gray, Byte> image, int i, int j, byte value)
        {
            image.Data[i, j, 0] = value;
        }


        public static SplittedByMask<BgrChannels> SplitImageByMask(IInputArray inputImage, Image<Gray, Byte> mask)
        {
            return new SplittedByMask<BgrChannels>();
        }

        public static SplittedByMask<BgrChannels> SplitImageBgrByMask(IInputArray inputImage, Image<Gray, Byte> mask)
        {
            var inputArray = inputImage.GetInputArray();
            var mat = inputArray.GetMat();
            var image = mat.ToImage<Bgr, Byte>();
            var size = image.Size.Width * image.Size.Height;

            List<int[]> InIndexes = new List<int[]>();
            List<int[]> OutIndexes = new List<int[]>();

            for (int i = 0; i < image.Rows; i += 1)
            {
                for (int j = 0; j < image.Cols; j += 1)
                {
                    Bgr color = image[i, j];
                    Gray maskColor = mask[i, j];

                    if (maskColor.Intensity == 255)
                        InIndexes.Add(new []{ i, j });
                    else
                        OutIndexes.Add(new[] { i, j });
                }
            }

            int size2 = InIndexes.Count + OutIndexes.Count;
            SplittedByMask<BgrChannels> result = new SplittedByMask<BgrChannels>()
            {
                In = new BgrChannels(InIndexes.Count),
                Out = new BgrChannels(OutIndexes.Count)
            };

            int ii, jj;
            for (int k = 0; k < InIndexes.Count; k += 1)
            {
                ii = InIndexes[k][0];
                jj = InIndexes[k][1];

                result.In.B[k] = image[ii, jj].Blue;
                result.In.G[k] = image[ii, jj].Green;
                result.In.R[k] = image[ii, jj].Red;
            }
            for (int k = 0; k < OutIndexes.Count; k += 1)
            {
                ii = OutIndexes[k][0];
                jj = OutIndexes[k][1];

                result.Out.B[k] = image[ii, jj].Blue;
                result.Out.G[k] = image[ii, jj].Green;
                result.Out.R[k] = image[ii, jj].Red;
            }

            return result;
        }


        public static MCvScalar GenerateRandomColor()
        {
            Random rnd = new Random();
            int min = 0;
            int max = 255;
            MCvScalar color = new MCvScalar(rnd.Next(min, max), rnd.Next(min, max), rnd.Next(min, max));
            return color;
        }

        #region IMAGE NORMALIZATION

        public static Image<Gray, double> NormalizeImage(Image<Gray, Byte> image)
        {
            Image<Gray, double> result = new Image<Gray, double>(image.Size);
            for (int m = 0; m < image.Rows; m++)
            {
                for (int n = 0; n < image.Cols; n++)
                {
                    Gray pixel = image[m, n];
                    result[m, n] = new Gray(pixel.Intensity / 255);
                }
            }
            return result;
        }

        public static Image<Gray, Byte> DeNormalizeImage(Image<Gray, double> image)
        {
            Image<Gray, Byte> result = new Image<Gray, Byte>(image.Size);
            for (int m = 0; m < image.Rows; m++)
            {
                for (int n = 0; n < image.Cols; n++)
                {
                    Gray pixel = image[m, n];
                    result[m, n] = new Gray(pixel.Intensity * 255);
                }
            }
            return result;
        }

        #endregion

        #region GET IMAGE PIXELS

        public static double[] GetImagePixels(Image<Gray, Byte> image)
        {
            double[] result = new double[image.Rows * image.Cols];
            for (int m = 0; m < image.Rows; m++)
            {
                for (int n = 0; n < image.Cols; n++)
                {
                    Gray pixel = image[m, n];
                    result[m * image.Cols + n] = pixel.Intensity;
                }
            }
            return result;
        }

        public static GrayPixelWithCoords[] GetImagePixelsWithCoords(Image<Gray, Byte> image)
        {
            GrayPixelWithCoords[] result = new GrayPixelWithCoords[image.Rows * image.Cols];
            for (int m = 0; m < image.Rows; m++)
            {
                for (int n = 0; n < image.Cols; n++)
                {
                    Gray pixel = image[m, n];
                    result[m * image.Cols + n] = new GrayPixelWithCoords { Intensity = pixel.Intensity, Coords = new PixelCoords(m, n) };
                }
            }
            return result;
        }

        public static double[] GetImagePixels(Image<Gray, double> image)
        {
            double[] result = new double[image.Rows * image.Cols];
            for (int m = 0; m < image.Rows; m++)
            {
                for (int n = 0; n < image.Cols; n++)
                {
                    Gray pixel = image[m, n];
                    result[m * image.Cols + n] = pixel.Intensity;
                }
            }
            return result;
        }

        public static GrayImagePixelValueWithPosition[] GetImagePixelsWithPositions(Image<Gray, Byte> image)// : T is new
        {
            GrayImagePixelValueWithPosition[] result = new GrayImagePixelValueWithPosition[image.Rows * image.Cols];
            for (int m = 0; m < image.Rows; m++)
            {
                for (int n = 0; n < image.Cols; n++)
                {
                    Gray pixel = image[m, n];
                    result[m * image.Cols + n] = new GrayImagePixelValueWithPosition
                    {
                        Intensity = pixel.Intensity,
                        Position = new Point(m, n)
                    };
                }
            }
            return result;
        }

        #endregion

        public static Image<Gray, float> MaskToImage(double[,] mask)
        {
            Image<Gray, float> image = new Image<Gray, float>(mask.GetLength(1), mask.GetLength(0));
            for (int m = 0; m < mask.GetLength(0); m++)
            {
                for (int n = 0; n < mask.GetLength(1); n++)
                {
                    image[m, n] = new Gray(mask[m,n] * 255);
                }
            }
            return image;
        }

        public static Image<Gray, byte> Inverse(Image<Gray, byte> image)
        {
            Image<Gray, byte> result = new Image<Gray, byte>(image.Size);
            for (int m = 0; m < image.Rows; m++)
            {
                for (int n = 0; n < image.Cols; n++)
                {
                    result[m, n] = new Gray(255 - image[m, n].Intensity);
                }
            }
            return result;
        }



        #region Fast Fourier Transform
        // TAKEN FROM https://github.com/rajatvikramsingh/RajatView

        // WITH ERROR
        public static Image<Gray, Byte> FFT(Image<Gray, Byte> img)
        {
            //Image<Gray, byte> img = new Image<Gray, byte>(openFileDialog1.FileName);
            double[,] arr = new double[img.Height, img.Width];
            Image<Gray, byte> fft_img = new Image<Gray, byte>(img.Height, img.Width);
            double p = 0;
            for (int k = 0; k < img.Height; k++)
            {
                for (int l = 0; l < img.Width; l++)
                {
                    /*p = calculate_fft(img[k, l].Intensity, k, l, img.Height, img.Width);
                    
                    arr[k, l] = p;*/
                    Complex total = Complex.Zero;
                    for (int i = 0; i < img.Width; i++)
                    {
                        for (int j = 0; j < img.Height; j++)
                        {
                            Complex c1 = Complex.Multiply(Complex.ImaginaryOne, (2 * Math.PI));
                            double c2 = (((double)(k * i)) / ((double)img.Width)) + (((double)(l * j)) / ((double)img.Height));
                            Complex power = Complex.Multiply(c1, c2);
                            Complex exp = Complex.Exp(power);
                            Complex answer;
                            if ((i + j) % 2 != 0)
                                answer = Complex.Divide((-img[i, j].Intensity), exp);
                            else
                                answer = Complex.Divide(img[i, j].Intensity, exp);
                            total = Complex.Add(total, answer);
                        }
                    }
                    total = Complex.Divide(total, (img.Width * img.Height));
                    double value = 1 + Math.Log10(total.Magnitude);
                    arr[k, l] = value;
                }
            }

            fft_img = scaleImage(arr, fft_img.Height, fft_img.Width);
            return fft_img;

        }

        public static Image<Gray, byte> scaleImage(double[,] arr, int height, int width)
        {
            double min = arr[0, 0];

            double[,] imgnew = new double[height, width];
            Image<Gray, byte> image = new Image<Gray, byte>(height, width);
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    if (arr[i, j] < min)
                        min = arr[i, j];
                }

            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    imgnew[i, j] = arr[i, j] - min;
            double max = imgnew[0, 0];
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                {
                    if (imgnew[i, j] > max)
                        max = imgnew[i, j];
                }
            for (int i = 0; i < height; i++)
                for (int j = 0; j < width; j++)
                    image[i, j] = new Gray((imgnew[i, j] / max) * 255);
            return image;
        }

        public static Image<Gray, float> IdealLowPassFilter(Image<Gray, byte> img)
        {
            Image<Gray, float> im_pad = PerformForwardDiscreteFourierTransform(img.Convert<Gray, float>());
            Image<Gray, float> retimage = new Image<Gray, float>(2 * img.Width, 2 * img.Height);
            var h = retidealmask(img, 150, 0);
            retimage = Convolve(im_pad, h);

            //var maskImg = MaskToImage(h);
            //EmguCvWindowManager.Display(pad(img.Convert<Gray, float>()), "IdealLowPassFilter pad1");
            //EmguCvWindowManager.Display(pad(img.Convert<Gray, float>()).Convert<Gray, Byte>(), "IdealLowPassFilter pad2");
            //EmguCvWindowManager.Display(im_pad, "IdealLowPassFilter im_pad1");
            //EmguCvWindowManager.Display(im_pad.Convert<Gray, Byte>(), "IdealLowPassFilter im_pad2");
            //EmguCvWindowManager.Display(maskImg, "IdealLowPassFilter maskImg1");
            //EmguCvWindowManager.Display(maskImg.Convert<Gray, Byte>(), "IdealLowPassFilter maskImg2");

            return retimage;
        }

        public static Image<Gray, float> IdealHightPassFilter(Image<Gray, byte> img)
        {
            Image<Gray, float> im_pad = PerformForwardDiscreteFourierTransform(img.Convert<Gray, float>());
            Image<Gray, float> retimage = new Image<Gray, float>(2 * img.Width, 2 * img.Height);
            var h = retidealmask(img, 200, 1);
            retimage = Convolve(im_pad, h);
            return retimage;
        }

        public static Image<Gray, float> ButterworthLowPassFilter(Image<Gray, byte> img)
        {
            Image<Gray, float> paddedImage = PerformForwardDiscreteFourierTransform(img.Convert<Gray, float>());
            Image<Gray, float> resultImage = null;
            var h = GetButterworthMask(img, 1, 0);
            resultImage = Convolve(paddedImage, h);
            return resultImage;
        }

        public static Image<Gray, float> ButterworthHightPassFilter(Image<Gray, byte> img)
        {
            Image<Gray, float> paddedImage = PerformForwardDiscreteFourierTransform(img.Convert<Gray, float>());
            Image<Gray, float> resultImage = null;
            var mask = GetButterworthMask(img, 1, 1);
            resultImage = Convolve(paddedImage, mask);
            return resultImage;
        }

        public static Image<Gray, float> GaussianLowPassFilter(Image<Gray, byte> img)
        {
            Image<Gray, float> im_pad = PerformForwardDiscreteFourierTransform(img.Convert<Gray, float>());
            Image<Gray, float> retimage = new Image<Gray, float>(2 * img.Width, 2 * img.Height);
            var h = retgaussianmask(img, 0);
            retimage = Convolve(im_pad, h);
            return retimage;
        }

        public static Image<Gray, float> GaussianHightPassFilter(Image<Gray, byte> img)
        {
            Image<Gray, float> im_pad = PerformForwardDiscreteFourierTransform(img.Convert<Gray, float>());
            Image<Gray, float> retimage = new Image<Gray, float>(2 * img.Width, 2 * img.Height);
            var h = retgaussianmask(img, 1);
            retimage = Convolve(im_pad, h);
            return retimage;
        }

        /// <summary>
        /// Performs a forward Discrete Fourier transform 
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private static Image<Gray, float> PerformForwardDiscreteFourierTransform(Image<Gray, float> img)
        {
            Image<Gray, float> im_pad = new Image<Gray, float>(img.Width * 2, img.Height * 2);
            Image<Gray, float> dft = new Image<Gray, float>(img.Width * 2, img.Height * 2);
            im_pad = PadImage(img);

            // http://www.emgu.com/wiki/files/3.0.0/document/html/030dfa0f-c105-c661-92d2-c79ba367df3b.htm
            // https://docs.opencv.org/3.4/d2/de8/group__core__array.html#gadd6cf9baf2b8b704a11b5f04aaf4f39d
            CvInvoke.Dft(
                src: im_pad, // Source array, real or complex
                dst: dft, // Destination array of the same size and same type as the source
                flags: DxtType.Forward, // Transformation flags
                nonzeroRows: 0 // Number of nonzero rows to in the source array (in case of forward 2d transform), or a number of rows of interest in the destination array (in case of inverse 2d transform). If the value is negative, zero, or greater than the total number of rows, it is ignored. The parameter can be used to speed up 2d convolution/correlation when computing them via DFT
            );
            return dft;
        }

        /// <summary>
        /// Increases image in size 2x
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        private static Image<Gray, float> PadImage(Image<Gray, float> img)
        {
            // Origin (double size of image with white pixels)
            //Image<Gray, float> im_new = new Image<Gray, float>(ig.Width * 2, ig.Height * 2);
            //Image<Gray, float> img_return = new Image<Gray, float>(ig.Width * 2, ig.Height * 2);
            //double b = 0;

            //for (int i = 0; i < ig.Rows; i++)
            //{
            //    for (int j = 0; j < ig.Cols; j++)
            //    {
            //        b = ig[i, j].Intensity;
            //        b = b * ((-1) ^ (i + j));
            //        img_return[i, j] = new Gray(b);
            //    }

            //}
            // return img_return;

            // My (resize image in 2 times)
            // http://www.emgu.com/wiki/files/3.1.0/document/html/c9c13b1e-4f15-31ea-9d40-4a76d5bee079.htm
            // https://docs.opencv.org/trunk/da/d54/group__imgproc__transform.html#ga47a974309e9102f5f08231edc7e7529d
            return img.Resize(img.Width * 2, img.Height * 2, Inter.Linear);

            // My (resize optimal)
            //int m = CvInvoke.GetOptimalDFTSize(ig.Rows);
            //int n = CvInvoke.GetOptimalDFTSize(ig.Cols);
            //Image<Gray, float> padded = new Image<Gray, float>(ig.Size);
            //CvInvoke.CopyMakeBorder(ig, padded, 0, m - ig.Rows, 0, n - ig.Cols, BorderType.Constant);
            //return padded;
        }

        private static double[,] retidealmask(Image<Gray, byte> img, int radius, int mode)
        {
            var h = new double[img.Rows * 2, img.Cols * 2];
            for (int i = 0; i < 2 * img.Rows; i++)
                for (int j = 0; j < 2 * img.Cols; j++)
                {
                    if (Math.Sqrt(Math.Pow((i - (img.Width)), 2) + Math.Pow(j - (img.Height), 2)) <= radius)
                        h[i, j] = 1;
                    else
                        h[i, j] = 0;
                }
            if (mode == 0)
            {
                for (int i = 0; i < 2 * img.Rows; i++)
                    for (int j = 0; j < 2 * img.Cols; j++)
                    {
                        h[i, j] = 1 - h[i, j];
                    }
            }
            return h;
        }

        /// <summary>
        /// The Butterworth High Pass / Low Pass Filter
        /// About: https://www.sciencedirect.com/topics/engineering/butterworth
        /// </summary>
        /// <param name="img"></param>
        /// <param name="p">
        /// The parameter n is a user-defined positive integer called the order of the filter. As the value of n increases, the BHPF approaches the ideal filter.
        /// порядок фильтра (если верить литературе, то его оптимальное значение 2)
        /// </param>
        /// <param name="mode">
        /// 0 - Low Pass (inverse of High Pass)
        /// 1 - High Pass
        /// </param>
        /// <returns></returns>
        private static double[,] GetButterworthMask(Image<Gray, byte> img, int p, int mode)
        {
            // D0 - расстояние от начала координат в частотной области
            int D0 = 30;

            var h = new double[img.Rows * 2, img.Cols * 2];
            for (int i = 0; i < 2 * img.Rows; i++)
                for (int j = 0; j < 2 * img.Cols; j++)
                {
                    h[i, j] = 1 / (Math.Pow((Math.Sqrt(Math.Pow((i - (img.Width)), 2) + Math.Pow(j - (img.Height), 2)) / D0), 2 * p) + 1);
                }
            if (mode == 0)
            {
                for (int i = 0; i < 2 * img.Rows; i++)
                    for (int j = 0; j < 2 * img.Cols; j++)
                    {
                        h[i, j] = 1 - h[i, j];
                    }
            }
            return h;
        }

        private static double[,] retgaussianmask(Image<Gray, byte> img, int mode)
        {
            var h = new double[img.Rows * 2, img.Cols * 2];
            for (int i = 0; i < 2 * img.Rows; i++)
                for (int j = 0; j < 2 * img.Cols; j++)
                {
                    h[i, j] = Math.Exp(((-1) * (Math.Pow((i - (img.Width)), 2) + Math.Pow(j - (img.Height), 2))) / (2 * Math.Pow(15, 2)));
                }
            if (mode == 0)
            {
                for (int i = 0; i < 2 * img.Rows; i++)
                    for (int j = 0; j < 2 * img.Cols; j++)
                    {
                        h[i, j] = 1 - h[i, j];
                    }
            }
            return h;
        }

        private static Image<Gray, float> Convolve(Image<Gray, float> img, double[,] mask)
        {
            Image<Gray, float> convultionResult = new Image<Gray, float>(img.Width, img.Height);
            Image<Gray, float> resultImage = new Image<Gray, float>(img.Width, img.Height);
            Image<Gray, float> resultResizedImage = new Image<Gray, float>(img.Width / 2, img.Height / 2);

            for (int k = 0; k < img.Rows; k++)
            {
                for (int l = 0; l < img.Cols; l++)
                {
                    convultionResult[k, l] = new Gray(img[k, l].Intensity * mask[k, l]);
                }
            }

            CvInvoke.Dft(
                src: convultionResult, 
                dst: resultImage, 
                flags: DxtType.InvScale, 
                nonzeroRows: 0
            );
            //CvInvoke.Dft(convultionResult, resultImage, DxtType.Inverse, 0);

            // Original (reduce image size back)
            //for (int k = 0; k < img.Rows / 2; k++)
            //    for (int l = 0; l < img.Cols / 2; l++)
            //    {
            //        resultResizedImage[k, l] = resultImage[k, l];
            //    }

            // My (resize image back)
            resultResizedImage = resultImage.Resize(resultImage.Width / 2, resultImage.Height / 2, Inter.Linear);

            return resultResizedImage;
        }

      #endregion





      // Source - http://efundies.com/adjust-the-contrast-of-an-image-in-c/
      // And Here - https://softwarebydefault.com/2013/04/20/image-contrast/
      public static Image<Bgr, byte> AdjustContrast(Image<Bgr, byte> image, double threshold = 10)
        {
            Image<Bgr, byte> result = new Image<Bgr, byte>(image.Size);
            var contrastLevel = Math.Pow((100.0 + threshold) / 100.0, 2);
            for (int m = 0; m < image.Rows; m++)
            {
                for (int n = 0; n < image.Cols; n++)
                {
                    var pixel = image[m, n];

                    double B = pixel.Blue;
                    double G = pixel.Green;
                    double R = pixel.Red;

                    B = ((((B / 255.0) - 0.5) * contrastLevel) + 0.5) * 255.0;
                    G = ((((G / 255.0) - 0.5) * contrastLevel) + 0.5) * 255.0;
                    R = ((((R / 255.0) - 0.5) * contrastLevel) + 0.5) * 255.0;

                  if (B > 255)
                  { B = 255; }
                  else if (B < 0)
                  { B = 0; }


                  if (G > 255)
                  { G = 255; }
                  else if (G < 0)
                  { G = 0; }


                  if (R > 255)
                  { R = 255; }
                  else if (R < 0)
                  { R = 0; }

               result[m, n] = new Bgr(B, G, R);
                }
            }
            return result;
        }

        #region Guided Filter
        // EmguCv Guided Filter - http://www.emgu.com/wiki/files/3.2.0/document/html/47f05712-19f6-f558-ff34-0e6f3b1445ef.htm

        public static Image<Gray, byte> GuidedFilterEmguCv(
                Image<Bgr, byte> guideImage, 
                Image<Gray, byte> inputImage,
                int radius = 3,
                double eps = 0.02,
                int dDepth = -1
            )
            //where TColor : struct, IColor
            //where TDepth : struct
        {
            Image<Gray, byte> result = new Image<Gray, byte>(inputImage.Size);

            //var fastGuidedFilter = new AForge.Imaging.Filters.FastGuidedFilter
            //{
            //    KernelSize = 8,
            //    Epsilon = 0.02f,
            //    SubSamplingRatio = 0.25f,
            //    OverlayImage = (Bitmap)bmp.Clone()
            //};

            IInputArray guide = guideImage; // guided image (or array of images) with up to 3 channels, if it have more then 3 channels then only first 3 channels will be used.
            IInputArray src = inputImage; // filtering image with any numbers of channels.
            IOutputArray dst = result; // output image.
            //int radius = 3; // radius of Guided Filter.
            //double eps = 0.02; // regularization term of Guided Filter. eps^2 is similar to the sigma in the color space into bilateralFilter.
            //int dDepth = -1; // optional depth of the output image.
            Emgu.CV.XImgproc.XImgprocInvoke.GuidedFilter(guide: guide, src: src, dst: dst, radius: radius, eps: eps, dDepth: dDepth);
            return result;
        }

        // Guided Filter By clarkzjw - https://github.com/clarkzjw/GuidedFilter
        // as author said his method is better than OpenCv implementation wich can give wrong results sometimes
        public static void GuidedFilterBy_clarkzjw(Mat guide, Mat src, Mat dst, int radius = 3, double eps = 0.02)
        {
            int depth = guide.NumberOfChannels;
            Debug.Assert(depth == 1 || depth == 3);

            if (depth == 3)
                GuidedFilterColorBy_clarkzjw(guide, src, dst, radius, eps);
            else if (depth == 1)
                GuidedFilterMonoBy_clarkzjw(guide, src, dst, radius, eps);
        }

        public static Image<Gray, byte> GuidedFilterBy_clarkzjw(
                Image<Bgr, byte> guideImage,
                Image<Gray, byte> inputImage,
                int radius,
                double eps
            )
        {
            Mat dst = (new Image<Gray, byte>(inputImage.Size)).Mat;
            GuidedFilterBy_clarkzjw(guideImage.Mat, inputImage.Mat, dst, radius, eps);
            Image<Gray, byte> result = dst.ToImage<Gray, byte>();
            return result;
        }

        public static void GuidedFilterColorBy_clarkzjw(Mat guide, Mat src, Mat dst, int radius, double eps)
        {
            int height = src.Rows;
            int width = src.Cols;
            int widthstep = guide.Step;
            int gwidthstep = src.Step;
            int nch = guide.ElementSize;
            int gnch = src.ElementSize;

            int i, j;
            int m, n;
            int w;
            int e = 0;
            int st_row, ed_row;
            int st_col, ed_col;

            double sum_Ir, sum_Ig, sum_Ib;
            double sum_Ir_square, sum_Ig_square, sum_Ib_square;
            double sum_IrIg, sum_IgIb, sum_IrIb;
            double sum_PiIr, sum_PiIg, sum_PiIb;
            double sum_Pi;

            double A, B, C, D, E, F, G, H, I, J, K, L;
            double X, Y, Z;
            double ak_r, ak_g, ak_b;
            double bk;
            double det;

            double tmp_Ir, tmp_Ig, tmp_Ib;
            double tmp_p, tmp_q;

            //double* v_ak_r = (double*)malloc(sizeof(double) * height * width);
            //double* v_ak_g = (double*)malloc(sizeof(double) * height * width);
            //double* v_ak_b = (double*)malloc(sizeof(double) * height * width);
            //double* v_bk = (double*)malloc(sizeof(double) * height * width);

            double[] v_ak_r = new double[height * width];
            double[] v_ak_g = new double[height * width];
            double[] v_ak_b = new double[height * width];
            double[] v_bk = new double[height * width];

            int count = 0;

            //uchar* data_guide = guide.data;
            //uchar* data_src = src.data;
            //uchar* data_dst = dst.data;

            //Array data_guide = guide.Data;
            //Array data_src = src.Data;
            //Array data_dst = dst.Data;

            byte[] data_guide = guide.GetData();
            byte[] data_src = src.GetData();
            byte[] data_dst = dst.GetData();

            for (i = 0; i < height; i++)
            {
                for (j = 0; j < width; j++)
                {
                    st_row = i - radius; ed_row = i + radius;
                    st_col = j - radius; ed_col = j + radius;

                    st_row = st_row < 0 ? 0 : st_row;
                    ed_row = ed_row >= height ? (height - 1) : ed_row;
                    st_col = st_col < 0 ? 0 : st_col;
                    ed_col = ed_col >= width ? (width - 1) : ed_col;

                    sum_Ir = sum_Ig = sum_Ib = 0;
                    sum_Ir_square = sum_Ig_square = sum_Ib_square = 0;
                    sum_IrIg = sum_IgIb = sum_IrIb = 0;
                    sum_PiIr = sum_PiIg = sum_PiIb = 0;
                    sum_Pi = 0;
                    w = 0;

                    for (m = st_row; m <= ed_row; m++)
                    {
                        for (n = st_col; n <= ed_col; n++)
                        {
                            //tmp_Ib = *(data_guide + m * widthstep + n * nch);
                            //tmp_Ig = *(data_guide + m * widthstep + n * nch + 1);
                            //tmp_Ir = *(data_guide + m * widthstep + n * nch + 2);

                            //tmp_p = *(data_src + m * gwidthstep + n * gnch);

                            //tmp_Ib = (double)data_guide.GetValue(m * widthstep + n * nch);
                            //tmp_Ig = (double)data_guide.GetValue(m * widthstep + n * nch + 1);
                            //tmp_Ir = (double)data_guide.GetValue(m * widthstep + n * nch + 2);

                            //tmp_p = (double)data_src.GetValue(m * gwidthstep + n * gnch);

                            tmp_Ib = (double)data_guide[m * widthstep + n * nch];
                            tmp_Ig = (double)data_guide[m * widthstep + n * nch + 1];
                            tmp_Ir = (double)data_guide[m * widthstep + n * nch + 2];

                            tmp_p = (double)data_src[m * gwidthstep + n * gnch];

                            sum_Ib += tmp_Ib;
                            sum_Ig += tmp_Ig;
                            sum_Ir += tmp_Ir;

                            sum_Ib_square += tmp_Ib * tmp_Ib;
                            sum_Ig_square += tmp_Ig * tmp_Ig;
                            sum_Ir_square += tmp_Ir * tmp_Ir;

                            sum_IrIg += tmp_Ir * tmp_Ig;
                            sum_IgIb += tmp_Ig * tmp_Ib;
                            sum_IrIb += tmp_Ir * tmp_Ib;

                            sum_Pi += tmp_p;
                            sum_PiIb += tmp_p * tmp_Ib;
                            sum_PiIg += tmp_p * tmp_Ig;
                            sum_PiIr += tmp_p * tmp_Ir;

                            w++;
                        }
                    }

                    A = (sum_Ir_square + w * eps) * sum_Ig - sum_Ir * sum_IrIg;
                    B = sum_IrIg * sum_Ig - sum_Ir * (sum_Ig_square + w * eps);
                    C = sum_IrIb * sum_Ig - sum_Ir * sum_IgIb;
                    D = sum_PiIr * sum_Ig - sum_PiIg * sum_Ir;
                    E = (sum_Ir_square + w * eps) * sum_Ib - sum_IrIb * sum_Ir;
                    F = sum_IrIg * sum_Ib - sum_IgIb * sum_Ir;
                    G = sum_IrIb * sum_Ib - (sum_Ib_square + w * eps) * sum_Ir;
                    H = sum_PiIr * sum_Ib - sum_PiIb * sum_Ir;
                    I = (sum_Ir_square + w * eps) * w - sum_Ir * sum_Ir;
                    J = sum_IrIg * w - sum_Ig * sum_Ir;
                    K = sum_IrIb * w - sum_Ib * sum_Ir;
                    L = sum_PiIr * w - sum_Pi * sum_Ir;

                    det = A * F * K + B * G * I + C * E * J - C * F * I - A * G * J - B * E * K;
                    X = D * F * K + B * G * L + C * H * J - C * F * L - D * G * J - B * H * K;
                    Y = A * H * K + D * G * I + C * E * L - C * H * I - D * E * K - A * G * L;
                    Z = A * F * L + B * H * I + D * J * E - D * F * I - B * E * L - A * H * J;

                    ak_r = X / det;
                    ak_g = Y / det;
                    ak_b = Z / det;

                    bk = (sum_PiIg - sum_IrIg * ak_r - (sum_Ig_square + w * eps) * ak_g - sum_IgIb * ak_b) / sum_Ig;

                    //tmp_Ib = *(data_guide + i * widthstep + j * nch);
                    //tmp_Ig = *(data_guide + i * widthstep + j * nch + 1);
                    //tmp_Ir = *(data_guide + i * widthstep + j * nch + 2);

                    //tmp_Ib = (double)data_guide.GetValue(i * widthstep + j * nch);
                    //tmp_Ig = (double)data_guide.GetValue(i * widthstep + j * nch + 1);
                    //tmp_Ir = (double)data_guide.GetValue(i * widthstep + j * nch + 2);

                    tmp_Ib = (double)data_guide[i * widthstep + j * nch];
                    tmp_Ig = (double)data_guide[i * widthstep + j * nch + 1];
                    tmp_Ir = (double)data_guide[i * widthstep + j * nch + 2];

                    tmp_q = ak_b * tmp_Ib + ak_g * tmp_Ig + ak_r * tmp_Ir + bk;
                    tmp_q = tmp_q > 255 ? 255 : (tmp_q < 0 ? 0 : tmp_q);

                    //*(data_dst + i * gwidthstep + j * gnch) = cvRound(tmp_q);
                    //data_dst.SetValue(Math.Round(tmp_q), i * gwidthstep + j * gnch);
                    data_dst[i * gwidthstep + j * gnch] = (byte)Math.Round(tmp_q);

                    v_ak_b[count] = ak_b;
                    v_ak_g[count] = ak_g;
                    v_ak_r[count] = ak_r;
                    v_bk[count] = bk;
                    count++;
                }
            }

            for (i = 0; i < height; i++)
            {
                for (j = 0; j < width; j++)
                {
                    st_row = i - radius; ed_row = i + radius;
                    st_col = j - radius; ed_col = j + radius;

                    st_row = st_row < 0 ? 0 : st_row;
                    ed_row = ed_row >= height ? (height - 1) : ed_row;
                    st_col = st_col < 0 ? 0 : st_col;
                    ed_col = ed_col >= width ? (width - 1) : ed_col;

                    // double ak_r, ak_g, ak_b, bk;
                    ak_r = ak_g = ak_b = bk = 0;

                    int number = 0;
                    for (m = st_row; m <= ed_row; m++)
                    {
                        for (n = st_col; n <= ed_col; n++)
                        {
                            ak_r += v_ak_r[(m) * width + n];
                            ak_g += v_ak_g[(m) * width + n];
                            ak_b += v_ak_b[(m) * width + n];
                            bk += v_bk[(m) * width + n];
                            number++;
                        }
                    }

                    ak_r /= number;
                    ak_g /= number;
                    ak_b /= number;
                    bk /= number;

                    //tmp_Ib = *(data_guide + i * widthstep + j * nch);
                    //tmp_Ig = *(data_guide + i * widthstep + j * nch + 1);
                    //tmp_Ir = *(data_guide + i * widthstep + j * nch + 2);

                    //tmp_Ib = (double)data_guide.GetValue(i * widthstep + j * nch);
                    //tmp_Ig = (double)data_guide.GetValue(i * widthstep + j * nch + 1);
                    //tmp_Ir = (double)data_guide.GetValue(i * widthstep + j * nch + 2);

                    tmp_Ib = (double)data_guide[i * widthstep + j * nch];
                    tmp_Ig = (double)data_guide[i * widthstep + j * nch + 1];
                    tmp_Ir = (double)data_guide[i * widthstep + j * nch + 2];

                    tmp_q = ak_b * tmp_Ib + ak_g * tmp_Ig + ak_r * tmp_Ir + bk;
                    tmp_q = tmp_q > 255 ? 255 : (tmp_q < 0 ? 0 : tmp_q);

                    //*(data_dst + i * gwidthstep + j * gnch) = cvRound(tmp_q);
                    //data_dst.SetValue(Math.Round(tmp_q), i * gwidthstep + j * gnch);
                    data_dst[i * gwidthstep + j * gnch] = (byte)Math.Round(tmp_q);
                }
            }
            //free(v_ak_b);
            //free(v_ak_g);
            //free(v_ak_r);
            //free(v_bk);

            dst.SetTo(data_dst);
        }

        public static void GuidedFilterMonoBy_clarkzjw(Mat guide, Mat src, Mat dst, int radius, double eps)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Image Min, Max

        public static ImageMinMaxResult ImageMinMax(Image<Bgr, Byte> image)
        {
            double[] minValues, maxValues;
            Point[] minLocations, maxLocations;

            image.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

            return new ImageMinMaxResult
            {
                MinValues = minValues,
                MaxValues = maxValues,
                MinLocations = minLocations,
                MaxLocations = maxLocations
            };
        }

        public static ImageMinMaxResult ImageMinMax(Image<Gray, Byte> image)
        {
            double[] minValues, maxValues;
            Point[] minLocations, maxLocations;

            image.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

            return new ImageMinMaxResult
            {
                MinValues = minValues,
                MaxValues = maxValues,
                MinLocations = minLocations,
                MaxLocations = maxLocations
            };
        }

        #endregion

        public static Image<Gray, byte> CalibrateColorsWithHistogramScratching(Image<Gray, byte> image, double w = 0.95)
        {
            // 0 < w < 1; adujusts max and min intensity

            var colorCalibrated = new Image<Gray, byte>(image.Size);
            const int MAX_VAL = 255; // the max intensity of channel c in output image, which is set to 255 to maximize the contrast
            var minMaxResult = ImageHelper.ImageMinMax(image);
            double min = minMaxResult.MinValues[0];
            double max = minMaxResult.MaxValues[0];
            for (int m = 0; m < image.Rows; m++)
            {
                for (int n = 0; n < image.Cols; n++)
                {
                    var I = image[m, n];

                    var intensity = ((I.Intensity - min / w) / (w * max - min / w)) * MAX_VAL;
                    intensity = intensity > 255 ? 255 : Math.Abs(intensity);
                    colorCalibrated[m, n] = new Gray(intensity);
                }
            }
            return colorCalibrated;
        }

        public static Image<Bgr, byte> CalibrateColorsWithHistogramScratching(Image<Bgr, byte> image, double w = 0.95)
        {
            var channels = image.Split();
            for (int i = 0; i < channels.Length; i++)
            {
                channels[i] = CalibrateColorsWithHistogramScratching(channels[i], w);
            }
            return new Image<Bgr, byte>(channels);
        }

        //public static Image<Bgr, byte> AdjustContrast(Image<Bgr, byte> image, double threshold = 10)
        //{
        //    Image<Bgr, byte> result = new Image<Bgr, byte>(image.Size);
        //    for (int i = 0; i < image.Rows; i++)
        //    {
        //        for (int j = 0; j < image.Cols; j++)
        //        {
        //            var B = image[i, j].Blue;
        //            var G = image[i, j].Green;
        //            var R = image[i, j].Red;

        //            double B_ = B;
        //            double G_ = G;
        //            double R_ = R;

        //            // ADJUST THE CONTRAST
        //            var contrast = Math.Pow((100.0 + threshold) / 100.0, 2);
        //            B_ = ((((B_ / 255.0) - 0.5) * contrast) + 0.5) * 255.0;
        //            G_ = ((((G_ / 255.0) - 0.5) * contrast) + 0.5) * 255.0;
        //            R_ = ((((R_ / 255.0) - 0.5) * contrast) + 0.5) * 255.0;

        //            B_ = B_ > 255 ? 255 : Math.Abs(B_);
        //            G_ = G_ > 255 ? 255 : Math.Abs(G_);
        //            R_ = R_ > 255 ? 255 : Math.Abs(R_);

        //            result[i, j] = new Bgr(B_, G_, R_);
        //        }
        //    }
        //    return result;
        //}
    }



    public class BgrChannels
    {
        public BgrChannels(int size)
        {
            B = new double[size];
            G = new double[size];
            R = new double[size];
        }

        public double[] B;
        public double[] G;
        public double[] R;
    }

    public class BgrChannelsByte
    {
        public BgrChannelsByte(int size)
        {
            B = new byte[size];
            G = new byte[size];
            R = new byte[size];
        }

        public byte[] B;
        public byte[] G;
        public byte[] R;
    }

    public class LabChannels
    {
        public LabChannels(int size)
        {
            L = new double[size];    
            A = new double[size];    
            B = new double[size];    
        }

        public double[] L;
        public double[] A;
        public double[] B;
    }

    public class GrayChannel
    {
        public GrayChannel(int size)
        {
            Intensity = new double[size];
        }

        public double[] Intensity;
    }

    public class GrayPixelWithCoords
    {
        public double Intensity;
        public PixelCoords Coords;
    }

    public class PixelCoords
    {
        public PixelCoords(int row, int col)
        {
            Row = row;
            Col = col;
        }
        public int Row;
        public int Col;
    }

    public class SplittedByMask<T>
    {
        public T In;
        public T Out;
    }

    public class GrayImagePixelValueWithPosition
    {
        public double Intensity { get; set; }
        public Point Position { get; set; }
    }

    public class ImageMinMaxResult
    {
        public ImageMinMaxResult()
        {
        }

        public double[] MinValues { get; set; }
        public double[] MaxValues { get; set; }
        public Point[] MinLocations { get; set; }
        public Point[] MaxLocations { get; set; }
    }
}