using System;

namespace diploma5_csharp
{
    public static class StatisticsHelper
    {
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