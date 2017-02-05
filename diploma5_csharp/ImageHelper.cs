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