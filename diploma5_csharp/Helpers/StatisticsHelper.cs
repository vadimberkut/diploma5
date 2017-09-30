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
    }
}