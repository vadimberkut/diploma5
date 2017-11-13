using AForge.Imaging.Filters;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuidedFilter2
{
    public static  class GuidedFilterHelper
    {
        #region Guided Filter (GF)

        // Use method from forked Accord v.3.0.2 repository that uses AForge.Net
        // https://github.com/hzawary/CSharpDemoFastGuidedFilter

        public static Image<Bgr, byte> FastGuidedFilter(Image<Bgr, byte> image)
        {
            Image<Bgr, byte> result;
            var bmp = image.Bitmap;

            var fastGuidedFilter = new AForge.Imaging.Filters.FastGuidedFilter
            {
                KernelSize = 8,
                Epsilon = 0.02f,
                SubSamplingRatio = 0.25f,
                OverlayImage = (Bitmap)bmp.Clone()
            };

            var filtered = fastGuidedFilter.Apply(bmp);
            fastGuidedFilter.OverlayImage.Dispose();

            result = new Image<Bgr, byte>(filtered);
            return result;
        }

        public static Image<Gray, byte> FastGuidedFilter(Image<Gray, byte> image)
        {
            return FastGuidedFilter(image.Convert<Bgr, byte>()).Convert<Gray, byte>();
        }

        public static Image<Bgr, byte> FastGuidedFilterEnchacement(Image<Bgr, byte> image)
        {
            Image<Bgr, byte> result;
            var bmp = image.Bitmap;

            var fastGuidedFilter = new FastGuidedFilter
            {
                KernelSize = 16,
                Epsilon = 0.16f,
                SubSamplingRatio = 0.25f,
                OverlayImage = (Bitmap)bmp.Clone()
            };

            var guidedImage = fastGuidedFilter.Apply(bmp);
            
            var subtracted = new AForge.Imaging.Filters.Subtract(guidedImage).Apply(fastGuidedFilter.OverlayImage);
            guidedImage.Dispose();

            //var mul = 100;
            //var Multiply = FastGuidedFilter.GetFilledImage(
            //    subtracted.Width, subtracted.Height,
            //    subtracted.PixelFormat, Color.FromArgb(mul, mul, mul));

            //new Multiply(Multiply).ApplyInPlace(subtracted);
            //Multiply.Dispose();

            new AForge.Imaging.Filters.Add(subtracted).ApplyInPlace(fastGuidedFilter.OverlayImage);
            subtracted.Dispose();
            fastGuidedFilter.OverlayImage.Dispose();

            result = new Image<Bgr, byte>(fastGuidedFilter.OverlayImage);
            return result;
        }

        public static Image<Gray, byte> FastGuidedFilterEnchacement(Image<Gray, byte> image)
        {
            return GuidedFilterHelper.FastGuidedFilter(image.Convert<Bgr, byte>()).Convert<Gray, byte>();
        }

        #endregion
    }
}
