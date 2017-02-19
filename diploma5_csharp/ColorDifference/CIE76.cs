using System;
using Emgu.CV.Structure;

namespace diploma5_csharp.ColorDifference
{
    //https://ru.wikipedia.org/wiki/Формула_цветового_отличия

    //CIE76 standart to determine Color difference (useing delta E metric)
    public class CIE76
    {
	    //примерно соответствует минимально различимому для человеческого глаза отличию между цветами
	    public static double STANDART_HUMAN_EYE_DELTA_E = 2.3;

        public static double GetMetric(Lab pixel1, Lab pixel2)
        {
            double L1 = pixel1.X;
            double L2 = pixel2.X;
            double A1 = pixel1.Y;
            double A2 = pixel2.Y;
            double B1 = pixel1.Z;
            double B2 = pixel2.Z;

            return Math.Sqrt(Math.Pow(L2 - L1, 2) + Math.Pow(A2 - A1, 2) + Math.Pow(B2 - B1, 2));
        }

        public static double GetMetric(double[] pixel1, double[] pixel2, int size)
        {
            if (size == 2)
            {
                return Math.Sqrt(Math.Pow(pixel2[0] - pixel1[0], 2) + Math.Pow(pixel2[1] - pixel1[1], 2));
            }
            if (size == 3)
            {
                return Math.Sqrt(Math.Pow(pixel2[0] - pixel1[0], 2) + Math.Pow(pixel2[1] - pixel1[1], 2) + Math.Pow(pixel2[2] - pixel1[2], 2));
            }
            throw new Exception("received invalid size (required 2 or 3)");
        }
    };
}