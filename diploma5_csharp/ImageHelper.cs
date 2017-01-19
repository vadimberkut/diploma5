using System;
using Accord.Math;
using diploma5_csharp.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

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

        public static ChannelAverageResult CalcLabChannelAverage(Image<Lab, Byte> image)
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
            ChannelAverageResult result = new ChannelAverageResult()
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
}