using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diploma5_csharp.Models
{
    public class DustRemovalParams
    {
        public bool ShowWindows { get; set; }
        public string InputImageFileName { get; set; }
        /// <summary>
        /// Ground truth image
        /// </summary>
        public Image<Bgr, Byte> ImageGT { get; set; }
    }
}
