using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diploma5_csharp
{
   public  class Dust
    {

        public Image<Emgu.CV.Structure.Bgr, Byte> VisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod(Image<Bgr, Byte> image)
        {
            //Image<Bgr, Byte> result = image.Clone();
            Image<Bgr, Byte> result = new Image<Bgr, byte>(image.Size);

            //zeta (ζ), which is a tuning parameter that is used to control colors fidelity of the processed image
            double zeta = 0.5;

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

                    //result[m, n] = new Bgr(B, G, R);
                    result[m, n] = new Bgr(B2, G2, R2);
                }
            }


            return result;
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

    }
}
