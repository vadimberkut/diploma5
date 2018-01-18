using diploma5_csharp.Helpers;
using Emgu.CV;
using Emgu.CV.Structure;

namespace diploma5_csharp
{
    public class AppState
    {
        public Form1 Form1;

        public Shadow Shadow;
        public Fog Fog;
        public Dust Dust;

        public string InputImageFileName;
        public Image<Bgr, byte> InputImageBgr;
        public Image<Lab, byte> InputImageLab;

        public Image<Gray, byte> ShadowMaskImageGray;

        public Image<Bgr, byte> OutputImageBgr;
        public Image<Bgr, byte> OutputImageBgrOrigin;

        //CONSTANTS
        public int FORM_DISPLAY_DOUBLE_PRECISION = 2;
        public int OPTIMAL_INPUT_IMAGE_WIDTH = 640;
        public int OPTIMAL_INPUT_IMAGE_HEIGHT = 480;

        public AppState(Form1 form1)
        {
            Form1 = form1;
            Shadow = new Shadow();
            Fog = new Fog();
            Dust = new Dust();
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
        public void SetOutputImage(Image<Bgr, byte> image)
        {
            this.OutputImageBgr = image;
        }
        public void SetOutputImageOrigin(Image<Bgr, byte> image)
        {
            this.OutputImageBgrOrigin = image;
        }
        public void RestoreOutputImage()
        {
            if (this.OutputImageBgrOrigin != null && this.OutputImageBgr != null)
                this.OutputImageBgr = this.OutputImageBgrOrigin;
        }
    }
}