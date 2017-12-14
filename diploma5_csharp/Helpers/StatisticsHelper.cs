using System;
using System.Collections.Generic;
using System.Linq;

namespace diploma5_csharp.Helpers
{
    public static class StatisticsHelper
    {
        public static double Min(params double[] data)
        {
            if (data.Length == 0)
                throw new ArgumentException("Array must contain elements");

            double min = data[0];
            for (int i = 1; i < data.Length; i++)
            {
                if (data[i] < min)
                    min = data[i];
            }
            return min;
        }
        //public static double Min(params double[] values)
        //{
        //    double min = values.Aggregate((x, y) => x < y ? x : y);
        //    return min;
        //}

        public static double Max(params double[] data)
        {
            if (data.Length == 0)
                throw new ArgumentException("Array must contain elements");

            double max = data.Max();
            return max;
        }

        public static double Average(double[] data)
        {
            double average = 0;
            for (int i = 0; i < data.Length; i++)
            {
                average += data[i];
            }
            average /= data.Length;
            return average;
        }

        public static List<double> Average(List<double[]> data)
        {
            return data.Select(Average).ToList();
        }


        // std = sqrt(variance)
        public static double StandartDeviation(double[] data)
        {
            double stdDevL = 0;
            double average = Average(data);
            int count = data.Length;
            for (int i = 0; i < count; i++)
            {
                stdDevL += Math.Pow(data[i] - average, 2);
            }
            stdDevL = stdDevL * (1.0 / ((double)count - 1.0));
            stdDevL = Math.Sqrt(stdDevL);
            return stdDevL;
        }

        public static double StandartDeviation2(double[] data)
        {
            double average = data.Average();
            double sumOfSquaresOfDifferences = data.Select(val => (val - average) * (val - average)).Sum();
            double sd = Math.Sqrt(sumOfSquaresOfDifferences / data.Length);

            return sd;
        }

        // https://statanaliz.info/metody/opisanie-dannyx/11-dispersiya-standartnoe-otklonenie-koeffitsient-variatsii
        /// <summary>
        /// Выборочная дисперсия, рассчитанная по данным наблюдений
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static double SampleVariance(double[] data)
        {
            double variance = 0;
            double n = data.Count();
            double avg = Average(data);
            double nominator = 0;

            for (int i = 0; i < n; i++)
            {
                nominator += Math.Pow(data[i] - avg, 2);
            }

            variance = nominator / n;
            return variance;
        }

        public static double SampleVariance(double[] data, double avg)
        {
            double variance = 0;
            double n = data.Count();
            double nominator = 0;

            for (int i = 0; i < n; i++)
            {
                nominator += Math.Pow(data[i] - avg, 2);
            }

            variance = nominator / n;
            return variance;
        }


        // https://ru.wikipedia.org/wiki/Ковариация
        /// <summary>
        /// Covariance of 2 data samples
        /// </summary>
        /// <param name="data"></param>
        /// <param name="avg"></param>
        /// <returns></returns>
        public static double Covariance(double[] x, double[] y)
        {
            if (x.Count() != y.Count()) throw new ArgumentException();

            double covariance = 0;
            double n = x.Count();
            double avg_x = Average(x);
            double avg_y = Average(y);

            for (int i = 0; i < n; i++)
            {
                covariance = (x[i] - avg_x) * (y[i] - avg_y);
            }

            covariance = covariance / n;
            return covariance;
        }

        public static double Covariance(double[] x, double[] y, double avg_x, double avg_y)
        {
            if (x.Count() != y.Count()) throw new ArgumentException();

            double covariance = 0;
            double n = x.Count();

            for (int i = 0; i < n; i++)
            {
                covariance = (x[i] - avg_x) * (y[i] - avg_y);
            }

            covariance = covariance / n;
            return covariance;
        }

        public static double Moda(IEnumerable<double> values)
        {
            return values.GroupBy(x => x, (key, g) => new { Value = key, Count = g.Count() }).OrderByDescending(x => x.Count).Select(x => x.Value).FirstOrDefault();
        }
    }
}