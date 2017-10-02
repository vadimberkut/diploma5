using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diploma5_csharp.Models
{
    public class BaseMethodResponse
    {
        public Image<Bgr, Byte> EnhancementResult { get; set; }
        public Image<Gray, Byte> DetectionResult { get; set; }
        public MetricsResult  Metrics { get; set; }
        public double ExecutionTimeMs { get; set; }
    }

    public class MetricsResult
    {
        public double MSE { get; set; }
        public double NAE { get; set; }
        public double SC { get; set; }
        public double PSNR { get; set; }
        public double AD { get; set; }
        public double FVM { get; set; }
    }
}
