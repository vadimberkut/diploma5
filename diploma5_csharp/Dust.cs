﻿using diploma5_csharp.Helpers;
using diploma5_csharp.Models;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diploma5_csharp
{
   public  class Dust
    {
        // Source: http://www.mecs-press.org/ijisa/ijisa-v8-n8/IJISA-V8-N8-2.pdf
        public BaseMethodResponse VisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod(
            Image<Bgr, Byte> image, 
            TriThresholdFuzzyIntensificationOperatorsMethodParams _params
        )
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Image<Bgr, Byte> result = new Image<Bgr, byte>(image.Size);

            //zeta (ζ), which is a tuning parameter that is used to control colors fidelity of the processed image
            double zeta = _params.Dzeta ?? 0.5;

            // tau (τ) - represents the thresholding limits of the operators
            double tao_R = 0.5;
            double tao_G = 0.6;
            double tao_B = 0.4;

            //membership function is required because it sets the pixels’ values of a given channel to the default range between zero and one
            //f_C = (C - min(C))/(max(C)-min(C)); C є {R, G, B
            double[] minValues;
            double[] maxValues;
            Point[] minLocations;
            Point[] maxLocations;
            image.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);

            for (int m = 0; m < image.Rows; m++)
            {
                for (int n = 0; n < image.Cols; n++)
                {
                    Bgr pixel = image[m, n];

                    //membership function
                    double f_B = MembershipFunction(pixel.Blue, minValues[0], maxValues[0]);
                    double f_G = MembershipFunction(pixel.Green, minValues[1], maxValues[1]);
                    double f_R = MembershipFunction(pixel.Red, minValues[2], maxValues[2]);

                    //intensification operator
                    double k_B = IntensificationOperator(f_B, tao_B);
                    double k_G = IntensificationOperator(f_G, tao_G);
                    double k_R = IntensificationOperator(f_R, tao_R);

                    //tune intensification outcome
                    double u_B = Math.Pow(k_B, tao_B + zeta);
                    double u_G = Math.Pow(k_G, tao_G + zeta);
                    double u_R = Math.Pow(k_R, tao_R + zeta);

                    double B = u_B * (maxValues[0] - minValues[0]) + minValues[0];
                    double G = u_G * (maxValues[1] - minValues[1]) + minValues[1];
                    double R = u_R * (maxValues[2] - minValues[2]) + minValues[2];

                    double B2 = u_B * 255;
                    double G2 = u_G * 255;
                    double R2 = u_R * 255;

                    result[m, n] = new Bgr(B2, G2, R2);
                }
            }

            stopwatch.Stop();

            return new BaseMethodResponse
            {
                EnhancementResult = result,
                DetectionResult = new Image<Gray, byte>(image.Size),
                DetailedResults = new List<IInputArray> { image, result },
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }
        private double MembershipFunction(double C, double minC, double maxC)
        {
            double f_C = (C - minC) / (maxC - minC);
            return f_C;
        }
        private double IntensificationOperator(double f_C, double tao_C)
        {
            if(f_C <= tao_C)
            {
                return 2 * Math.Pow(f_C, 2);
            }
            else
            {
                return 1 - 2 * (Math.Pow(1 - f_C, 2));
            }
        }

        // Universal method for dust, mist, fog
        // Source: http://colorimaginglab.ugr.es/pages/pdfs/ao_2015_B222/!
        public BaseMethodResponse RecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod(
            Image<Bgr, Byte> image, 
            RGBResponseRatioConstancyMethodParams _params
        )
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Image<Bgr, Byte> result = new Image<Bgr, byte>(image.Size);
            Image<Bgr, Byte> resultToAll = new Image<Bgr, byte>(image.Size);

            //Apply mean shift clustering
            MeanShiftClusteringAcordParams msParams = new MeanShiftClusteringAcordParams()
            {
                Kernel = _params?.MeanShiftParams?.Kernel ?? 5,
                Sigma = _params?.MeanShiftParams?.Sigma ?? 0.13
            };
            var msResult = Clustering.MeanShiftAccord(image, msParams);
            var labels = msResult.Labels.Distinct().ToArray(); //regions numbers

            //Find regions pixels coordinates
            var regionPixelsCoordinates = Clustering.GetRegionsPixelsCoordinates(msResult);

            //Loop through regions
            for (int region = 0; region < msResult.RegionCount; region++)
            {
                //Find min and max values for each channel
                var regionPixels = regionPixelsCoordinates[region].Select(coordinates => image[coordinates.X, coordinates.Y]).ToArray();

                var regionBValues = regionPixels.Select(p => p.Blue).ToArray();
                var regionGValues = regionPixels.Select(p => p.Green).ToArray();
                var regionRValues = regionPixels.Select(p => p.Red).ToArray();

                double[] minValues = new double[3] { regionBValues.Min(), regionGValues.Min(), regionRValues.Min() };
                double[] maxValues = new double[3] { regionBValues.Max(), regionGValues.Max(), regionRValues.Max() };

                int pixelsCountInRegion = regionPixelsCoordinates[region].Count;

                double B_min = minValues[0];
                double G_min = minValues[1];
                double R_min = minValues[2];

                double B_max = maxValues[0];
                double G_max = maxValues[1];
                double R_max = maxValues[2];

                if (_params.Imin != null)
                {
                    B_min = _params.Imin.Value;
                    G_min = _params.Imin.Value;
                    R_min = _params.Imin.Value;
                }
                if (_params.Imax != null)
                {
                    B_max = _params.Imax.Value;
                    G_max = _params.Imax.Value;
                    R_max = _params.Imax.Value;
                }

                //Apply formula
                for (int i = 0; i < pixelsCountInRegion; i++)
                {
                    var pixelCoordinates = regionPixelsCoordinates[region][i];
                    Bgr pixel = image[pixelCoordinates.X, pixelCoordinates.Y];

                    double B = (pixel.Blue - B_min) * (B_max / (B_max - B_min));
                    double G = (pixel.Green - G_min) * (G_max / (G_max - G_min));
                    double R = (pixel.Red - R_min) * (R_max / (R_max - R_min));

                    result[pixelCoordinates.X, pixelCoordinates.Y] = new Bgr(B, G, R);
                }
            }

            //apply for all image

            //Find min and max values for each channel
            double[] minValues2;
            double[] maxValues2;
            Point[] minLocations2;
            Point[] maxLocations2;
            image.MinMax(out minValues2, out maxValues2, out minLocations2, out maxLocations2);

            double B_min2 = minValues2[0];
            double G_min2 = minValues2[1];
            double R_min2 = minValues2[2];

            double B_max2 = maxValues2[0];
            double G_max2 = maxValues2[1];
            double R_max2 = maxValues2[2];

            if (_params.Imin != null)
            {
                B_min2 = _params.Imin.Value;
                G_min2 = _params.Imin.Value;
                R_min2 = _params.Imin.Value;
            }
            if (_params.Imax != null)
            {
                B_max2 = _params.Imax.Value;
                G_max2 = _params.Imax.Value;
                R_max2 = _params.Imax.Value;
            }

            for (int m = 0; m < image.Rows; m++)
            {
                for (int n = 0; n < image.Cols; n++)
                {
                    Bgr pixel = image[m, n];

                    double B = (pixel.Blue - B_min2) * (B_max2 / (B_max2 - B_min2));
                    double G = (pixel.Green - G_min2) * (G_max2 / (G_max2 - G_min2));
                    double R = (pixel.Red - R_min2) * (R_max2 / (R_max2 - R_min2));

                    resultToAll[m, n] = new Bgr(B, G, R);
                }
            }

            if (_params.ShowWindows)
            {
                EmguCvWindowManager.Display(image, "1. image");
                EmguCvWindowManager.Display(msResult.Image, "2. MS");
                EmguCvWindowManager.Display(resultToAll, "3. resultToAll");
                EmguCvWindowManager.Display(result, "4. result");
            }

            stopwatch.Stop();

            return new BaseMethodResponse
            {
                EnhancementResult = result,
                DetectionResult = new Image<Gray, byte>(image.Size),
                DetailedResults = new List<IInputArray> { image, msResult.Image, resultToAll, result },
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }
    }
}
