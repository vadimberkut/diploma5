using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diploma5_csharp
{
    public static class MathHelper
    {
        public static double VectorLength(double[] vector)
        {
            double length = 0;
            for (int i = 0; i < vector.Length; i++)
            {
                length += Math.Pow(vector[i], 2);
            }
            length = Math.Sqrt(length);
            return length;
        } 
    }
}
