using System;
using System.Collections.Generic;
using System.Linq;

namespace diploma5_csharp
{
    public static class StatisticsHelper
    {
        public static double Min(double[] data)
        {
            if (data.Length == 0)
                throw new ArgumentException("Array must contain elements");

            double min = data.Min();
            return min;
        }
        public static double Max(double[] data)
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
    }
}