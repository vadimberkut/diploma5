using diploma5_csharp.Extensions;
using diploma5_csharp.Models;
using Emgu.CV;
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
        #region Public members

        public static MetricsResult ComputeAll(Image<Bgr, double> image1, Image<Bgr, double> image2)
        {
            double MSE = ImageMetricHelper.MSE(image1.Convert<Bgr, double>(), image2.Convert<Bgr, double>());
            double NAE = ImageMetricHelper.NAE(image1.Convert<Bgr, double>(), image2.Convert<Bgr, double>());
            double SC = ImageMetricHelper.SC(image1.Convert<Bgr, double>(), image2.Convert<Bgr, double>());
            double PSNR = ImageMetricHelper.PSNR(image1.Convert<Bgr, double>(), image2.Convert<Bgr, double>());
            double AD = ImageMetricHelper.AD(image1.Convert<Bgr, double>(), image2.Convert<Bgr, double>());

            return new MetricsResult
            {
                MSE = MSE,
                NAE = NAE,
                SC = SC,
                PSNR = PSNR,
                AD = AD
            };
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
