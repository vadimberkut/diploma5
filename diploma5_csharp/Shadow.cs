using System;
using System.Drawing;
using diploma5_csharp.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

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
    }
}