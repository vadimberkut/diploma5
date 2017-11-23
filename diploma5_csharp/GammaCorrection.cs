using diploma5_csharp.Helpers;
using diploma5_csharp.Models;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diploma5_csharp
{
    public static class GammaCorrection
    {
        // Source - https://jivp-eurasipjournals.springeropen.com/articles/10.1186/s13640-016-0138-1
        public static Image<Bgr, Byte> Adaptive(Image<Bgr, Byte> image, bool showWindows = false)
        {
            Image<Hsv, Byte> hsvImage;
            Image<Gray, Byte> grayImage;
            Image<Gray, double> grayImageNormilized;
            Image<Gray, Byte> grayImageEnhanced;
            Image<Gray, double> grayImageEnhancedNormilized;
            Image<Hsv, Byte> hsvImageEnhanced;
            Image<Bgr, Byte> result;

            // Color image?
            // FOR NOW CONSIDER ONLY COLOR IMAGES
            bool isColorImage = true;

            // RGB to HSV and take V
            hsvImage = ImageHelper.ToHsv(image);
            var hsvChannels = hsvImage.Split();
            grayImage = hsvChannels.ElementAt(2);

            // Normalize image
            grayImageNormilized = ImageHelper.NormalizeImage(grayImage);

            // Low contrast image?
            bool isLowContrastImage;
            bool isDarkImage;

            double[] pixelValues = ImageHelper.GetImagePixels(grayImageNormilized);
            double[] positivePixelValues = pixelValues.Where(x => x > 0).ToArray(); // filter 0 values to retrieve more accurate results (according to article)
            double mu = StatisticsHelper.Average(positivePixelValues); // mean of the image intensity
            double sigma = StatisticsHelper.StandartDeviation(positivePixelValues); // standard deviation

            Func<List<string>> g = () =>
            {
                string Q1 = "Q1"; // low-contrast class
                string Q2 = "Q2"; // high (or moderate) contrast class

                string QSubclassDark = "Dark";
                string QSubclassBright = "Bright";

                string Q;
                string QSubclass;

                // D=diff((μ+2σ),(μ−2σ))
                //double D = (mu + 2.0 * sigma) - (mu - 2.0 * sigma);
                double D = 4.0 * sigma; // seci=ond criteria

                //  τ is a parameter used for defining the contrast of an image
                double tao = 3.0; // τ=3 is a suitable choice for characterizing the contrasts of different images.

                if (D <= (1.0 / tao))
                    Q = Q1;
                else
                    Q = Q2;

                if (mu >= 0.5)
                    QSubclass = QSubclassBright;
                else
                    QSubclass = QSubclassDark;

                return new List<string>()
                {
                    Q,
                    QSubclass
                };
            };

            List<string> imageClasses = g();
            isLowContrastImage = imageClasses[0] == "Q1" ? true : false;
            isDarkImage = imageClasses[1] == "Dark" ? true : false;

            // Intensity transformation

            // c and γ are two parameters that control the shape of the transformation curve
            //  In contrast to traditional gamma correction, AGC sets the values of γ and c automatically using image information, making it an adaptive method
            double c = default(double);
            double gamma = default(double);

            Func<double, double> Heaviside = (x) =>
            {
                if (x <= 0)
                    return 0;
                else // (x > 0)
                    return 1;
            };

            grayImageEnhancedNormilized = new Image<Gray, double>(grayImage.Size);

            for (int m = 0; m < grayImageNormilized.Rows; m++)
            {
                for (int n = 0; n < grayImageNormilized.Cols; n++)
                {
                    Gray pixel = grayImageNormilized[m, n];
                    double I_in = pixel.Intensity;

                    //// Enhancement of low-contrast image
                    if (isLowContrastImage)
                    {
                        // calc gamma
                        gamma = -Math.Log(sigma, newBase: 2);

                        // calc k
                        double I_gamma = Math.Pow(I_in, gamma);
                        double k = I_gamma + (1 - I_gamma) * Math.Pow(mu, gamma);

                        // calc c
                        c = 1.0 / (1.0 + Heaviside(0.5 - mu) * (k - 1.0));

                        // Bright image in Q1
                        if (isDarkImage == false)
                        {
                            // mu  >= 0.5 => c = 1
                            //c = 1;
                        }
                        // Dark image in Q1
                        if (isDarkImage == true)
                        {
                            // mu < 0.5 => c = 1/k
                            //c = 1.0 / k;
                        }
                    }

                    ////  Enhancement of high- or moderate-contrast image
                    if (isLowContrastImage == false)
                    {
                        // calc gamma
                        gamma = Math.Exp((1.0 - (mu + sigma)) / 2.0);

                        // calc k
                        double I_gamma = Math.Pow(I_in, gamma);
                        double k = I_gamma + (1.0 - I_gamma) * Math.Pow(mu, gamma);

                        // calc c
                        c = 1.0 / (1.0 + Heaviside(0.5 - mu) * (k - 1.0));

                        // Dark image in Q2
                        if (isDarkImage == true)
                        {
                            // For images with μ<0.5, (μ+σ)≤1, since both μ and σ are less than (or equal to) 0.5 which implies γ≥1

                        }

                        // Bright image in ϱ 2
                        if (isDarkImage == false)
                        {

                        }
                    }

                    // apply final formula
                    // I_out = c * I_in
                    double val = c * Math.Pow(I_in, gamma);
                    grayImageEnhancedNormilized[m, n] = new Gray(val);
                }
            }

            // DeNormalize image
            grayImage = ImageHelper.DeNormalizeImage(grayImageNormilized);
            grayImageEnhanced = ImageHelper.DeNormalizeImage(grayImageEnhancedNormilized);

            // get enhanced hsv image
            hsvImageEnhanced = new Image<Hsv, byte>(new Image<Gray, Byte>[] { hsvChannels[0], hsvChannels[1], grayImageEnhanced });

            // convert hsv to rgb
            result = ImageHelper.ToBgr(hsvImageEnhanced);

            if (showWindows)
            {
                EmguCvWindowManager.Display(image, "AGC_V2_1 image");
                EmguCvWindowManager.Display(hsvImage, "AGC_V2_2 hsvImage");
                EmguCvWindowManager.Display(grayImage, "AGC_V2_3 grayImage");
                EmguCvWindowManager.Display(grayImageEnhanced, "AGC_V2_4 grayImageEnhanced");
                EmguCvWindowManager.Display(hsvImageEnhanced, "AGC_V2_5 hsvImageEnhanced");
                EmguCvWindowManager.Display(result, "AGC_V2_6 result");
            }

            return result;
        }

        public static BaseMethodResponse AdaptiveWithBaseResponse(Image<Bgr, Byte> image, bool showWindows = false)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var result = Adaptive(image, showWindows);

            stopwatch.Stop();

            var Metrics = ImageMetricHelper.ComputeAll(image, result);
            return new BaseMethodResponse
            {
                EnhancementResult = result,
                DetectionResult = new Image<Gray, byte>(image.Size),
                Metrics = Metrics,
                ExecutionTimeMs = stopwatch.ElapsedMilliseconds
            };
        }
    }
}
