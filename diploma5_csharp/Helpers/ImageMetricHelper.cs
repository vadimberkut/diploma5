using diploma5_csharp.DataEntropy;
using diploma5_csharp.Extensions;
using diploma5_csharp.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diploma5_csharp.Helpers
{
    // Metrics described here - http://research.ijcaonline.org/volume99/number10/pxc3897996.pdf
    public static class ImageMetricHelper
    {
        public const int DECIMALS = 4;

        #region Public members

        public static MetricsResult ComputeAll(Image<Bgr, double> image1, Image<Bgr, double> image2)
        {
            double MSE = ImageMetricHelper.MSE(image1, image2);
            double NAE = ImageMetricHelper.NAE(image1, image2);
            double SC = ImageMetricHelper.SC(image1, image2);
            double PSNR = ImageMetricHelper.PSNR(image1, image2);
            double AD = ImageMetricHelper.AD(image1, image2);
            double FVM = ImageMetricHelper.FVM(image1, image2);
            double RMSDiff = ImageMetricHelper.RMSDifference(image1, image2);
            double ShannonEntropyDiff = ImageMetricHelper.ShannonEntropyDiff(image1, image2);

            return new MetricsResult
            {
                AD = Math.Round(AD, DECIMALS),
                FVM = Math.Round(FVM, DECIMALS),
                MSE = Math.Round(MSE, DECIMALS),
                NAE = Math.Round(NAE, DECIMALS),
                SC = Math.Round(SC, DECIMALS),
                PSNR = Math.Round(PSNR, DECIMALS),
                RMSDiff = Math.Round(RMSDiff, DECIMALS),
                ShannonEntropyDiff = ShannonEntropyDiff
            };
        }

        public static MetricsResult ComputeAll(Image<Bgr, byte> image1, Image<Bgr, byte> image2)
        {
            var converted1 = image1.Convert<Bgr, double>();
            var converted2 = image2.Convert<Bgr, double>();
            var result = ComputeAll(converted1, converted2);

            // dispose resources
            converted1.Dispose();
            converted2.Dispose();

            return result;
        }

        //public static void AAA<TColor, TDepth>(Image<TColor, TDepth> image)
        //    where TColor : struct, IColor
        //    where TDepth : new()
        //{
        //}


        /// <summary>
        /// Signal noise ratio is used to quantify how much a signal has been corrupted by noise
        /// </summary>
        /// <returns></returns>
        //public static double SNR(Image<Gray, double> image)
        //{
        //    double result = 0;
        //    int M = image.Rows;
        //    int N = image.Cols;

        //    // 1. Calculate the P_signal as the mean of pixel values.
        //    double P_signal = 0;
        //    for (int m = 0; m < M; m++)
        //    {
        //        for (int n = 0; n < N; n++)
        //        {
        //            P_signal += image.Data[m, n, 0];
        //        }
        //    }
        //    P_signal = P_signal / (M * N);

        //    // 2. Calculate the P_noise and the standard deviation or error value of the pixel values.
        //    double P_noise = 0;

        //    // 3.
        //    result = 20 * Math.Log10(P_signal / P_noise);

        //    return result;
        //}



        /// <summary>
        /// Mean Square Error
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public static double MSE(Image<Gray, double> image1, Image<Gray, double> image2)
        {
            double result = 0;
            int M = image1.Rows;
            int N = image1.Cols;

            for (int m = 0; m < M; m++)
            {
                for (int n = 0; n < N; n++)
                {
                    result += Math.Pow(Math.Abs(image1.Data[m,n, 0] - image2.Data[m,n, 0]), 2); // |I-K|^2
                }
            }
            result = result * (1.0 / (M * N));
            return result;
        }

        /// <summary>
        /// Mean Square Error
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public static double MSE(Image<Bgr, double> image1, Image<Bgr, double> image2)
        {
            var channels1 = image1.Split();
            var channels2 = image2.Split();
            double result = 0;
            for (int i = 0; i < channels1.Count(); i++)
            {
                result += MSE(channels1[i], channels2[i]);
            }
            return result / channels1.Count();
        }





        /// <summary>
        /// Normalized Absolute Error (NAE) - The larger value of NAE means that image is of poor quality
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public static double NAE(Image<Gray, double> image1, Image<Gray, double> image2)
        {
            double sum = 0;
            double result = 0;
            int M = image1.Rows;
            int N = image1.Cols;

            // compute sum of first image
            for (int m = 0; m < M; m++)
            {
                for (int n = 0; n < N; n++)
                {
                    sum += Math.Abs(image1.Data[m, n, 0]); // abs - by formula
                }
            }

            double mse = MSE(image1, image2);
            result = mse / sum;
            return result;
        }

        /// <summary>
        /// Normalized Absolute Error (NAE) - The larger value of NAE means that image is of poor quality
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public static double NAE(Image<Bgr, double> image1, Image<Bgr, double> image2)
        {
            var channels1 = image1.Split();
            var channels2 = image2.Split();
            double result = 0;
            for (int i = 0; i < channels1.Count(); i++)
            {
                result += NAE(channels1[i], channels2[i]);
            }
            return result;
        }



        /// <summary>
        /// Structural Content (SC) - If it is spread at 1, then the
        /// decompressed image is of better quality and large value of
        /// SC means that the image is of poor quality
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public static double SC(Image<Gray, double> image1, Image<Gray, double> image2)
        {
            double result = 0;
            int M = image1.Rows;
            int N = image1.Cols;

            double sumQuadrtic1 = 0;
            double sumQuadrtic2 = 0;

            for (int m = 0; m < M; m++)
            {
                for (int n = 0; n < N; n++)
                {
                    sumQuadrtic1 += Math.Pow(image1.Data[m, n, 0], 2);
                }
            }
            for (int m = 0; m < M; m++)
            {
                for (int n = 0; n < N; n++)
                {
                    sumQuadrtic2 += Math.Pow(image2.Data[m, n, 0], 2);
                }
            }
            result = sumQuadrtic1 / sumQuadrtic2;
            return result;
        }

        /// <summary>
        /// Structural Content (SC) - If it is spread at 1, then the
        /// decompressed image is of better quality and large value of
        /// SC means that the image is of poor quality
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public static double SC(Image<Bgr, double> image1, Image<Bgr, double> image2)
        {
            var channels1 = image1.Split();
            var channels2 = image2.Split();
            double result = 0;
            for (int i = 0; i < channels1.Count(); i++)
            {
                result += SC(channels1[i], channels2[i]);
            }
            return result;
        }



        // PSNR - https://en.wikipedia.org/wiki/Peak_signal-to-noise_ratio

        /// <summary>
        /// Peak signal-to-noise ratio (PSNR) -  term for the ratio between the maximum possible power of a signal and the power of corrupting noise that affects 
        /// the fidelity of its representation. Because many signals have a very wide dynamic range, PSNR is usually expressed in terms of the logarithmic decibel scale.
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public static double PSNR(Image<Gray, double> image1, Image<Gray, double> image2)
        {
            double result = 0;
            int M = image1.Rows;
            int N = image1.Cols;

            // find MAXI is the maximum possible pixel value of the image1
            double[] minValues, maxValues;
            Point[] minLocations, maxLocations;
            image1.MinMax(out minValues, out maxValues, out minLocations, out maxLocations);
            double MAX = maxValues[0]; // because GRAY

            // compute MSE
            double mse = MSE(image1, image2);
            
            result = 20 * Math.Log10(MAX) - 10 * Math.Log10(mse);
            return result;
        }

        /// <summary>
        /// Peak signal-to-noise ratio (PNSR) -  term for the ratio between the maximum possible power of a signal and the power of corrupting noise that affects 
        /// the fidelity of its representation. Because many signals have a very wide dynamic range, PSNR is usually expressed in terms of the logarithmic decibel scale.
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public static double PSNR(Image<Bgr, double> image1, Image<Bgr, double> image2)
        {
            var channels1 = image1.Split();
            var channels2 = image2.Split();
            double result = 0;
            for (int i = 0; i < channels1.Count(); i++)
            {
                result += PSNR(channels1[i], channels2[i]);
            }
            return result;
        }



        // SSIM - https://ru.wikipedia.org/wiki/SSIM
        // -1 <= SSIM <= 1
        // usually calculated for NxN windows x and y

        /// <summary>
        /// Structural Similarity Index (SSIM) for measuring image quality. If SSIM = 1 we have equal images
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public static double SSIM(Image<Gray, double> image1, Image<Gray, double> image2)
        {
            double result = 0;
            int M = image1.Rows;
            int N = image1.Cols;

            // average
            double mu_x = 0;
            double mu_y = 0;

            // variance
            double sigma_x = 0;
            double sigma_y = 0;

            // covariance
            double sigma_xy = 0;

            double k1 = 0.01;
            double k2 = 0.03;
            double L = Math.Pow(2, sizeof(byte)) - 1; // dynamic pixel range 2^(bits per pixel) - 1
            double c1 = Math.Pow(k1 * L, 2);
            double c2 = Math.Pow(k2 * L, 2);


            throw new NotImplementedException();

            return result;
        }

        /// <summary>
        /// Structural Similarity Index (SSIM) for measuring image quality. If SSIM = 1 we have equal images
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public static double SSIM(Image<Bgr, double> image1, Image<Bgr, double> image2)
        {
            throw new NotImplementedException();

            var channels1 = image1.Split();
            var channels2 = image2.Split();
            double result = 0;
            for (int i = 0; i < channels1.Count(); i++)
            {
                result += SSIM(channels1[i], channels2[i]);
            }
            return result;
        }

        /// <summary>
        /// Average Difference (AD)
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public static double AD(Image<Gray, double> image1, Image<Gray, double> image2)
        {
            double result = 0;
            int M = image1.Rows;
            int N = image1.Cols;

            var absDiff = image1.AbsDiff(image2);

            for (int m = 0; m < M; m++)
            {
                for (int n = 0; n < N; n++)
                {
                    result += absDiff.Data[m, n, 0]; // |I-K|^2
                }
            }
            result = result * (1.0 / (M * N));
            return result;
        }

        /// <summary>
        /// Average Difference (AD)
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public static double AD(Image<Bgr, double> image1, Image<Bgr, double> image2)
        {
            var channels1 = image1.Split();
            var channels2 = image2.Split();
            double result = 0;
            for (int i = 0; i < channels1.Count(); i++)
            {
                result += AD(channels1[i], channels2[i]);
            }
            return result / channels1.Count();
        }


        // Source (Matlab code) - https://www.mathworks.com/matlabcentral/fileexchange/33529-a-new-visibility-metric-for-haze-images
        /// <summary>
        /// Fog Visibility Metric (FVM) - A New Visibility Metric For Haze Images
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static double FVM(Image<Bgr, Byte> image)
        {
            // convert rgb to double Gray
            Image<Gray, Byte> gray = new Image<Gray, Byte>(image.Size);
            Image<Gray, double> gray_double = new Image<Gray, double>(image.Size);
            CvInvoke.CvtColor(image, gray, ColorConversion.Bgr2Gray); // can't be double
            gray_double = gray.Convert<Gray, double>();

            double epsilon = 1e-1;
            double sigma1 = 5;
            double sigma2 = 5;

            // norminv - Normal inverse cumulative distribution
            double norminv1 = QNorm(epsilon / 2, 0, sigma1, false, false);
            double norminv2 = QNorm(epsilon / 2, 0, sigma2, false, false);
            double half_size1 = Math.Ceiling(-norminv1);
            double half_size2 = Math.Ceiling(-norminv2);
            double size1 = 2 * half_size1 + 1;
            double size2 = 2 * half_size2 + 1;

            // I THINK size SHOULD BE POSITIVE
            size1 = Math.Abs(size1);
            size2 = Math.Abs(size2);

            var IM = gray_double;
            var IM_smoothed = IM.SmoothGaussian((int)size1, (int)size1, sigma1, sigma1);
            var IM_abs_diff = IM.AbsDiff(IM_smoothed);
            var IM_abs_diff_smoothed = IM_abs_diff.SmoothGaussian((int)size1, (int)size1, sigma1, sigma1);
            var noise = IM_abs_diff - IM_abs_diff_smoothed;

            var noise_sq = noise.Pow(2);
            var noise_sq_smoothed = noise_sq.SmoothGaussian((int)size2, (int)size2, sigma2, sigma2);
            var noise_sq_smoothed_sqrt = noise_sq_smoothed.Pow(1.0 / 2.0); // sqrt
            var lsd = noise_sq_smoothed_sqrt;
            var lsd_max = ImageHelper.Max(lsd);
            var lsd_normilized = (lsd / lsd_max) * 255.0; // normalize
            var lsd_normilized_sum = ImageHelper.Sum(lsd_normilized);

            double CNR_ = (lsd_normilized_sum / lsd_normilized.Rows) * lsd_normilized.Cols;

            return CNR_;
        }

        /// <summary>
        /// Computes difference between FVM for image1 and image2. Positive means quality impovement of image2, negative - quality degradation
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public static double FVM(Image<Bgr, double> image1, Image<Bgr, double> image2)
        {
            double result = FVM(image2.Convert<Bgr, Byte>()) - FVM(image1.Convert<Bgr, Byte>());
            return result;
        }

        /// <summary>
        /// Quantile function (Inverse CDF) for the normal distribution.
        /// </summary>
        /// <param name="p">Probability.</param>
        /// <param name="mu">Mean of normal distribution.</param>
        /// <param name="sigma">Standard deviation of normal distribution.</param>
        /// <param name="lower_tail">If true, probability is P[X <= x], otherwise P[X > x].</param>
        /// <param name="log_p">If true, probabilities are given as log(p).</param>
        /// <returns>P[X <= x] where x ~ N(mu,sigma^2)</returns>
        /// <remarks>See https://svn.r-project.org/R/trunk/src/nmath/qnorm.c</remarks>
        public static double QNorm(double p, double mu, double sigma, bool lower_tail, bool log_p)
        {
            if (double.IsNaN(p) || double.IsNaN(mu) || double.IsNaN(sigma)) return (p + mu + sigma);
            double ans;
            bool isBoundaryCase = R_Q_P01_boundaries(p, double.NegativeInfinity, double.PositiveInfinity, lower_tail, log_p, out ans);
            if (isBoundaryCase) return (ans);
            if (sigma < 0) return (double.NaN);
            if (sigma == 0) return (mu);

            double p_ = R_DT_qIv(p, lower_tail, log_p);
            double q = p_ - 0.5;
            double r, val;

            if (Math.Abs(q) <= 0.425)  // 0.075 <= p <= 0.925
            {
                r = .180625 - q * q;
                val = q * (((((((r * 2509.0809287301226727 +
                           33430.575583588128105) * r + 67265.770927008700853) * r +
                         45921.953931549871457) * r + 13731.693765509461125) * r +
                       1971.5909503065514427) * r + 133.14166789178437745) * r +
                     3.387132872796366608)
                / (((((((r * 5226.495278852854561 +
                         28729.085735721942674) * r + 39307.89580009271061) * r +
                       21213.794301586595867) * r + 5394.1960214247511077) * r +
                     687.1870074920579083) * r + 42.313330701600911252) * r + 1.0);
            }
            else
            {
                r = q > 0 ? R_DT_CIv(p, lower_tail, log_p) : p_;
                r = Math.Sqrt(-((log_p && ((lower_tail && q <= 0) || (!lower_tail && q > 0))) ? p : Math.Log(r)));

                if (r <= 5)              // <==> min(p,1-p) >= exp(-25) ~= 1.3888e-11
                {
                    r -= 1.6;
                    val = (((((((r * 7.7454501427834140764e-4 +
                            .0227238449892691845833) * r + .24178072517745061177) *
                          r + 1.27045825245236838258) * r +
                         3.64784832476320460504) * r + 5.7694972214606914055) *
                       r + 4.6303378461565452959) * r +
                      1.42343711074968357734)
                     / (((((((r *
                              1.05075007164441684324e-9 + 5.475938084995344946e-4) *
                             r + .0151986665636164571966) * r +
                            .14810397642748007459) * r + .68976733498510000455) *
                          r + 1.6763848301838038494) * r +
                         2.05319162663775882187) * r + 1.0);
                }
                else                     // very close to  0 or 1 
                {
                    r -= 5.0;
                    val = (((((((r * 2.01033439929228813265e-7 +
                            2.71155556874348757815e-5) * r +
                           .0012426609473880784386) * r + .026532189526576123093) *
                         r + .29656057182850489123) * r +
                        1.7848265399172913358) * r + 5.4637849111641143699) *
                      r + 6.6579046435011037772)
                     / (((((((r *
                              2.04426310338993978564e-15 + 1.4215117583164458887e-7) *
                             r + 1.8463183175100546818e-5) * r +
                            7.868691311456132591e-4) * r + .0148753612908506148525)
                          * r + .13692988092273580531) * r +
                         .59983220655588793769) * r + 1.0);
                }
                if (q < 0.0) val = -val;
            }

            return (mu + sigma * val);
        }

        private static bool R_Q_P01_boundaries(double p, double _LEFT_, double _RIGHT_, bool lower_tail, bool log_p, out double ans)
        {
            if (log_p)
            {
                if (p > 0.0)
                {
                    ans = double.NaN;
                    return (true);
                }
                if (p == 0.0)
                {
                    ans = lower_tail ? _RIGHT_ : _LEFT_;
                    return (true);
                }
                if (p == double.NegativeInfinity)
                {
                    ans = lower_tail ? _LEFT_ : _RIGHT_;
                    return (true);
                }
            }
            else
            {
                if (p < 0.0 || p > 1.0)
                {
                    ans = double.NaN;
                    return (true);
                }
                if (p == 0.0)
                {
                    ans = lower_tail ? _LEFT_ : _RIGHT_;
                    return (true);
                }
                if (p == 1.0)
                {
                    ans = lower_tail ? _RIGHT_ : _LEFT_;
                    return (true);
                }
            }
            ans = double.NaN;
            return (false);
        }

        private static double R_DT_qIv(double p, bool lower_tail, bool log_p)
        {
            return (log_p ? (lower_tail ? Math.Exp(p) : -ExpM1(p)) : R_D_Lval(p, lower_tail));
        }

        private static double R_DT_CIv(double p, bool lower_tail, bool log_p)
        {
            return (log_p ? (lower_tail ? -ExpM1(p) : Math.Exp(p)) : R_D_Cval(p, lower_tail));
        }

        private static double R_D_Lval(double p, bool lower_tail)
        {
            return lower_tail ? p : 0.5 - p + 0.5;
        }

        private static double R_D_Cval(double p, bool lower_tail)
        {
            return lower_tail ? 0.5 - p + 0.5 : p;
        }
        private static double ExpM1(double x)
        {
            if (Math.Abs(x) < 1e-5)
                return x + 0.5 * x * x;
            else
                return Math.Exp(x) - 1.0;
        }




        // TODO - RMSE, BER





        /// <summary>
        /// Root-mean-square (RMS) contrast allows to define the contrast of an image
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static double RMS(Image<Gray, double> image)
        {
            double result = 0;
            int M = image.Rows;
            int N = image.Cols;

            double[] pixelValues = ImageHelper.GetImagePixels(image);
            double[] positivePixelValues = pixelValues.Where(x => x > 0).ToArray(); // filter 0 values to retrieve more accurate results (according to article)
            double mu = StatisticsHelper.Average(positivePixelValues); // mean of the image intensity

            for (int m = 0; m < M; m++)
            {
                for (int n = 0; n < N; n++)
                {
                    result += Math.Pow(mu - image.Data[m, n, 0], 2); // |I-K|^2
                }
            }
            result = result * (1.0 / (M * N));
            result = Math.Sqrt(result);
            return result;
        }

        /// <summary>
        /// Root-mean-square (RMS) contrast allows to define the contrast of an image
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static double RMS(Image<Bgr, double> image)
        {
            var channels = image.Split();
            double result = 0;
            for (int i = 0; i < channels.Count(); i++)
            {
                result += RMS(channels[i]);
            }
            return result / channels.Count();
        }

        /// <summary>
        /// Root-mean-square (RMS) contrast allows to define the contrast of an image. Computes difference between RMS of two images.
        /// Positive value means enchacement of contrast in image2 comparing to image1. Negative - degradation in contrast.
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public static double RMSDifference(Image<Gray, double> image1, Image<Gray, double> image2)
        {
            double rms1 = RMS(image1);
            double rms2 = RMS(image2);
            double result = rms2 - rms1;
            return result;
        }

        /// <summary>
        /// Root-mean-square (RMS) contrast allows to define the contrast of an image. Computes difference between RMS of two images.
        /// Positive value means enchacement of contrast in image2 comparing to image1. Negative - degradation in contrast.
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public static double RMSDifference(Image<Bgr, double> image1, Image<Bgr, double> image2)
        {
            double rms1 = RMS(image1);
            double rms2 = RMS(image2);
            double result = rms2 - rms1;
            return result;
        }







        /// <summary>
        /// Calculates Shannon entropy for image1 and image2. Retuns entropy defference of image2 and image1
        /// Positive value means enchacement (of contrast, visibility etc) in image2 comparing to image1. Negative - degradation.
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public static double ShannonEntropyDiff(Image<Gray, double> image1, Image<Gray, double> image2)
        {
            var SE1 = new DataEntropyUTF8(image1);
            var SE2 = new DataEntropyUTF8(image2);
            double result = SE2.Entropy - SE1.Entropy;
            return result;
        }

        /// <summary>
        /// Calculates Shannon entropy for image1 and image2. Retuns entropy defference of image2 and image1
        /// Positive value means enchacement (of contrast, visibility etc) in image2 comparing to image1. Negative - degradation.
        /// </summary>
        /// <param name="image1"></param>
        /// <param name="image2"></param>
        /// <returns></returns>
        public static double ShannonEntropyDiff(Image<Bgr, double> image1, Image<Bgr, double> image2)
        {
            var SE1 = new DataEntropyUTF8(image1);
            var SE2 = new DataEntropyUTF8(image2);
            double result = SE2.Entropy - SE1.Entropy;
            return result;
        }


        #endregion

        #region Private members

        private static void CheckSameType(Mat mat1, Mat mat2)
        {
            if(
                (mat1.Rows != mat2.Rows) || 
                (mat1.Cols != mat2.Cols) ||
                (mat1.Depth != mat2.Depth) ||
                (mat1.NumberOfChannels != mat2.NumberOfChannels)
                )
            {
                throw new Exception("Input images must be same type (size, depth)!");
            }
        }

        #endregion
    }
}
