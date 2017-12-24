using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diploma5_csharp.Models
{
    public class RGBResponseRatioConstancyMethodParams
    {
        public double? Imin { get; set; }
        public double? Imax { get; set; }
        public MeanShiftClusteringAcordParams MeanShiftParams { get; set; }
        public bool ShowOptionalWindows { get; set; }
    }
}
