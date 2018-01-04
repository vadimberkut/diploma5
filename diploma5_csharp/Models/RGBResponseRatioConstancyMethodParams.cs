using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diploma5_csharp.Models
{
    public class RGBResponseRatioConstancyMethodParams : DustRemovalParams
    {
        public double? Imin { get; set; }
        public double? Imax { get; set; }
        public MeanShiftClusteringAcordParams MeanShiftParams { get; set; }
    }
}
