using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diploma5_csharp.Models
{
    

    public class EdgeInpaintModel
    {
        public int?DilationKernelSize { get; set; }
        public int? KernelRadius { get; set; }
    }

    public class EdgeGaussianModel
    {
        public int? DilationKernelSize { get; set; }
        public int? KernelRadius { get; set; }
    }

    public class EdgeMedianModel
    {
        public int? DilationKernelSize { get; set; }
        public int? KernelRadius { get; set; }
    }
}
