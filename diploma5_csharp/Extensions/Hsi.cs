using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//namespace diploma5_csharp.Extensions
//{
//    class Hsi
//    {
//    }
//}


namespace Emgu.CV.Structure
{
    ///!!!!!!!!!!!!!!!!!!!!!!!
    /////NOT WORKING
    
        
        //
    // Summary:
    //     Defines a Hls (Hue Lightness Satuation) color
    [ColorInfo(ConversionCodename = "Hsi")]
    public struct Hsi : IColor, IEquatable<Hsi>
    {
        //
        // Summary:
        //     Create a Hls color using the specific values
        //
        // Parameters:
        //   hue:
        //     The hue value for this color ( 0 < hue < 180 )
        //
        //   satuation:
        //     The satuation for this color
        //
        //   lightness:
        //     The lightness for this color
        public Hsi(double hue, double saturation, double intensity)
        {
            Hue = hue;
            Satuation = saturation;
            Intensity = intensity;

            Hls hls = new Hls();

            Dimension = hls.Dimension;
            MCvScalar = hls.MCvScalar;
        }

        //
        // Summary:
        //     Get the dimension of this color
        public int Dimension { get; }
        //
        // Summary:
        //     Get or set the intensity of the hue color channel ( 0 < hue < 180 )
        //[DisplayColorAttribute(255, 0, 0)]
        public double Hue { get; set; }
        //
        // Summary:
        //     Get or set the intensity of the lightness color channel
        //[DisplayColorAttribute(122, 122, 122)]
        public double Satuation { get; set; }
        //
        // Summary:
        //     Get or Set the equivalent MCvScalar value
        public MCvScalar MCvScalar { get; set; }
        //
        // Summary:
        //     Get or set the intensity of the satuation color channel
        //[DisplayColorAttribute(122, 122, 122)]
        public double Intensity { get; set; }

        //
        // Summary:
        //     Return true if the two color equals
        //
        // Parameters:
        //   other:
        //     The other color to compare with
        //
        // Returns:
        //     true if the two color equals
        public bool Equals(Hsi other)
        {
            bool equal = this.Hue == other.Hue && this.Satuation == other.Satuation && this.Intensity == other.Intensity;
            return equal;
        }
        //
        // Summary:
        //     Represent this color as a String
        //
        // Returns:
        //     The string representation of this color
        public override string ToString()
        {
            string str = $"{Hue}:{Satuation}:{Intensity}";
            return str;
        }

    }
}