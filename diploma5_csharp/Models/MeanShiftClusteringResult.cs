using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diploma5_csharp.Models
{
    public class MeanShiftClusteringResult
    {
        public Image<Emgu.CV.Structure.Bgr, Byte> Image { get; set; }
        public int[] Labels { get; set; }
        public int RegionCount { get; set; }

    }
}
