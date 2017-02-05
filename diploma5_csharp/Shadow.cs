using System;
using System.Collections.Generic;
using System.Drawing;
using Accord.MachineLearning;
using diploma5_csharp.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Accord.Statistics.Distributions.DensityKernels;

namespace diploma5_csharp
{
    public class Shadow
    {
        //public ShadowDetectionLabParams ShadowDetectionLabParams;

        public Shadow()
        {
            
        }

        public Image<Emgu.CV.Structure.Gray, Byte> DetectUsingLabMethod(Image<Lab, Byte> image, ShadowDetectionLabParams _params)
        {
            Image<Gray, Byte> shadowMask = new Image<Gray, byte>(new Size(image.Width, image.Height));
            var channelAverage = ImageHelper.CalcLabChannelAverage(image);
            var channels = ImageHelper.GetLabChannels(image);
            double stdDevL = StatisticsHelper.StandartDeviation(channels.L);

            double L;
            double A;
            double B;
            for (int i = 0; i < image.Rows; i += 1)
            {
                for (int j = 0; j < image.Cols; j += 1)
                {
                    Lab color = image[i, j];
//                    byte shadowMaskColor = shadowMask.Data[i, j, 0];

                    L = color.X;
                    A = color.Y;
                    B = color.Z;
                    
                    if (channelAverage.A + channelAverage.B <= _params.Threshold)//256
                    {
                        if (L <= (channelAverage.L - stdDevL / 3.0))
                            shadowMask.Data[i, j, 0] = 255;
                        else
                            shadowMask.Data[i, j, 0] = 0;
                    }
                    else
                    {
                        if (L < channelAverage.L && B < channelAverage.B)
                            shadowMask.Data[i, j, 0] = 255;
                        else
                            shadowMask.Data[i, j, 0] = 0;
                    }
                }
            }

            if (_params.ShowWindows)
            {
            }

            return shadowMask;
        }

        public Image<Emgu.CV.Structure.Gray, Byte> DetectUsingMSMethod(Image<Bgr, Byte> image, ShadowDetectionMSParams _params)
        {
            Image<Gray, Byte> shadowMask = new Image<Gray, byte>(new Size(image.Width, image.Height));
            Image<Bgr, Byte> imgGaussian = new Image<Bgr, byte>(new Size(image.Width, image.Height));
            Image<Bgr, Byte> imgMeanShift = new Image<Bgr, byte>(new Size(image.Width, image.Height));
            Image<Gray, Byte> imgMeanShiftGray;

            int kernel = 9; // 5 - 15
            Emgu.CV.CvInvoke.GaussianBlur(image, imgGaussian, new Size(kernel, kernel), 0);

            double sp = 25; //The spatial window radius.
            double sr = 25; //The color window radius.
            int maxLevel = 1; //Maximum level of the pyramid for the segmentation.

            CvInvoke.PyrMeanShiftFiltering(imgGaussian.Mat, imgMeanShift.Mat, sp, sr, maxLevel, new MCvTermCriteria(5, 1));

            //Convert BGR to Gray
            imgMeanShiftGray = ImageHelper.TotGray(imgMeanShift);

            //AVG
            double avg = StatisticsHelper.Average(ImageHelper.GetGrayChannel(imgMeanShiftGray).Intensity);

            //STD DEV
            double stdDev = StatisticsHelper.StandartDeviation(ImageHelper.GetGrayChannel(imgMeanShiftGray).Intensity);

            //Fixed Threshold
            double thresh = avg - stdDev / 3.0;
            //double thresh = avg;
            double maxValue = 255;

            double? customThreshold = _params.Threshold;
            if (customThreshold != null)
                thresh = (double)customThreshold;

            //Apply global binarization
            CvInvoke.Threshold(imgMeanShiftGray, shadowMask, thresh, maxValue, ThresholdType.BinaryInv);

            if (_params.ShowWindows)
            {
                EmguCvWindowManager.Display(imgGaussian, "1_imgGaussian");
                EmguCvWindowManager.Display(imgMeanShift, "2_imgMeanShift");
                EmguCvWindowManager.Display(imgMeanShiftGray, "3_imgMeanShiftGray");
                EmguCvWindowManager.Display(shadowMask, "4_shadowMask");
            }

            return shadowMask;

        }

        public Image<Emgu.CV.Structure.Bgr, Byte> RemoveUsingAditiveMethod(Image<Bgr, Byte> image, Image<Gray, Byte> shadowMask)
        {
            //Image<Bgr, Byte> result = new Image<Bgr, byte>(image.Size);
            Image<Bgr, Byte> result = image.Clone();

            SplittedByMask<BgrChannels> splited = ImageHelper.SplitImageBgrByMask(image, shadowMask);

            List<double> lightAvg = StatisticsHelper.Average(new List<double[]>() { splited.Out.B, splited.Out.G, splited.Out.R });
            List<double> shadowAvg = StatisticsHelper.Average(new List<double[]>() { splited.In.B, splited.In.G, splited.In.R });

            double diffB = lightAvg[0] - shadowAvg[0];
            double diffG = lightAvg[1] - shadowAvg[1];
            double diffR = lightAvg[2] - shadowAvg[2];

            for (int i = 0; i < image.Rows; i += 1)
            {
                for (int j = 0; j < image.Cols; j += 1)
                {
                    Bgr color = image[i, j];
                    Gray maskColor = shadowMask[i, j];

                    if(maskColor.Intensity == 255)
                        result[i, j] = new Bgr(color.Blue + diffB, color.Green + diffG, color.Red + diffR);
                }
            }

            return result;
        }

        public Image<Emgu.CV.Structure.Bgr, Byte> RemoveUsingBasicLightModelMethod(Image<Bgr, Byte> image, Image<Gray, Byte> shadowMask)
        {
            Image<Bgr, Byte> result = image.Clone();

            SplittedByMask<BgrChannels> splited = ImageHelper.SplitImageBgrByMask(image, shadowMask);

            List<double> lightAvg = StatisticsHelper.Average(new List<double[]>() { splited.Out.B, splited.Out.G, splited.Out.R });
            List<double> shadowAvg = StatisticsHelper.Average(new List<double[]>() { splited.In.B, splited.In.G, splited.In.R });
//
//            double ratioB = lightAvg[0] / shadowAvg[0];
//            double ratioG = lightAvg[1] / shadowAvg[1];
//            double ratioR = lightAvg[2] / shadowAvg[2];

            double ratioB = lightAvg[0] / shadowAvg[0] - 1;
            double ratioG = lightAvg[1] / shadowAvg[1] - 1;
            double ratioR = lightAvg[2] / shadowAvg[2] - 1;

            for (int i = 0; i < image.Rows; i += 1)
            {
                for (int j = 0; j < image.Cols; j += 1)
                {
                    Bgr color = image[i, j];
                    Gray maskColor = shadowMask[i, j];

                    //                    if (maskColor.Intensity == 255)
                    //                        result[i, j] = new Bgr(color.Blue * ratioB, color.Green * ratioG, color.Red * ratioR);

                    int ki = maskColor.Intensity == 255 ? 0 : 1;

                    double blue = color.Blue * (ratioB + 1) / (ki * ratioB + 1);
                    double green = color.Green * (ratioG + 1) / (ki * ratioG + 1);
                    double red = color.Red * (ratioR + 1) / (ki * ratioR + 1);

                    blue = blue > 255 ? 255 : (blue < 0 ? 0 : blue);
                    green = green > 255 ? 255 : (green < 0 ? 0 : green);
                    red = red > 255 ? 255 : (red < 0 ? 0 : red);

                    result[i, j] = new Bgr(blue, green, red);
                }
            }
            return result;
        }

        public Image<Emgu.CV.Structure.Bgr, Byte> RemoveUsingCombinedMethod(Image<Bgr, Byte> image, Image<Gray, Byte> shadowMask)
        {
            Image<Bgr, Byte> result = image.Clone();
            Image<Ycc, Byte> YCrCbImage = ImageHelper.ToYCrCb(image);

//            Image<Gray, Byte> lightMask = shadowMask.ThresholdToZeroInv(new Gray(254));// inverse shadow mask
            Image<Gray, Byte> lightMask = shadowMask.ThresholdBinaryInv(new Gray(254), new Gray(255));

            Ycc avgLight = YCrCbImage.GetAverage(lightMask);
            Ycc avgShadow = YCrCbImage.GetAverage(shadowMask);

            double diffY = avgLight.Y - avgShadow.Y;
            double diffCb = avgLight.Cb - avgShadow.Cb;
            double diffCr = avgLight.Cr - avgShadow.Cr;

            double ratioY = avgLight.Y / avgShadow.Y;
            double ratioCb = avgLight.Cb / avgShadow.Cb;
            double ratioCr = avgLight.Cr / avgShadow.Cr;

            for (int i = 0; i < YCrCbImage.Rows; i += 1)
            {
                for (int j = 0; j < YCrCbImage.Cols; j += 1)
                {
                    Ycc color = YCrCbImage[i, j];
                    Gray maskColor = shadowMask[i, j];

                    if (maskColor.Intensity == 255)
                    {
                        int ki = maskColor.Intensity == 255 ? 0 : 1;

                        double y = color.Y + 1*diffY;
                        double cb = color.Cb*ratioCb;
                        double cr = color.Cr*ratioCr;

                        y = y > 255 ? 255 : (y < 0 ? 0 : y);
                        cb = cb > 255 ? 255 : (cb < 0 ? 0 : cb);
                        cr = cr > 255 ? 255 : (cr < 0 ? 0 : cr);

                        YCrCbImage[i, j] = new Ycc(y, cr, cb);
                    }
                }
            }

            CvInvoke.CvtColor(YCrCbImage, result, ColorConversion.YCrCb2Bgr);

            return result;
        }

        public Image<Emgu.CV.Structure.Bgr, Byte> RemoveUsingLabMethod(Image<Bgr, Byte> image, Image<Gray, Byte> shadowMask)
        {
            Image<Bgr, Byte> result = image.Clone();
            Image<Lab, Byte> labImage = ImageHelper.ToLab(image);
            Image<Bgr, Byte> msResult = image.Clone();

            Image<Gray, Byte> lightMask = shadowMask.ThresholdBinaryInv(new Gray(254), new Gray(255));

            Lab avgLight = labImage.GetAverage(lightMask);
            Lab avgShadow = labImage.GetAverage(shadowMask);


            //Apply Mean shift
            // Use a fixed seed for reproducibility
            Accord.Math.Random.Generator.Seed = 0;

            // Declare some data to be clustered
//            double[][] input =
//            {
//                new double[] { -5, -2, -4 },
//                new double[] { -5, -5, -6 },
//                new double[] {  2,  1,  1 },
//                new double[] {  1,  1,  2 },
//                new double[] {  1,  2,  2 },
//                new double[] {  3,  1,  2 },
//                new double[] { 11,  5,  4 },
//                new double[] { 15,  5,  6 },
//                new double[] { 10,  5,  6 },
//            };

            var arr = labImage.ManagedArray;
            var data = labImage.Data;
            var bytes = labImage.Bytes;

            int length = labImage.Rows*labImage.Cols;
            double[][] input = new double[length][];

            for (int i = 0; i < labImage.Rows; i += 1)
            {
                for (int j = 0; j < labImage.Cols; j += 1)
                {
                    int index = labImage.Width * i + j;
                    input[index] = new double[labImage.NumberOfChannels];

                    for (int k = 0; k < labImage.NumberOfChannels; k += 1)
                    {
                        input[index][k] = data[i, j, k];
                    }
                }
            }

            // Create a uniform kernel density function
            UniformKernel kernel = new UniformKernel();

            // Create a new Mean-Shift algorithm for 3 dimensional samples
            MeanShift meanShift = new MeanShift(dimension: 3, kernel: kernel, bandwidth: 2);

            // Learn a data partitioning using the Mean Shift algorithm
            MeanShiftClusterCollection clustering = meanShift.Learn(input);

            // Predict group labels for each point
            int[] labels = clustering.Decide(input);

            // As a result, the first two observations should belong to the
            //  same cluster (thus having the same label). The same should
            //  happen to the next four observations and to the last three.

            //Display Result
            Dictionary<int,Bgr> labelColors = new Dictionary<int, Bgr>();
            for (int i = 0; i < labImage.Rows; i += 1)
            {
                for (int j = 0; j < labImage.Cols; j += 1)
                {
                    int index = labImage.Width * i + j;
                    int label = labels[index];

                    //add color for label
                    if (!labelColors.ContainsKey(label))
                    {
                        Random rand = new Random();
                        labelColors.Add(label, new Bgr(rand.Next(256), rand.Next(256), rand.Next(256)));
                    }
                    Bgr labelColor = labelColors[label];

                    msResult[i, j] = labelColor;

                }
            }
            EmguCvWindowManager.Display(msResult, "1_msResult");

            //determine regions contain both shadow and non-shadow pixels

            return result;
        }

        public Image<Emgu.CV.Structure.Bgr, Byte> RemoveUsingConstantMethod(Image<Bgr, Byte> image)
        {
            return image;
        }
    }
}