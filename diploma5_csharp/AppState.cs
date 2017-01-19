using Emgu.CV;
using Emgu.CV.Structure;

namespace diploma5_csharp
{
    public class AppState
    {
        public Form1 Form1;

        public Shadow Shadow;

        public Image<Bgr, byte> InputImageBgr;
        public Image<Lab, byte> InputImageLab;
        public Image<Bgr, byte> OutputImageBgr;
        public Image<Gray, byte> ShadowMaskImageGray;

        public AppState(Form1 form1)
        {
            Form1 = form1;
            Shadow = new Shadow();
        }

        public void SetInputImage(Image<Bgr, byte> inputImage)
        {
            this.InputImageBgr = inputImage;
            this.InputImageLab = ImageHelper.ToLab(inputImage);
        }
        public void SetShadowMaskImage(Image<Gray, byte> inputImage)
        {
            this.ShadowMaskImageGray = inputImage;
        }
    }
}