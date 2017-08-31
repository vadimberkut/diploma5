using System;
using diploma5_csharp.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Collections.Generic;

namespace diploma5_csharp
{
    public static class ImageHelper
    {

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


        public static Image<Gray, Byte> TotGray(Image<Bgr, Byte> image)
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
        public static Image<Bgr, Byte> ToHSI(Image<Bgr, Byte> image)
        {
            Image<Bgr, Byte> input = image.Clone();
            Image<Bgr, Byte> result = new Image<Bgr, byte>(image.Size);

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
                    double min = StatisticsHelper.Min(new double[] { R, G, B });
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
        public static Image<Bgr, Byte> ToHSIGitHub(Image<Bgr, Byte> image)
        {
            Image<Bgr, Byte> input = image.Clone();
            Image<Bgr, Byte> result = new Image<Bgr, byte>(image.Size);

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

        public static double[] GetImagePixels(Image<Gray, double> image)
        {
            double[] result = new double[image.Rows * image.Cols];
            for (int m = 0; m < image.Rows; m++)
            {
                for (int n = 0; n < image.Cols; n++)
                {
                    Gray pixel = image[m, n];
                    result[m * n + n] = pixel.Intensity;
                }
            }
            return result;
        }

        #endregion
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

    public class SplittedByMask<T>
    {
        public T In;
        public T Out;
    }
}