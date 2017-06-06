using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Accord.Imaging.Converters;
using Accord.MachineLearning;
using Accord.Math;
using diploma5_csharp.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Accord.Statistics.Distributions.DensityKernels;
using Emgu.CV.Util;

namespace diploma5_csharp
{
    public class Shadow
    {
        //public ShadowDetectionLabParams ShadowDetectionLabParams;

        public Shadow()
        {
            
        }

        //DETECTION
        public Image<Gray, Byte> DetectUsingLabMethod(Image<Lab, Byte> image, ShadowDetectionLabParams _params)
        {
            Image<Gray, Byte> shadowMask = new Image<Gray, byte>(new Size(image.Width, image.Height));
            var channelAverage = ImageHelper.CalcLabChannelAverage(image);
            var channels = ImageHelper.GetLabChannels(image);
            double stdDevL = StatisticsHelper.StandartDeviation(channels.L);

            //Check params
            if (_params.Threshold == null)
                _params.Threshold = 250;

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

        public Image<Gray, Byte> DetectUsingMSMethod(Image<Bgr, Byte> image, ShadowDetectionMSParams _params)
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

            //Set threshold from params or calc it
            double thresh;
            double maxValue = 255;

            if (_params.Threshold != null)
            {
                thresh = (double)_params.Threshold;
            }
            else
            {
                //AVG
                double avg = StatisticsHelper.Average(ImageHelper.GetGrayChannel(imgMeanShiftGray).Intensity);

                //STD DEV
                double stdDev = StatisticsHelper.StandartDeviation(ImageHelper.GetGrayChannel(imgMeanShiftGray).Intensity);

                //Fixed Threshold
                thresh = avg - stdDev / 3.0;
            }
            _params.Threshold = thresh;

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


        //REMOVAL
        public Image<Bgr, Byte> RemoveUsingAditiveMethod(Image<Bgr, Byte> image, Image<Gray, Byte> shadowMask, ShadowRemovalParams _params)
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

        public Image<Bgr, Byte> RemoveUsingBasicLightModelMethod(Image<Bgr, Byte> image, Image<Gray, Byte> shadowMask, ShadowRemovalParams _params)
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

        public Image<Bgr, Byte> RemoveUsingCombinedMethod(Image<Bgr, Byte> image, Image<Gray, Byte> shadowMask, ShadowRemovalParams _params)
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

        public Image<Bgr, Byte> RemoveUsingLabMethod(Image<Bgr, Byte> image, Image<Gray, Byte> shadowMask, ShadowRemovalParams _params)
        {
            Image<Bgr, Byte> result = image.Clone();
            Image<Lab, Byte> labImage = ImageHelper.ToLab(image);
            Image<Bgr, Byte> msResult = image.Clone();
            Image<Lab, Byte> imgResLAB = ImageHelper.ToLab(image);

            Image<Gray, Byte> lightMask = shadowMask.ThresholdBinaryInv(new Gray(254), new Gray(255));

            Lab avgLight = labImage.GetAverage(lightMask);
            Lab avgShadow = labImage.GetAverage(shadowMask);


            //Apply Mean shift
            // Use a fixed seed for reproducibility
           // Accord.Math.Random.Generator.Seed = 0;

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

            //var arr = labImage.ManagedArray;
            //var data = labImage.Data;
            //var bytes = labImage.Bytes;

            //int length = labImage.Rows*labImage.Cols;
            //double[][] input = new double[length][];

            //for (int i = 0; i < labImage.Rows; i += 1)
            //{
            //    for (int j = 0; j < labImage.Cols; j += 1)
            //    {
            //        int index = labImage.Width * i + j;
            //        input[index] = new double[labImage.NumberOfChannels];

            //        for (int k = 0; k < labImage.NumberOfChannels; k += 1)
            //        {
            //            input[index][k] = data[i, j, k];
            //        }
            //    }
            //}

            //// Create a uniform kernel density function
            //UniformKernel kernel = new UniformKernel();

            //// Create a new Mean-Shift algorithm for 3 dimensional samples
            //MeanShift meanShift = new MeanShift(dimension: 3, kernel: kernel, bandwidth: 2);

            //// Learn a data partitioning using the Mean Shift algorithm
            //MeanShiftClusterCollection clustering = meanShift.Learn(input);

            //// Predict group labels for each point
            //int[] labels = clustering.Decide(input);

            ////Display Result
            //Dictionary<int,Bgr> labelColors = new Dictionary<int, Bgr>();
            //for (int i = 0; i < labImage.Rows; i += 1)
            //{
            //    for (int j = 0; j < labImage.Cols; j += 1)
            //    {
            //        int index = labImage.Width * i + j;
            //        int label = labels[index];

            //        //add color for label
            //        if (!labelColors.ContainsKey(label))
            //        {
            //            Random rand = new Random();
            //            labelColors.Add(label, new Bgr(rand.Next(256), rand.Next(256), rand.Next(256)));
            //        }
            //        Bgr labelColor = labelColors[label];

            //        msResult[i, j] = labelColor;

            //    }
            //}
            //EmguCvWindowManager.Display(msResult, "1_msResult");


            ///////////
            int pixelSize = 3;   // RGB color pixel
            double sigma = 0.06; // kernel bandwidth

            // Load a test image (shown below)
            Bitmap image2 = labImage.Bitmap;

            // Create converters
            ImageToArray imageToArray = new ImageToArray(min: -1, max: +1);
            ArrayToImage arrayToImage = new ArrayToImage(image2.Width, image2.Height, min: -1, max: +1);

            // Transform the image into an array of pixel values
            double[][] pixels;
            imageToArray.Convert(image2, out pixels);

            // Create a MeanShift algorithm using given bandwidth
            //   and a Gaussian density kernel as kernel function.
            MeanShift meanShift = new MeanShift(pixelSize, new GaussianKernel(3), sigma);

            // We will compute the mean-shift algorithm until the means
            // change less than 0.5 between two iterations of the algorithm
            meanShift.Tolerance = 0.05;
            meanShift.MaxIterations = 10;

            // Learn the clusters in the data
            var clustering = meanShift.Learn(pixels);

            // Use clusters to decide class labels
            int[] labels = clustering.Decide(pixels);
            int regionCount = labels.DistinctCount();

            // Replace every pixel with its corresponding centroid
            pixels.ApplyInPlace((x, i) => meanShift.Clusters.Modes[labels[i]]);

            // Retrieve the resulting image in a picture box
            Bitmap result2;
            arrayToImage.Convert(pixels, out result2);
            msResult = new Image<Bgr, byte>(result2);
            if (_params.ShowWindows)
                EmguCvWindowManager.Display(msResult, "1_msResult");
            //////

            //determine regions contain both shadow and non-shadow pixels
            List<int> regions_to_separate = new List<int>();
            for (int r = 0; r < regionCount; r++)
            {
                int CURRENT_LABEL = r;
                bool is_non_shadow_region = false;
                bool is_shadow_region = false;
                for (int i = 0; i < shadowMask.Rows; i++)
                {
                    bool break_ = false;
                    for (int j = 0; j < shadowMask.Cols; j++)
                    {

                        //Handle current region
                        //int cl = ilabels[i][j];
                        int index = labImage.Width * i + j;
                        int label = labels[index];
                        if (label != CURRENT_LABEL)
                        {
                            continue;
                        }

                        //Handle shadow region
                        Gray shadowPixelMask = shadowMask[i, j];
                        if (shadowPixelMask.Intensity == 255)
                        {
                            is_non_shadow_region = true;
                        }
                        else
                        {
                            is_shadow_region = true;
                        }

                        if (is_non_shadow_region && is_shadow_region)
                        {
                            regions_to_separate.Add(CURRENT_LABEL);
                            break_ = true;
                            break;
                        }
                    }
                    if (break_) break;
                }
            }

            //separate such regions on two (old label - non shadow, new label - shadow region)
            for (int r = 0; r != regions_to_separate.Count; r++)
            {
                int CURRENT_LABEL = regions_to_separate[r];
                for (int i = 0; i < shadowMask.Rows; i++)
                {
                    for (int j = 0; j < shadowMask.Cols; j++)
                    {
                        //Handle current region
//                        int cl = ilabels[i][j];
                        int index = labImage.Width * i + j;
                        int label = labels[index];
                        if (label != CURRENT_LABEL)
                        {
                            continue;
                        }

                        Gray shadowPixelMask = shadowMask[i, j];
                        //Assign new label for shadow pixels AND Leave old label for non shadow pixels
                        if (shadowPixelMask.Intensity == 255)
                        {
//                            ilabels[i][j] = regionCount;
                            labels[index] = regionCount;
                        }
                    }
                }
                regionCount += 1;
            }

            // Draw random color for new segmentaion
            List<int> color = new List<int>();
            //            CvRNG rng = cvRNG(cvGetTickCount());

            Random rnd = new Random();
            for (int i = 0; i < regionCount; i++)
            {
//                MCvScalar rcolor = ImageHelper.GenerateRandomColor();
//                color.Add((int)rcolor.V0);
                color.Add(rnd.Next(0, 255));
            }

            Image<Bgr, Byte> imgSegmentationResNew = image.Clone();
            for (int i = 0; i < imgSegmentationResNew.Rows; i++)
            {
                for (int j = 0; j < imgSegmentationResNew.Cols; j++)
                {
                    Bgr pixel = imgSegmentationResNew[i,j];

//                    int cl = ilabels[i][j];
                    int index = labImage.Width * i + j;
                    int label = labels[index];

                    double B = (color[label]) & 255;
//                    double G = (color[label] >> 8) & 255;
//                    double R = (color[label] >> 16) & 255;
                    double G = (color[label] >> 2) & 255;
                    double R = (color[label] << 2) & 255;

                    imgSegmentationResNew[i, j] = new Bgr(B, G, R);
                }
            }
            if (_params.ShowWindows)
                EmguCvWindowManager.Display(imgSegmentationResNew, "2_imgSegmentationResNew");


            //Count pixels for each region and determine shadow regions
            int[] labelsCount = new int[regionCount];
            bool[] isShadowRegion = new bool[regionCount];
            for (int r = 0; r < regionCount; r++)
            {
                labelsCount[r] = 0;
                isShadowRegion[r] = false;
            }
            for (int r = 0; r < regionCount; r++)
            {
                int CURRENT_LABEL = r;
                for (int i = 0; i < labImage.Rows; i++)
                {
                    for (int j = 0; j < labImage.Cols; j++)
                    {
                        //Handle current region
                        int index = labImage.Width*i + j;
                        int label = labels[index];
                        if (label != CURRENT_LABEL)
                        {
                            continue;
                        }
                        labelsCount[r] += 1; //Count pixels
                        if (shadowMask[i,j].Intensity == 255) //is shadow region?
                            isShadowRegion[r] = true;
                    }
                }
            }

            //Define adjacent regions matrix
            bool[,] adjacentRegionsMatrix = new bool[regionCount, regionCount];
            for (int i = 0; i < regionCount; i++)
            {
                for (int j = 0; j < regionCount; j++)
                {
                    adjacentRegionsMatrix[i,j] = false;
                }
            }
            int minDev = 1;
            int maxDev = 10;
            for (int dev = minDev; dev <= maxDev; dev++)
            {
                for (int r = 0; r < regionCount; r++)
                {
                    int CURRENT_LABEL = r;
                    for (int i = 0; i < labImage.Rows; i++)
                    {
                        for (int j = 0; j < labImage.Cols; j++)
                        {
                            //Handle current region
                            int index = labImage.Width * i + j;
                            int label = labels[index];
                            if (label != CURRENT_LABEL)
                            {
                                continue;
                            }

                            //Look for adjacent region's pixels

                            //Go 1px to 4 base direction - up,down,left,right
                            int ANOTHER_LABEL;
                            index = labImage.Width*(i + dev) + j;
                            if (i + dev < labImage.Rows && labels[index] != CURRENT_LABEL)
                            {
                                ANOTHER_LABEL = labels[index];
                                adjacentRegionsMatrix[CURRENT_LABEL,ANOTHER_LABEL] = true;
                            }

                            index = labImage.Width * (i - dev) + j;
                            if (i - dev >= 0 && labels[index] != CURRENT_LABEL)
                            {
                                ANOTHER_LABEL = labels[index];
                                adjacentRegionsMatrix[CURRENT_LABEL,ANOTHER_LABEL] = true;
                            }

                            index = labImage.Width * i + j + dev;
                            if (j + dev < labImage.Cols && labels[index] != CURRENT_LABEL)
                            {
                                ANOTHER_LABEL = labels[index];
                                adjacentRegionsMatrix[CURRENT_LABEL, ANOTHER_LABEL] = true;
                            }

                            index = labImage.Width * i + j - dev;
                            if (j - dev >= 0 && labels[index] != CURRENT_LABEL)
                            {
                                ANOTHER_LABEL = labels[index];
                                adjacentRegionsMatrix[CURRENT_LABEL, ANOTHER_LABEL] = true;
                            }
                        }
                    }
                }
            }

            //define small regions near shadow border to exclude from regions for align
            Image<Gray, Byte> imgShadowMaskDilated = shadowMask.Clone();
            int inpaintDilationKernelSize = 23;
            Mat elementD = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(inpaintDilationKernelSize, inpaintDilationKernelSize), new Point(-1, -1));
            CvInvoke.Dilate(shadowMask, imgShadowMaskDilated, elementD, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());

            bool[] regionsNotForAlign = new bool[regionCount];
            for (int region = 0; region < regionCount; region++)
            {
                if (isShadowRegion[region] == true)
                    regionsNotForAlign[region] = true;
                else
                    regionsNotForAlign[region] = false;
            }

            for (int currentRegion = 0; currentRegion < regionCount; currentRegion++)
            {
                //skip shadow regions - take non-shadow region
                if (isShadowRegion[currentRegion] == true)
                    continue;

                //Count shadow pixels using Dilated Mask
                int countAllPixels = 0;
                int countShadowPixels = 0;
                int countNonShadowPixels = 0;

                for (int i = 0; i < imgShadowMaskDilated.Rows; i++)
                {
                    for (int j = 0; j < imgShadowMaskDilated.Cols; j++)
                    {

                        //Handle current region
//                        int cl = ilabels[i][j];
                        int index = labImage.Width * i + j;
                        int label = labels[index];
                        if (label != currentRegion)
                        {
                            continue;
                        }

                        Gray dilatedShadowMaskPixel  = imgShadowMaskDilated[i, j];
                        //is shadow region?
                        if (dilatedShadowMaskPixel.Intensity == 255)
                            countShadowPixels += 1;
                        else
                            countNonShadowPixels += 1;
                    }
                }
                countAllPixels = countShadowPixels + countNonShadowPixels;

                //
                if (countShadowPixels > countNonShadowPixels)
                {
                    regionsNotForAlign[currentRegion] = true;
                }
            }

            bool[] shadowRegionsUsedForAlign = new bool[regionCount]; //handled shadow regions that can be used to align remaining shadow regions
            bool[] shadowRegionsWithNoLighAdjacentRegions = new bool[regionCount]; //
            for (int i = 0; i < regionCount; i++)
            {
                shadowRegionsUsedForAlign[i] = false;
                shadowRegionsWithNoLighAdjacentRegions[i] = false;
            }

            //define shadow regions for relight
//            std::vector<int> shadowRegionsForRelight;
            List<int> shadowRegionsForRelight = new List<int>();
            for (int currentRegion = 0; currentRegion < regionCount; currentRegion++)
            {
                //skip non-shadow regions - take shadow region
                if (isShadowRegion[currentRegion] == false)
                    continue;

                shadowRegionsForRelight.Add(currentRegion);
            }

            //loop through shadow regions and find adjacent non-shadow
            int MAX_ITERATIONS = 1000;
            //for (int currentRegion = 0; currentRegion < regionCount; currentRegion++)
            for (int iteration = 0; shadowRegionsForRelight.Count > 0; iteration++)
            {
                if (iteration >= MAX_ITERATIONS)
                    break;

                //int currentRegion = shadowRegionsForRelight[0];
//                int currentRegion = *(shadowRegionsForRelight.begin());
                int currentRegion = shadowRegionsForRelight[0];

                //find non-shadow adjacent regions or aligned shadow regions
                List<int> adjacentNonShadowRegions = new List<int>();
                for (int k = 0; k < regionCount; k++)
                {
                    bool isAdjacentRegion = adjacentRegionsMatrix[currentRegion,k];
                    bool isRegionNotForAlign = regionsNotForAlign[k];
                    bool isShadowRegion___ = isShadowRegion[k];
                    bool isShadowRegionForAlign = shadowRegionsUsedForAlign[k];
                    if (isAdjacentRegion == true && isRegionNotForAlign == false && (isShadowRegion___ == false || (isShadowRegionForAlign == true && shadowRegionsWithNoLighAdjacentRegions[currentRegion])))
                    {
                        adjacentNonShadowRegions.Add(k);
                    }
                }

                //Calc avg values for current shadow region
                double L_shadow_avg = 0;
                double A_shadow_avg = 0;
                double B_shadow_avg = 0;
                int currentRegionCount = labelsCount[currentRegion];

                double L_non_shadow_avg = 0;
                double A_non_shadow_avg = 0;
                double B_non_shadow_avg = 0;
                int adjacentRegionCount = 0;

                int adjacentRegionForAlign = -1;

                for (int i = 0; i < imgResLAB.Rows; i++)
                {
                    for (int j = 0; j < imgResLAB.Cols; j++)
                    {

                        //Handle current region
                        //                        int cl = ilabels[i][j];
                        int index = labImage.Width * i + j;
                        int label = labels[index];
                        if (label != currentRegion)
                        {
                            continue;
                        }

                        Lab pixel = imgResLAB[i, j];

                        L_shadow_avg += pixel.X;
                        A_shadow_avg += pixel.Y;
                        B_shadow_avg += pixel.Z;
                    }
                }
                L_shadow_avg /= currentRegionCount;
                A_shadow_avg /= currentRegionCount;
                B_shadow_avg /= currentRegionCount;

                //Find adjacent region that closest in chromatacity
                double chromaDeltaEMetric = 1000000000000;
                double luminanceAndChromaDeltaEMetric = 1000000000000;
                //double MaxDeltaEMetric = 12; // if more - only relight
                for (int r2 = 0; r2 != adjacentNonShadowRegions.Count; r2++)
                {
                    int adjacentNonShadowRegion = adjacentNonShadowRegions[r2];
                    adjacentRegionCount = labelsCount[adjacentNonShadowRegion];

                    //Calc avg values for adjacent non-shadow region
                    double L_non_shadow_avg_ = 0;
                    double A_non_shadow_avg_ = 0;
                    double B_non_shadow_avg_ = 0;

                    for (int i = 0; i < imgResLAB.Rows; i++)
                    {
                        for (int j = 0; j < imgResLAB.Cols; j++)
                        {

                            //Handle adjacent region
//                            int cl = ilabels[i][j];
                            int index = labImage.Width * i + j;
                            int label = labels[index];
                            if (label != adjacentNonShadowRegion)
                            {
                                continue;
                            }

                            //cv::Vec3b & pixel = imgResLAB.at<cv::Vec3b>(i, j);
                            Lab pixel = imgResLAB[i, j];

                            L_non_shadow_avg_ += pixel.X;
                            A_non_shadow_avg_ += pixel.Y;
                            B_non_shadow_avg_ += pixel.Z;
                        }
                    }
                    L_non_shadow_avg_ /= adjacentRegionCount;
                    A_non_shadow_avg_ /= adjacentRegionCount;
                    B_non_shadow_avg_ /= adjacentRegionCount;

                    //Compare chrom values to define minimum diff region
                    double currentChromaDeltaEMetric = ColorDifference.CIE76.GetMetric(new double[2] { A_shadow_avg, B_shadow_avg }, new double[2] { A_non_shadow_avg_, B_non_shadow_avg_ }, 2);
                    double currentLuminanceAndChromaDeltaEMetric = ColorDifference.CIE76.GetMetric(new double[3] { L_shadow_avg, A_shadow_avg, B_shadow_avg }, new double[3] { L_non_shadow_avg_, A_non_shadow_avg_, B_non_shadow_avg_ }, 3);
                    if (currentChromaDeltaEMetric < chromaDeltaEMetric)
                    {
                        chromaDeltaEMetric = currentChromaDeltaEMetric;
                        adjacentRegionForAlign = adjacentNonShadowRegion;

                        L_non_shadow_avg = L_non_shadow_avg_;
                        A_non_shadow_avg = A_non_shadow_avg_;
                        B_non_shadow_avg = B_non_shadow_avg_;
                    }
                }

                //if adjacent region not found, mark current shadow region as "handle in the end" 
                if (adjacentRegionForAlign == -1)
                {
                    //replace current(first) item with other
//                    shadowRegionsForRelight.erase(shadowRegionsForRelight.begin());
                    shadowRegionsForRelight.RemoveAt(0);
                    //shadowRegionsForRelight.push_back(currentRegion);
                    shadowRegionsForRelight.Add(currentRegion);

                    shadowRegionsWithNoLighAdjacentRegions[currentRegion] = true;

                    continue;
                }

                //Visualize current regions
//                cv::Mat imgAdjacentRegions(imgForMeanShiftCluster);
                Image<Bgr, Byte> imgAdjacentRegions = image.Clone();
                for (int i = 0; i < imgAdjacentRegions.Rows; i++)
                {
                    for (int j = 0; j < imgAdjacentRegions.Cols; j++)
                    {
                        //cv::Vec3b & pixel = imgAdjacentRegions.at<cv::Vec3b>(i, j);
                        // Bgr pixel = imgAdjacentRegions[i,j];
                        Bgr pixel = imgAdjacentRegions[i,j];

//                        int cl = ilabels[i][j];
                        int index = labImage.Width * i + j;
                        int label = labels[index];
                        if (label == currentRegion || label == adjacentRegionForAlign)
                        {

                            //                            pixel.val[0] = (color[cl]) & 255;
                            //                            pixel.val[1] = (color[cl] >> 8) & 255;
                            //                            pixel.val[2] = (color[cl] >> 16) & 255;

                            double B = (color[label]) & 255;
//                            double G = (color[label] >> 8) & 255;
//                            double R = (color[label] >> 16) & 255;
                            double G = (color[label] >> 2) & 255;
                            double R = (color[label] << 2) & 255;

                            imgAdjacentRegions[i, j] = new Bgr(B, G, R);
                        }
                        else
                        {
                            imgAdjacentRegions[i, j] = new Bgr(0, 0, 0);
                        }
                    }
                }
                if (_params.ShowWindows)
                {
//                    char integer_string[32];
//                    int integer = currentRegion;
//                    sprintf(integer_string, "%d", integer);
//                    char integer_string2[32];
//                    int integer2 = adjacentRegionForAlign;
//                    sprintf(integer_string2, "%d", integer2);
//                    char spliter[2] = "_";
//                    std::string windowName = "imgAdjacentRegions";
//                    windowName += (integer_string);
//                    windowName += (spliter);
//                    windowName += (integer_string2);
//                    cv::imshow(windowName, imgAdjacentRegions);


                    EmguCvWindowManager.Display(imgAdjacentRegions, $"{currentRegion}-${adjacentRegionForAlign}");
                }

                //Find values
                double L_diff_ = L_non_shadow_avg - L_shadow_avg;
                double A_diff_ = A_non_shadow_avg - A_shadow_avg;
                double B_diff_ = B_non_shadow_avg - B_shadow_avg;

                double L_ratio = L_non_shadow_avg / L_shadow_avg;
                double A_ratio = A_non_shadow_avg / A_shadow_avg;
                double B_ratio = B_non_shadow_avg / B_shadow_avg;

                //mark current shadow regions as relighted
                shadowRegionsUsedForAlign[currentRegion] = true;
                regionsNotForAlign[currentRegion] = false;
                //shadowRegionsForRelight.erase(shadowRegionsForRelight.begin()); //delete  first
                shadowRegionsForRelight.RemoveAt(0); //delete  first

                //Perform COLOR ALIGNMENT 
                for (int i = 0; i < imgResLAB.Rows; i++)
                {
                    for (int j = 0; j < imgResLAB.Cols; j++)
                    {

                        //Handle current region
                        //int cl = ilabels[i][j];
                        int index = labImage.Width * i + j;
                        int label = labels[index];
                        if (label != currentRegion)
                        {
                            continue;
                        }

                        Lab pixel = imgResLAB[i, j];

                        double L = pixel.X;
                        double A = pixel.Y;
                        double B = pixel.Z;

                        L += L_diff_;
                        A *= A_ratio;
                        B *= B_ratio;

                        L = (L > 255 ? 255 : (L < 0 ? 0 : L));
                        A = (A > 255 ? 255 : (A < 0 ? 0 : A));
                        B = (B > 255 ? 255 : (B < 0 ? 0 : B));

//                        pixel.val[0] = L;
//                        pixel.val[1] = A;
//                        pixel.val[2] = B;

                        imgResLAB[i, j] = new Lab(L, A, B);
                    }
                }
            }

//            cv::cvtColor(imgResLAB, imgBGRRes, CV_Lab2BGR);
            CvInvoke.CvtColor(imgResLAB, result, ColorConversion.Lab2Bgr);

            return result;
        }

        public Image<Bgr, Byte> RemoveUsingLabMethod2(Image<Bgr, Byte> image, Image<Gray, Byte> shadowMask, ShadowRemovalParams _params)
        {
            Image<Bgr, Byte> result = image.Clone();
            Image<Lab, Byte> labImage = ImageHelper.ToLab(image);
            Image<Bgr, Byte> msResult = image.Clone();
            Image<Lab, Byte> imgResLAB = ImageHelper.ToLab(image);

            Image<Gray, Byte> lightMask = shadowMask.ThresholdBinaryInv(new Gray(254), new Gray(255));

            Lab avgLight = labImage.GetAverage(lightMask);
            Lab avgShadow = labImage.GetAverage(shadowMask);


            //Apply Mean shift
            // Use a fixed seed for reproducibility
            // Accord.Math.Random.Generator.Seed = 0;

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

            //var arr = labImage.ManagedArray;
            //var data = labImage.Data;
            //var bytes = labImage.Bytes;

            //int length = labImage.Rows*labImage.Cols;
            //double[][] input = new double[length][];

            //for (int i = 0; i < labImage.Rows; i += 1)
            //{
            //    for (int j = 0; j < labImage.Cols; j += 1)
            //    {
            //        int index = labImage.Width * i + j;
            //        input[index] = new double[labImage.NumberOfChannels];

            //        for (int k = 0; k < labImage.NumberOfChannels; k += 1)
            //        {
            //            input[index][k] = data[i, j, k];
            //        }
            //    }
            //}

            //// Create a uniform kernel density function
            //UniformKernel kernel = new UniformKernel();

            //// Create a new Mean-Shift algorithm for 3 dimensional samples
            //MeanShift meanShift = new MeanShift(dimension: 3, kernel: kernel, bandwidth: 2);

            //// Learn a data partitioning using the Mean Shift algorithm
            //MeanShiftClusterCollection clustering = meanShift.Learn(input);

            //// Predict group labels for each point
            //int[] labels = clustering.Decide(input);

            ////Display Result
            //Dictionary<int,Bgr> labelColors = new Dictionary<int, Bgr>();
            //for (int i = 0; i < labImage.Rows; i += 1)
            //{
            //    for (int j = 0; j < labImage.Cols; j += 1)
            //    {
            //        int index = labImage.Width * i + j;
            //        int label = labels[index];

            //        //add color for label
            //        if (!labelColors.ContainsKey(label))
            //        {
            //            Random rand = new Random();
            //            labelColors.Add(label, new Bgr(rand.Next(256), rand.Next(256), rand.Next(256)));
            //        }
            //        Bgr labelColor = labelColors[label];

            //        msResult[i, j] = labelColor;

            //    }
            //}
            //EmguCvWindowManager.Display(msResult, "1_msResult");

            ///////////
            //int pixelSize = 3;   // RGB color pixel
            //int kernel = 3;
            //double sigma = 0.06; // kernel bandwidth
            int pixelSize = 3;   // RGB color pixel
            int kernel = 7; 
            double sigma = 0.11; // kernel bandwidth

            // Load a test image (shown below)
            Bitmap image2 = labImage.Bitmap;

            // Create converters
            ImageToArray imageToArray = new ImageToArray(min: -1, max: +1);
            ArrayToImage arrayToImage = new ArrayToImage(image2.Width, image2.Height, min: -1, max: +1);

            // Transform the image into an array of pixel values
            double[][] pixels;
            imageToArray.Convert(image2, out pixels);

            // Create a MeanShift algorithm using given bandwidth
            //   and a Gaussian density kernel as kernel function.
            MeanShift meanShift = new MeanShift(pixelSize, new GaussianKernel(kernel), sigma);

            // We will compute the mean-shift algorithm until the means
            // change less than 0.5 between two iterations of the algorithm
            meanShift.Tolerance = 0.05;
            meanShift.MaxIterations = 10;

            // Learn the clusters in the data
            var clustering = meanShift.Learn(pixels);

            // Use clusters to decide class labels
            int[] labels = clustering.Decide(pixels);
            int regionCount = labels.DistinctCount();

            // Replace every pixel with its corresponding centroid
            pixels.ApplyInPlace((x, i) => meanShift.Clusters.Modes[labels[i]]);

            // Retrieve the resulting image in a picture box
            Bitmap result2;
            arrayToImage.Convert(pixels, out result2);
            msResult = new Image<Bgr, byte>(result2);
            if (_params.ShowWindows)
                EmguCvWindowManager.Display(msResult, "1_msResult");
            //////

            //return image;

            //start count labels from 0
            //labels = labels.Select(l => l - 1).ToArray<int>();


            //determine regions contain both shadow and non-shadow pixels
            List<int> regions_to_separate = new List<int>();
            for (int r = 0; r < regionCount; r++)
            {
                int CURRENT_LABEL = r;
                bool is_non_shadow_region = false;
                bool is_shadow_region = false;
                for (int i = 0; i < shadowMask.Rows; i++)
                {
                    bool break_ = false;
                    for (int j = 0; j < shadowMask.Cols; j++)
                    {

                        //Handle current region
                        //int cl = ilabels[i][j];
                        int index = labImage.Width * i + j;
                        int label = labels[index];
                        if (label != CURRENT_LABEL)
                        {
                            continue;
                        }

                        //Handle shadow region
                        Gray shadowPixelMask = shadowMask[i, j];
                        if (shadowPixelMask.Intensity == 255)
                        {
                            is_non_shadow_region = true;
                        }
                        else
                        {
                            is_shadow_region = true;
                        }

                        if (is_non_shadow_region && is_shadow_region)
                        {
                            regions_to_separate.Add(CURRENT_LABEL);
                            break_ = true;
                            break;
                        }
                    }
                    if (break_) break;
                }
            }

            var asdsd = labels.Distinct();

            //separate such regions on two (old label - non shadow, new label - shadow region)
            for (int r = 0; r != regions_to_separate.Count; r++)
            {
                int CURRENT_LABEL = regions_to_separate[r];
                for (int i = 0; i < shadowMask.Rows; i++)
                {
                    for (int j = 0; j < shadowMask.Cols; j++)
                    {
                        //Handle current region
                        //                        int cl = ilabels[i][j];
                        int index = labImage.Width * i + j;
                        int label = labels[index];
                        if (label != CURRENT_LABEL)
                        {
                            continue;
                        }

                        Gray shadowPixelMask = shadowMask[i, j];
                        //Assign new label for shadow pixels AND Leave old label for non shadow pixels
                        int newRegionNumber = regionCount; //regions start counting from 1. e.g. - 52 regions = from 1 to 52
                        if (shadowPixelMask.Intensity == 255)
                        {
                            //                            ilabels[i][j] = regionCount;
                            labels[index] = newRegionNumber;
                        }
                    }
                }
                regionCount += 1;
            }

            var asd = labels.Distinct();



            //// Draw random color for new segmentaion
            List<int> color = new List<int>();
            Random rnd = new Random();
            for (int i = 0; i < regionCount; i++)
            {
                color.Add(rnd.Next(0, 255));
            }

            Image<Bgr, Byte> imgSegmentationResNew = image.Clone();
            for (int i = 0; i < imgSegmentationResNew.Rows; i++)
            {
                for (int j = 0; j < imgSegmentationResNew.Cols; j++)
                {
                    Bgr pixel = imgSegmentationResNew[i, j];

                    //                    int cl = ilabels[i][j];
                    int index = labImage.Width * i + j;
                    int label = labels[index];

                    double B = (color[label]) & 255;
                    double G = (color[label] >> 2) & 255;
                    double R = (color[label] << 2) & 255;

                    imgSegmentationResNew[i, j] = new Bgr(B, G, R);
                }
            }
            if (_params.ShowWindows)
                EmguCvWindowManager.Display(imgSegmentationResNew, "2_imgSegmentationResNew");


            /////////////////////////////////////////////////////////////////////////////
            //// Replace every pixel with its corresponding centroid
            //pixels.ApplyInPlace((x, i) => meanShift.Clusters.Modes[labels[i]]);

            //// Retrieve the resulting image in a picture box
            //Bitmap result3;
            //arrayToImage.Convert(pixels, out result3);
            //msResult = new Image<Bgr, byte>(result3);
            //if (_params.ShowWindows)
            //    EmguCvWindowManager.Display(msResult, "2_imgSegmentationResNew");

            //////////////////////////////////////////////////////////////////////////



            //Count pixels for each region and determine shadow regions
            int[] labelsCount = new int[regionCount];
            bool[] isShadowRegion = new bool[regionCount];
            for (int r = 0; r < regionCount; r++)
            {
                labelsCount[r] = 0;
                isShadowRegion[r] = false;
            }
            for (int r = 0; r < regionCount; r++)
            {
                int CURRENT_LABEL = r;
                for (int i = 0; i < labImage.Rows; i++)
                {
                    for (int j = 0; j < labImage.Cols; j++)
                    {
                        //Handle current region
                        int index = labImage.Width * i + j;
                        int label = labels[index];
                        if (label != CURRENT_LABEL)
                        {
                            continue;
                        }
                        labelsCount[r] += 1; //Count pixels
                        if (shadowMask[i, j].Intensity == 255) //is shadow region?
                            isShadowRegion[r] = true;
                    }
                }
            }


            //Define adjacent regions matrix
            bool[,] adjacentRegionsMatrix = new bool[regionCount, regionCount];
            for (int i = 0; i < regionCount; i++)
            {
                for (int j = 0; j < regionCount; j++)
                {
                    adjacentRegionsMatrix[i, j] = false;
                }
            }
            int minDev = 5;
            int maxDev = 10;
            for (int dev = minDev; dev <= maxDev; dev++)
            {
                for (int r = 0; r < regionCount; r++)
                {
                    int CURRENT_LABEL = r;
                    for (int i = 0; i < labImage.Rows; i++)
                    {
                        for (int j = 0; j < labImage.Cols; j++)
                        {
                            //Handle current region
                            int index = labImage.Width * i + j;
                            int label = labels[index];
                            if (label != CURRENT_LABEL)
                            {
                                continue;
                            }

                            //Look for adjacent region's pixels

                            //Go 1px to 4 base direction - up,down,left,right
                            int ANOTHER_LABEL;
                            index = labImage.Width * (i + dev) + j;
                            if (i + dev < labImage.Rows && labels[index] != CURRENT_LABEL)
                            {
                                ANOTHER_LABEL = labels[index];
                                adjacentRegionsMatrix[CURRENT_LABEL, ANOTHER_LABEL] = true;
                            }

                            index = labImage.Width * (i - dev) + j;
                            if (i - dev >= 0 && labels[index] != CURRENT_LABEL)
                            {
                                ANOTHER_LABEL = labels[index];
                                adjacentRegionsMatrix[CURRENT_LABEL, ANOTHER_LABEL] = true;
                            }

                            index = labImage.Width * i + j + dev;
                            if (j + dev < labImage.Cols && labels[index] != CURRENT_LABEL)
                            {
                                ANOTHER_LABEL = labels[index];
                                adjacentRegionsMatrix[CURRENT_LABEL, ANOTHER_LABEL] = true;
                            }

                            index = labImage.Width * i + j - dev;
                            if (j - dev >= 0 && labels[index] != CURRENT_LABEL)
                            {
                                ANOTHER_LABEL = labels[index];
                                adjacentRegionsMatrix[CURRENT_LABEL, ANOTHER_LABEL] = true;
                            }
                        }
                    }
                }
            }



            //define small regions near shadow border to exclude from regions for align
            Image<Gray, Byte> imgShadowMaskDilated = shadowMask.Clone();
            int inpaintDilationKernelSize = 23;
            Mat elementD = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(inpaintDilationKernelSize, inpaintDilationKernelSize), new Point(-1, -1));
            CvInvoke.Dilate(shadowMask, imgShadowMaskDilated, elementD, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());

            bool[] regionsNotForAlign = new bool[regionCount];
            for (int region = 0; region < regionCount; region++)
            {
                if (isShadowRegion[region] == true)
                    regionsNotForAlign[region] = true;
                else
                    regionsNotForAlign[region] = false;
            }

            for (int currentRegion = 0; currentRegion < regionCount; currentRegion++)
            {
                //skip shadow regions - take non-shadow region
                if (isShadowRegion[currentRegion] == true)
                    continue;

                //Count shadow pixels using Dilated Mask
                int countAllPixels = 0;
                int countShadowPixels = 0;
                int countNonShadowPixels = 0;

                for (int i = 0; i < imgShadowMaskDilated.Rows; i++)
                {
                    for (int j = 0; j < imgShadowMaskDilated.Cols; j++)
                    {

                        //Handle current region
                        //                        int cl = ilabels[i][j];
                        int index = labImage.Width * i + j;
                        int label = labels[index];
                        if (label != currentRegion)
                        {
                            continue;
                        }

                        Gray dilatedShadowMaskPixel = imgShadowMaskDilated[i, j];
                        //is shadow region?
                        if (dilatedShadowMaskPixel.Intensity == 255)
                            countShadowPixels += 1;
                        else
                            countNonShadowPixels += 1;
                    }
                }
                countAllPixels = countShadowPixels + countNonShadowPixels;

                //
                if (countShadowPixels > countNonShadowPixels)
                {
                    regionsNotForAlign[currentRegion] = true;
                }
            }

            bool[] shadowRegionsUsedForAlign = new bool[regionCount]; //handled shadow regions that can be used to align remaining shadow regions
            bool[] shadowRegionsWithNoLighAdjacentRegions = new bool[regionCount]; //
            for (int i = 0; i < regionCount; i++)
            {
                shadowRegionsUsedForAlign[i] = false;
                shadowRegionsWithNoLighAdjacentRegions[i] = false;
            }

            //define shadow regions for relight
            List<int> shadowRegionsForRelight = new List<int>();
            for (int currentRegion = 0; currentRegion < regionCount; currentRegion++)
            {
                //skip non-shadow regions - take shadow region
                if (isShadowRegion[currentRegion] == false)
                    continue;

                shadowRegionsForRelight.Add(currentRegion);
            }

            //loop through shadow regions and find adjacent non-shadow
            int MAX_ITERATIONS = 1000;
            //for (int currentRegion = 0; currentRegion < regionCount; currentRegion++)
            for (int iteration = 0; shadowRegionsForRelight.Count > 0; iteration++)
            {
                if (iteration >= MAX_ITERATIONS)
                    break;

                //int currentRegion = shadowRegionsForRelight[0];
                //                int currentRegion = *(shadowRegionsForRelight.begin());
                int currentRegion = shadowRegionsForRelight[0];
                //int currentRegion = shadowRegionsForRelight[iteration];

                //find non-shadow adjacent regions or aligned shadow regions
                List<int> adjacentNonShadowRegions = new List<int>();
                for (int k = 0; k < regionCount; k++)
                {
                    bool isAdjacentRegion = adjacentRegionsMatrix[currentRegion, k];
                    bool isRegionNotForAlign = regionsNotForAlign[k];
                    bool isShadowRegion___ = isShadowRegion[k];
                    bool isShadowRegionForAlign = shadowRegionsUsedForAlign[k];
                    if (isAdjacentRegion == true && isRegionNotForAlign == false && (isShadowRegion___ == false || (isShadowRegionForAlign == true && shadowRegionsWithNoLighAdjacentRegions[currentRegion])))
                    {
                        adjacentNonShadowRegions.Add(k);
                    }
                }

                //Calc avg values for current shadow region
                double L_shadow_avg = 0;
                double A_shadow_avg = 0;
                double B_shadow_avg = 0;
                int currentRegionCount = labelsCount[currentRegion];

                double L_non_shadow_avg = 0;
                double A_non_shadow_avg = 0;
                double B_non_shadow_avg = 0;
                int adjacentRegionCount = 0;

                int adjacentRegionForAlign = -1;

                for (int i = 0; i < imgResLAB.Rows; i++)
                {
                    for (int j = 0; j < imgResLAB.Cols; j++)
                    {

                        //Handle current region
                        //                        int cl = ilabels[i][j];
                        int index = labImage.Width * i + j;
                        int label = labels[index];
                        if (label != currentRegion)
                        {
                            continue;
                        }

                        Lab pixel = imgResLAB[i, j];

                        L_shadow_avg += pixel.X;
                        A_shadow_avg += pixel.Y;
                        B_shadow_avg += pixel.Z;
                    }
                }
                L_shadow_avg /= currentRegionCount;
                A_shadow_avg /= currentRegionCount;
                B_shadow_avg /= currentRegionCount;

                //Find adjacent region that closest in chromatacity
                double chromaDeltaEMetric = 1000000000000;
                double luminanceAndChromaDeltaEMetric = 1000000000000;
                //double MaxDeltaEMetric = 12; // if more - only relight
                for (int r2 = 0; r2 != adjacentNonShadowRegions.Count; r2++)
                {
                    int adjacentNonShadowRegion = adjacentNonShadowRegions[r2];
                    adjacentRegionCount = labelsCount[adjacentNonShadowRegion];

                    //Calc avg values for adjacent non-shadow region
                    double L_non_shadow_avg_ = 0;
                    double A_non_shadow_avg_ = 0;
                    double B_non_shadow_avg_ = 0;

                    for (int i = 0; i < imgResLAB.Rows; i++)
                    {
                        for (int j = 0; j < imgResLAB.Cols; j++)
                        {

                            //Handle adjacent region
                            //                            int cl = ilabels[i][j];
                            int index = labImage.Width * i + j;
                            int label = labels[index];
                            if (label != adjacentNonShadowRegion)
                            {
                                continue;
                            }

                            //cv::Vec3b & pixel = imgResLAB.at<cv::Vec3b>(i, j);
                            Lab pixel = imgResLAB[i, j];

                            L_non_shadow_avg_ += pixel.X;
                            A_non_shadow_avg_ += pixel.Y;
                            B_non_shadow_avg_ += pixel.Z;
                        }
                    }
                    L_non_shadow_avg_ /= adjacentRegionCount;
                    A_non_shadow_avg_ /= adjacentRegionCount;
                    B_non_shadow_avg_ /= adjacentRegionCount;

                    //Compare chrom values to define minimum diff region
                    double currentChromaDeltaEMetric = ColorDifference.CIE76.GetMetric(new double[2] { A_shadow_avg, B_shadow_avg }, new double[2] { A_non_shadow_avg_, B_non_shadow_avg_ }, 2);
                    double currentLuminanceAndChromaDeltaEMetric = ColorDifference.CIE76.GetMetric(new double[3] { L_shadow_avg, A_shadow_avg, B_shadow_avg }, new double[3] { L_non_shadow_avg_, A_non_shadow_avg_, B_non_shadow_avg_ }, 3);
                    if (currentChromaDeltaEMetric < chromaDeltaEMetric)
                    {
                        chromaDeltaEMetric = currentChromaDeltaEMetric;
                        adjacentRegionForAlign = adjacentNonShadowRegion;

                        L_non_shadow_avg = L_non_shadow_avg_;
                        A_non_shadow_avg = A_non_shadow_avg_;
                        B_non_shadow_avg = B_non_shadow_avg_;
                    }
                }

                //if adjacent region not found, mark current shadow region as "handle in the end" 
                if (adjacentRegionForAlign == -1)
                {
                    //replace current(first) item with other
                    //                    shadowRegionsForRelight.erase(shadowRegionsForRelight.begin());
                    shadowRegionsForRelight.RemoveAt(0);
                    //shadowRegionsForRelight.push_back(currentRegion);
                    shadowRegionsForRelight.Add(currentRegion);

                    shadowRegionsWithNoLighAdjacentRegions[currentRegion] = true;

                    continue;
                }

                //Visualize current regions
                //                cv::Mat imgAdjacentRegions(imgForMeanShiftCluster);
                Image<Bgr, Byte> imgAdjacentRegions = image.Clone();
                for (int i = 0; i < imgAdjacentRegions.Rows; i++)
                {
                    for (int j = 0; j < imgAdjacentRegions.Cols; j++)
                    {
                        Bgr pixel = imgAdjacentRegions[i, j];

                        int index = labImage.Width * i + j;
                        int label = labels[index];
                        if (label == currentRegion || label == adjacentRegionForAlign)
                        {

                            double B = (color[label]) & 255;
                            double G = (color[label] >> 2) & 255;
                            double R = (color[label] << 2) & 255;

                            imgAdjacentRegions[i, j] = new Bgr(B, G, R);
                        }
                        else
                        {
                            imgAdjacentRegions[i, j] = new Bgr(0, 0, 0);
                        }
                    }
                }
                if (_params.ShowWindows)
                {
                    //EmguCvWindowManager.Display(imgAdjacentRegions, $"{currentRegion}-${adjacentRegionForAlign}");
                }

                //Find values
                double L_diff_ = L_non_shadow_avg - L_shadow_avg;
                double A_diff_ = A_non_shadow_avg - A_shadow_avg;
                double B_diff_ = B_non_shadow_avg - B_shadow_avg;

                double L_ratio = L_non_shadow_avg / L_shadow_avg;
                double A_ratio = A_non_shadow_avg / A_shadow_avg;
                double B_ratio = B_non_shadow_avg / B_shadow_avg;

                //mark current shadow regions as relighted
                shadowRegionsUsedForAlign[currentRegion] = true;
                regionsNotForAlign[currentRegion] = false;
                //shadowRegionsForRelight.erase(shadowRegionsForRelight.begin()); //delete  first
                shadowRegionsForRelight.RemoveAt(0); //delete  first

                //Perform COLOR ALIGNMENT 
                for (int i = 0; i < imgResLAB.Rows; i++)
                {
                    for (int j = 0; j < imgResLAB.Cols; j++)
                    {

                        //Handle current region
                        //int cl = ilabels[i][j];
                        int index = labImage.Width * i + j;
                        int label = labels[index];
                        if (label != currentRegion)
                        {
                            continue;
                        }

                        Lab pixel = imgResLAB[i, j];

                        double L = pixel.X;
                        double A = pixel.Y;
                        double B = pixel.Z;

                        L += L_diff_;
                        A *= A_ratio;
                        B *= B_ratio;

                        L = (L > 255 ? 255 : (L < 0 ? 0 : L));
                        A = (A > 255 ? 255 : (A < 0 ? 0 : A));
                        B = (B > 255 ? 255 : (B < 0 ? 0 : B));

                        //                        pixel.val[0] = L;
                        //                        pixel.val[1] = A;
                        //                        pixel.val[2] = B;

                        imgResLAB[i, j] = new Lab(L, A, B);
                    }
                }
            }

            //            cv::cvtColor(imgResLAB, imgBGRRes, CV_Lab2BGR);
            CvInvoke.CvtColor(imgResLAB, result, ColorConversion.Lab2Bgr);

            return result;
        }

        public Image<Bgr, Byte> RemoveUsingConstantMethod(Image<Bgr, Byte> image, Image<Gray, Byte> shadowMask, ShadowRemovalParams _params)
        {
            Image<Bgr, Byte> result = image.Clone();
            Image<Gray, Byte> shadowMaskEdge = shadowMask.Clone();
            Image<Gray, Byte> shadowMaskEdgeDilated = shadowMask.Clone();

            //Find shadow edges
            double threshold1 = 50;
            double threshold2 = 150;
            int apertureSize = 5;// — розмір для оператора Собеля
            CvInvoke.Canny(shadowMask, shadowMaskEdge, threshold1, threshold2, apertureSize);

            //Dilate shadow edge
            Mat elementD = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle , new Size(3,3), new Point(-1, -1));
            CvInvoke.Dilate(shadowMaskEdge, shadowMaskEdgeDilated, elementD, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());

            #region            //FIND CONTOURS USING findContours

            // extract contours of the canny image:
            VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint(); //Detected contours. Each contour is stored as a vector of points
            Mat hierachy = new Mat();//Optional output vector, containing information about the image topology. 
            RetrType mode = Emgu.CV.CvEnum.RetrType.Tree;//retrieves all of the contours and reconstructs a full hierarchy of nested contours
            //int method = CV_CHAIN_APPROX_SIMPLE; //compresses horizontal, vertical, and diagonal segments and leaves only their end points
            ChainApproxMethod method = ChainApproxMethod.ChainApproxNone; //stores absolutely all the contour points
            CvInvoke.FindContours(shadowMaskEdge, contours, hierachy, mode, method);

            // draw the contours to a copy of the input image:
            Mat outputContour = new Mat();
            CvInvoke.CvtColor(shadowMaskEdge, outputContour, ColorConversion.Gray2Bgr);

            int thickness = 1;
//            int lineType = 8;
            LineType lineType = LineType.EightConnected;
            int maxLevel = 0;
            for (int i = 0; i < contours.Size; i++)
            {
                MCvScalar color = ImageHelper.GenerateRandomColor();
//                MCvScalar color =new MCvScalar(100, 150, 250);
                CvInvoke.DrawContours(outputContour, contours, i, color, thickness, lineType, hierachy, maxLevel);
            }
            if (_params.ShowWindows)
            {
                EmguCvWindowManager.Display(outputContour, "3 outputContour");
            }

            //Sort countours by size descending
            //TODO: WHY SORD US USED ???????
            //var contoursSortedDesc = contours.ToArrayOfArray().OrderByDescending(i => i.Count()).ToList();
            //TODO: NEED TO SORT hierarchy TOO
            //TODO: NEED TO SORT hierarchy TOO
            //TODO: NEED TO SORT hierarchy TOO
            //TODO: NEED TO SORT hierarchy TOO

            //Delete small countours useing threshold
            int MIN_COUNTOUR_THRESHLD = 30;
//            List<int>  indicesToRemove = new List<int>();
            List<List<Point>> _contours = new List<List<Point>>();
            for (int i = 0; i < contours.Size; i++)
            {
//                if (contours[i].Size < MIN_COUNTOUR_THRESHLD)
//                    indicesToRemove.Add(i);
                if (contours[i].Size >= MIN_COUNTOUR_THRESHLD)
                    _contours.Add(contours[i].ToArray().ToList());
            }
            #endregion

            int rows = image.Rows;
            int cols = image.Cols;
//            bool** lightenedPixels = Create2DArray(rows, cols, false); //indicates pixels than already have lightened
            bool[,] lightenedPixels = new bool[rows, cols];

            //Iterate through all countrous
            for (int contour_i = 0; contour_i < _contours.Count; contour_i++)
            {
                var currentContour = _contours[contour_i];
                int currentContourSize = currentContour.Count;

                #region Find pixels adjacent to shadow border (for specific contour)

                List<List<int>> borderAdjacentShadowPixelsIndexes = new List<List<int>>();
                List<List<int>> borderAdjacentNonShadowPixelsIndexes = new List<List<int>>();

                int distance = 10;

                //Loop thorough countour pixels  and determine adjacent
                for (int k = 0; k < currentContour.Count; k++)
                {
                    int j = currentContour[k].X;
                    int i = currentContour[k].Y;

                    if (i - distance >= 0)
                    {
                        if (shadowMask[i - distance, j].Intensity == 255)
                        {
                            borderAdjacentShadowPixelsIndexes.Add(new List<int>() { i - distance, j });
                        }
                        else
                        {
                            borderAdjacentNonShadowPixelsIndexes.Add(new List<int>() { i - distance, j });
                        }
                    }
                    if (i + distance < shadowMaskEdge.Rows)
                    {
                        if (shadowMask[i + distance, j].Intensity == 255)
                        {
                            borderAdjacentShadowPixelsIndexes.Add(new List<int>() { i + distance, j });
                        }
                        else
                        {
                            borderAdjacentNonShadowPixelsIndexes.Add(new List<int>() { i + distance, j });
                        }
                    }

                    if (j - distance >= 0)
                    {
                        if (shadowMask[i, j - distance].Intensity == 255)
                        {
                            borderAdjacentShadowPixelsIndexes.Add(new List<int>() { i, j - distance });
                        }
                        else
                        {
                            borderAdjacentNonShadowPixelsIndexes.Add(new List<int>() { i, j - distance });
                        }
                    }
                    if (j + distance < shadowMaskEdge.Cols)
                    {
                        if (shadowMask[i, j + distance].Intensity == 255)
                        {
                            borderAdjacentShadowPixelsIndexes.Add(new List<int>() { i, j + distance });
                        }
                        else
                        {
                            borderAdjacentNonShadowPixelsIndexes.Add(new List<int>() { i, j + distance });
                        }
                    }
                }

                if (borderAdjacentShadowPixelsIndexes.Count == 0 || borderAdjacentNonShadowPixelsIndexes.Count == 0)
                    continue;

                if (borderAdjacentShadowPixelsIndexes.Count > borderAdjacentNonShadowPixelsIndexes.Count)
                {
                    int count = borderAdjacentShadowPixelsIndexes.Count - borderAdjacentNonShadowPixelsIndexes.Count;
                    for (int i = 0; i < count; i++)
                    {
                        borderAdjacentShadowPixelsIndexes.RemoveAt(borderAdjacentShadowPixelsIndexes.Count - 1);
                    }
                }
                if (borderAdjacentShadowPixelsIndexes.Count < borderAdjacentNonShadowPixelsIndexes.Count)
                {
                    int count = borderAdjacentNonShadowPixelsIndexes.Count - borderAdjacentShadowPixelsIndexes.Count;
                    for (int i = 0; i < count; i++)
                    {
                        borderAdjacentNonShadowPixelsIndexes.RemoveAt(borderAdjacentNonShadowPixelsIndexes.Count - 1);
                    }
                }
                #endregion

                #region FINDING A CONSTANT

                double a_B = 0;
                double a_G = 0;
                double a_R = 0;

                double b_B = 255;
                double b_G = 255;
                double b_R = 255;

                double c_B = 0;
                double c_G = 0;
                double c_R = 0;

                double sigma = 0.005; //accuracy
                int max_step = 100000; //max iterations

                //hold all calculated constant values
                List<double> c_B_values = new List<double>();
                List<double> c_G_values = new List<double>();
                List<double> c_R_values = new List<double>();

                //calsc f value; f = sum (Pi - Si*r)^2

                //R
                for (int i = 0; true; i++)
                {
                    double f_a_R = 0;
                    double f_b_R = 0;

                    for (int r = 0; r != borderAdjacentShadowPixelsIndexes.Count; r++)
                    {
                        List<int> Si_indexes = borderAdjacentShadowPixelsIndexes[r];
                        List<int> Pi_indexes = borderAdjacentNonShadowPixelsIndexes[r];

                        Bgr S_pixel = image[Si_indexes[0], Si_indexes[1]];
                        Bgr P_pixel = image[Pi_indexes[0], Pi_indexes[1]];

                        f_a_R += Math.Pow(P_pixel.Red - S_pixel.Red - a_R, 2);
                        f_b_R += Math.Pow(P_pixel.Red - S_pixel.Red - b_R, 2);
                    }

                    if (f_a_R < f_b_R)
                    {
                        b_R = (a_R + b_R) / 2.0;
                    }
                    else
                    {
                        a_R = (a_R + b_R) / 2.0;
                    }

                    c_R = (a_R + b_R) / 2;
                    c_R_values.Add(c_R);

                    if ((b_R - a_R) <= sigma || i >= max_step)
                    {

                        break;
                    }
                }

                //G
                for (int i = 0; true; i++)
                {
                    double f_a_G = 0;
                    double f_b_G = 0;

                    for (int r = 0; r != borderAdjacentShadowPixelsIndexes.Count; r++)
                    {
                        List<int> Si_indexes = borderAdjacentShadowPixelsIndexes[r];
                        List<int> Pi_indexes = borderAdjacentNonShadowPixelsIndexes[r];

                        Bgr S_pixel = image[Si_indexes[0], Si_indexes[1]];
                        Bgr P_pixel = image[Pi_indexes[0], Pi_indexes[1]];

                        f_a_G += Math.Pow(P_pixel.Green - S_pixel.Green - a_G, 2);
                        f_b_G += Math.Pow(P_pixel.Green - S_pixel.Green - b_G, 2);
                    }

                    if (f_a_G < f_b_G)
                    {
                        b_G = (a_G + b_G) / 2.0;
                    }
                    else
                    {
                        a_G = (a_G + b_G) / 2.0;
                    }

                    c_G = (a_G + b_G) / 2;
                    c_G_values.Add(c_G);

                    if ((((b_G - a_G) <= sigma || i >= max_step)))
                    {

                        break;
                    }
                }

                //B
                for (int i = 0; true; i++)
                {
                    double f_a_B = 0;
                    double f_b_B = 0;

                    for (int r = 0; r != borderAdjacentShadowPixelsIndexes.Count; r++)
                    {
                        List<int> Si_indexes = borderAdjacentShadowPixelsIndexes[r];
                        List<int> Pi_indexes = borderAdjacentNonShadowPixelsIndexes[r];

                        Bgr S_pixel = image[Si_indexes[0], Si_indexes[1]];
                        Bgr P_pixel = image[Pi_indexes[0], Pi_indexes[1]];

                        f_a_B += Math.Pow(P_pixel.Blue - S_pixel.Blue - a_B, 2);
                        f_b_B += Math.Pow(P_pixel.Blue - S_pixel.Blue - b_B, 2);
                    }

                    if (f_a_B < f_b_B)
                    {
                        b_B = (a_B + b_B) / 2.0;
                    }
                    else
                    {
                        a_B = (a_B + b_B) / 2.0;
                    }

                    c_B = (a_B + b_B) / 2;
                    c_B_values.Add(c_B);

                    if (((b_B - a_B) <= sigma || i >= max_step))
                    {
                        break;
                    }
                }

                double c_res_B = c_B;
                double c_res_G = c_G;
                double c_res_R = c_R;

                #endregion

                #region FIND SHADOW PIXELS FOR CURRENT COUNTOUR

                bool[,] passedThroughPixels = new bool[rows, cols];  //indicates pixelsForRelight
                int pixelsHandled = 0;

                GoThroughAllAdjacentShadowPixelsRec(currentContour, ref passedThroughPixels, shadowMask);//V3

                //                if (checkBoxDisplayOptionalWindows->Checked == true)
                //                {
                //                    //visualize
                //                    cv::Mat visualRes = cv::Mat(imgBGR.rows, imgBGR.cols, CV_8UC3, cv::Scalar(0, 0, 0));
                //
                //                    for (int i = 0; i < visualRes.rows; i++)
                //                    { //draw all adjacent shadow pixels
                //                        for (int j = 0; j < visualRes.cols; j++)
                //                        {
                //
                //                            if (passedThroughPixels[i][j] == true)
                //                            {
                //                                cv::Vec3b & pixel = visualRes.at<cv::Vec3b>(i, j);
                //                                pixel.val[2] = 200;
                //                            }
                //                        }
                //                    }
                //
                //                    char integer_string[32];
                //                    int integer = contour_i;
                //                    sprintf(integer_string, "%d", integer);
                //                    char other_string[64] = "visualRes"; // make sure you allocate enough space to append the other string
                //                    strcat(other_string, integer_string); // other_string now contains "Integer: 1234"
                //                    cv::imshow(other_string, visualRes);
                //                }

                #endregion

                #region Find near shadow edge pixels (relight separatelly from shadow core pixels)

                bool[,] countour_adjacent_shadow_pixels_indexes = new bool[rows, cols];
                int maxDistance = 2; //max distance from countour

                //Loop thorough countour pixels and determine adjacent. start from coutours
                for (int _distance = 0; _distance < maxDistance; _distance++)
                {
                    for (int k = 0; k < currentContour.Count; k++)
                    {
                        int j = currentContour[k].X;
                        int i = currentContour[k].Y;

                        if (i - _distance >= 0)
                        {
                            if (shadowMask[i - _distance, j].Intensity == 255)
                            {
                                countour_adjacent_shadow_pixels_indexes[i - _distance,j] = true;
                            }
                        }
                        if (i + _distance < rows)
                        {
                            if (shadowMask[i + _distance, j].Intensity == 255)
                            {
                                countour_adjacent_shadow_pixels_indexes[i + _distance,j] = true;
                            }
                        }

                        if (j - _distance >= 0)
                        {
                            if (shadowMask[i, j - _distance].Intensity == 255)
                            {
                                countour_adjacent_shadow_pixels_indexes[i,j - _distance] = true;
                            }
                        }
                        if (j + _distance < cols)
                        {
                            if (shadowMask[i, j + _distance].Intensity == 255)
                            {
                                countour_adjacent_shadow_pixels_indexes[i,j + _distance] = true;
                            }
                        }
                    }
                }
                #endregion


                #region APPLYING THE CONSTANT

                //Apply to all pixels adajcent to border and all pixels adjacent to that pixels etc
                for (int i = 0; i < shadowMaskEdge.Rows; i++)
                {
                    for (int j = 0; j < shadowMaskEdge.Cols; j++)
                    {
                        //relight only pixels for cuurent countour
                        if (passedThroughPixels[i,j] != true)
                        {
                            continue;
                        }

                        //do not relight pixels that already lightened
                        if (lightenedPixels[i,j] == true)
                        {
                            continue;
                        }

                        //mark pixels as lightened to avoid double relight
                        lightenedPixels[i,j] = true;

//                        cv::Vec3b & pixel = imgBGRRes.at<cv::Vec3b>(i, j);
//                        cv::Vec3b & shadow_mask_pixel = imgShadowMask.at<cv::Vec3b>(i, j);

                        Bgr pixel = result[i, j];
                        Gray shadow_mask_pixel = shadowMask[i, j];

                        if (shadow_mask_pixel.Intensity == 255)
                        {

                            double B = pixel.Blue;
                            double G = pixel.Green;
                            double R = pixel.Red;

                            B += c_res_B;
                            G += c_res_G;
                            R += c_res_R;

                            B = B > 255 ? 255 : (B < 0 ? 0 : B);
                            G = G > 255 ? 255 : (G < 0 ? 0 : G);
                            R = R > 255 ? 255 : (R < 0 ? 0 : R);

//                            pixel.Blue = B;
//                            pixel.Green = G;
//                            pixel.Red = R;

                            result[i, j] = new Bgr(B, G, R);
                        }
                    }
                }
                #endregion
            }

            if (_params.ShowWindows)
            {
                EmguCvWindowManager.Display(shadowMaskEdge, "1 shadowMaskEdge");
                EmguCvWindowManager.Display(shadowMaskEdgeDilated, "2 shadowMaskEdgeDilated");
            }

            return result;
        }

        private void GoThroughAllAdjacentShadowPixelsRec(List<Point> currentContour, ref bool[,] passedThroughPixels, Image<Gray, Byte> shadowMask)
        {
            Image<Gray, Byte> imgShadowMaskCopy = shadowMask.Clone();

            //mark countour pixels as shadow to avoid errors in shadow pixels search
            for (int k = 0; k < currentContour.Count; k++)
            {
                int j2 = currentContour[k].X;
                int i2 = currentContour[k].Y;

//                Gray pixel = imgShadowMaskCopy[i2, j2];
                imgShadowMaskCopy[i2, j2] = new Gray(255);
            }

            //Take any shadow countour pixel as init value;
            Point startPixel = currentContour[0];
            for (int k = 0; k < currentContour.Count; k++)
            {
                int j = currentContour[k].X;
                int i = currentContour[k].Y;
                if (shadowMask[i, j].Intensity == 255)
                {
                    startPixel = currentContour[k];
                    break;
                }
            }

            int rows = imgShadowMaskCopy.Rows;
            int cols = imgShadowMaskCopy.Cols;

            int gridPixelsSize = imgShadowMaskCopy.Rows * imgShadowMaskCopy.Cols;
            int newGridPixelsSize = gridPixelsSize;

            int actualGridPixelsSize = 0;
            int actualNewGridPixelsSize = 0;

            //            cv::Point* gridPixels = new cv::Point[gridPixelsSize];
            //            cv::Point* newGridPixels = new cv::Point[newGridPixelsSize];

            Point[] gridPixels = new Point[gridPixelsSize];
            Point[] newGridPixels = new Point[newGridPixelsSize];
            bool[,] handledGridPixels = new bool[rows, cols];

            gridPixels[0] = startPixel;
            actualGridPixelsSize += 1;

            //main loop
            for (int i = 0; true; i++)
            {
                for (int k = 0; k < actualGridPixelsSize; k++)
                {
                    int j2 = gridPixels[k].X;
                    int i2 = gridPixels[k].Y;

                    passedThroughPixels[i2,j2] = true;

                    //look for adjacent shadow pixels
                    int disctance = 1;

                    //Go up, down, left, right
                    //up
                    if (i2 - disctance >= 0 && imgShadowMaskCopy[i2 - disctance, j2].Intensity == 255)
                    {
                        passedThroughPixels[i2 - disctance,j2] = true;
                        if (handledGridPixels[i2 - disctance,j2] == false)
                        {
                            newGridPixels[actualNewGridPixelsSize] = new Point(j2, i2 - disctance);
                            handledGridPixels[i2 - disctance,j2] = true;
                            actualNewGridPixelsSize += 1;
                        }

                    }
                    //down
                    if (i2 + disctance < imgShadowMaskCopy.Rows && imgShadowMaskCopy[i2 + disctance, j2].Intensity == 255)
                    {
                        passedThroughPixels[i2 + disctance,j2] = true;
                        if (handledGridPixels[i2 + disctance,j2] == false)
                        {
                            newGridPixels[actualNewGridPixelsSize] = new Point(j2, i2 + disctance);
                            handledGridPixels[i2 + disctance,j2] = true;
                            actualNewGridPixelsSize += 1;
                        }

                    }
                    //left
                    if (j2 - disctance >= 0 && imgShadowMaskCopy[i2, j2 - disctance].Intensity == 255)
                    {
                        passedThroughPixels[i2,j2 - disctance] = true;
                        if (handledGridPixels[i2,j2 - disctance] == false)
                        {
                            newGridPixels[actualNewGridPixelsSize] = new Point(j2 - disctance, i2);
                            handledGridPixels[i2,j2 - disctance] = true;
                            actualNewGridPixelsSize += 1;
                        }

                    }
                    //rigth
                    if (j2 + disctance < imgShadowMaskCopy.Cols && imgShadowMaskCopy[i2, j2 + disctance].Intensity == 255)
                    {
                        passedThroughPixels[i2,j2 + disctance] = true;
                        if (handledGridPixels[i2,j2 + disctance] == false)
                        {
                            newGridPixels[actualNewGridPixelsSize] = new Point(j2 + disctance, i2);
                            handledGridPixels[i2,j2 + disctance] = true;
                            actualNewGridPixelsSize += 1;
                        }

                    }

                }

//                delete gridPixels;
                gridPixels = newGridPixels;
                actualGridPixelsSize = actualNewGridPixelsSize;

                actualNewGridPixelsSize = 0;
                newGridPixels = new Point[newGridPixelsSize];

                //Reset2dArray(handledGridPixels,rows,cols,false);

                if (actualGridPixelsSize == 0)
                {
                    break;
                }
            }
        }


        //Shadow Detection and Compensation in Aerial Images using MATLAB
        //http://citeseerx.ist.psu.edu/viewdoc/download?doi=10.1.1.695.1720&rep=rep1&type=pdf
        public Image<Emgu.CV.Structure.Gray, Byte> DetectUsingModifiedRatioOfHueOverIntensityMethod(Image<Bgr, Byte> image)
        {
            Image<Gray, Byte> shadowMask = new Image<Gray, byte>(new Size(image.Width, image.Height));

            //Check params
            //if (_params.Threshold == null)
            //    _params.Threshold = 250;


            //To HSI
            //Todo - не уверен что HSI и HLS одно и то же
            Image<Hls, Byte> HsiImage = ImageHelper.ToHLS(image);

            //I_e [0, 255]
            //H_e [0, 255]
            Image<Gray, Byte> I_e = new Image<Gray, byte>(new Size(image.Width, image.Height)); ;
            Image<Gray, Byte> H_e = new Image<Gray, byte>(new Size(image.Width, image.Height)); ;

            for (int i = 0; i < image.Rows; i += 1)
            {
                for (int j = 0; j < image.Cols; j += 1)
                {
                    var RgbImagePixel = image[i, j];
                    double B = RgbImagePixel.Blue;
                    double G = RgbImagePixel.Green;
                    double R = RgbImagePixel.Red;

                    var HsiImagePixel = HsiImage[i, j];
                    double I = HsiImagePixel.MCvScalar.V0;
                    double V1 = HsiImagePixel.MCvScalar.V1;
                    double V2 = HsiImagePixel.MCvScalar.V2;

                    double I_e_ = (1.0 / 3.0) * B + (1.0 / 3.0) * G + (1.0 / 3.0) * R;
                    double H_e_ = (Math.Pow(Math.Tan(V1 / V2), -1) + Math.PI) * 255 / (2 * Math.PI);

                    I_e[i, j] = new Gray(I_e_);
                    H_e[i, j] = new Gray(H_e_);
                }
            }

            //Calc R1, B1, B3
            Image<Gray, Byte> R1 = new Image<Gray, byte>(new Size(image.Width, image.Height));
            Image<Gray, Byte> B1 = new Image<Gray, byte>(new Size(image.Width, image.Height));
            Image<Gray, Byte> B2 = new Image<Gray, byte>(new Size(image.Width, image.Height));
            for (int i = 0; i < image.Rows; i += 1)
            {
                for (int j = 0; j < image.Cols; j += 1)
                {
                    var RgbImagePixel = image[i, j];
                    double B = RgbImagePixel.Blue;
                    double G = RgbImagePixel.Green;
                    double R = RgbImagePixel.Red;

                    var HsiImagePixel = HsiImage[i, j];
                    double I = HsiImagePixel.MCvScalar.V0;
                    double V1 = HsiImagePixel.MCvScalar.V1;
                    double V2 = HsiImagePixel.MCvScalar.V2;

                    var I_e_pixel = I_e[i, j];
                    var H_e_pixel = H_e[i, j];

                    double R1_ = (double)(H_e_pixel.Intensity + 1.0) / (double)(I_e_pixel.Intensity + 0.1);
                    double B1_ = Math.Pow(0.5, R1_);
                    double B2_ = Math.Pow(0.7, R1_);

                    R1[i, j] = new Gray(R1_); // * 255
                    B1[i, j] = new Gray(B1_);
                    B2[i, j] = new Gray(B2_);
                }
            }

            //Merge R1, B1, B3 in RGB image
            Image<Bgr, Byte> mergedBgr = new Image<Bgr, byte>(new Size(image.Width, image.Height));
            for (int i = 0; i < image.Rows; i += 1)
            {
                for (int j = 0; j < image.Cols; j += 1)
                {
                    double R1_ = R1[i,j].Intensity;
                    double B1_ = B1[i,j].Intensity;
                    double B2_ = B2[i,j].Intensity;

                    mergedBgr[i, j] = new Bgr(B2_, B1_, R1_);
                }
            }

            //if (_params.ShowWindows)
            //{
            //}

            EmguCvWindowManager.Display(HsiImage, "HsiImage");
            EmguCvWindowManager.Display(I_e, "I_e");
            EmguCvWindowManager.Display(H_e, "H_e");
            EmguCvWindowManager.Display(R1, "R1");
            EmguCvWindowManager.Display(B1, "B1");
            EmguCvWindowManager.Display(B2, "B2");
            EmguCvWindowManager.Display(mergedBgr, "mergedBgr");

            return shadowMask;
        }



        public Image<Emgu.CV.Structure.Bgr, Byte> InpaintShadowEdges(Image<Bgr, Byte> image, Image<Gray, Byte> shadowMask)
        {
            int INPAINT_DILATION_KERNEL_SIZE = 3;
            double INPAINT_RADIUS = 4;

            Image<Bgr, Byte> result = image.Clone();
            Image<Gray, Byte> imgEdge = shadowMask.Clone();
            Image<Gray, Byte> imgEdgeDilatedForInpaint = shadowMask.Clone();
            Image<Bgr, Byte> imgBGRInpainted = image.Clone();

            CvInvoke.Canny(shadowMask, imgEdge, 50, 150, 3);

            //TODO //Load inpaint params from UI
            int inpaintDilationKernelSize = INPAINT_DILATION_KERNEL_SIZE;
            double inpaintRadius = INPAINT_RADIUS;

            Mat elementD = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(inpaintDilationKernelSize, inpaintDilationKernelSize), new Point(-1, -1));
            CvInvoke.Dilate(imgEdge, imgEdgeDilatedForInpaint, elementD, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());

            //Inpaint edge artifacts
            //int inpaintinMethod = CV_INPAINT_NS; //Navier-Stokes based method.
            InpaintType inpaintinMethod = InpaintType.Telea; //Method by Alexandru Telea
            CvInvoke.Inpaint(image, imgEdgeDilatedForInpaint, imgBGRInpainted, inpaintRadius, inpaintinMethod);

            return imgBGRInpainted;
        }

        public Image<Emgu.CV.Structure.Bgr, Byte> SmoothShadowEdgesUsingGaussianFilter(Image<Bgr, Byte> image,Image<Gray, Byte> shadowMask)
        {
            int GAUSSIAN_FILTER_DILATION_KERNEL_SIZE = 3;
            int GAUSSIAN_FILTER_KERNEL_SIZE = 3;


            Image<Bgr, Byte> result = image.Clone();
            Image<Gray, Byte> imgEdge = shadowMask.Clone();
            Image<Gray, Byte> imgEdgeDilated = shadowMask.Clone();
            Image<Bgr, Byte> imgEdgeGaussianWhole = image.Clone();
            Image<Bgr, Byte> imgBGRResSource = image.Clone();
            Image<Bgr, Byte> imgEdgeGaussianEdgeOnly = image.Clone();

            CvInvoke.Canny(shadowMask, imgEdge, 50, 150, 3);

            //TODO //Load inpaint params from UI
            int diationKernelSize = GAUSSIAN_FILTER_DILATION_KERNEL_SIZE;
            int kernelSize = GAUSSIAN_FILTER_KERNEL_SIZE;

            Mat elementD = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(diationKernelSize, diationKernelSize), new Point(-1, -1));
            CvInvoke.Dilate(imgEdge, imgEdgeDilated, elementD, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());

            CvInvoke.GaussianBlur(imgBGRResSource, imgEdgeGaussianWhole, new Size(kernelSize, kernelSize), 0);
            imgEdgeGaussianWhole.Copy(imgEdgeGaussianEdgeOnly, imgEdgeDilated);

            //if ( true)
            //{
            //    EmguCvWindowManager.Display(imgEdgeGaussianWhole, "1 imgEdgeGaussianWhole");
            //    EmguCvWindowManager.Display(imgEdgeGaussianEdgeOnly, "2 imgEdgeGaussianEdgeOnly");
            //}

            return imgEdgeGaussianEdgeOnly;
        }

        public Image<Emgu.CV.Structure.Bgr, Byte> SmoothShadowEdgesUsingMedianFilter(Image<Bgr, Byte> image, Image<Gray, Byte> shadowMask)
        {
            int MEDIAN_FILTER_DILATION_KERNEL_SIZE = 3;
            int MEDIAN_FILTER_KERNEL_SIZE = 3;

            Image<Bgr, Byte> result = image.Clone();
            Image<Gray, Byte> imgEdge = shadowMask.Clone();
            Image<Gray, Byte> imgEdgeDilated = shadowMask.Clone();
            Image<Bgr, Byte> imgMedianEdgeOnly = image.Clone();
            Image<Bgr, Byte> imgMedianWhole = image.Clone();

            CvInvoke.Canny(shadowMask, imgEdge, 50, 150, 3);

            //TODO //Load inpaint params from UI
            int diationKernelSize = MEDIAN_FILTER_DILATION_KERNEL_SIZE;
            int kernelSize = MEDIAN_FILTER_KERNEL_SIZE;

            Mat elementD = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new Size(diationKernelSize, diationKernelSize), new Point(-1, -1));
            CvInvoke.Dilate(imgEdge, imgEdgeDilated, elementD, new Point(-1, -1), 1, BorderType.Default, new MCvScalar());

            CvInvoke.MedianBlur(image, imgMedianWhole, kernelSize);
            imgMedianWhole.Copy(imgMedianEdgeOnly, imgEdgeDilated);

            //if (true)
            //{
            //    EmguCvWindowManager.Display(imgMedianWhole, "1 imgMedianWhole");
            //    EmguCvWindowManager.Display(imgMedianEdgeOnly, "2 imgMedianEdgeOnly");
            //}

            return imgMedianEdgeOnly;
        }
    }
}