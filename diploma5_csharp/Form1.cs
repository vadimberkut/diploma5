using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Accord;
using Accord.MachineLearning;
using Accord.Statistics.Distributions.DensityKernels;
using diploma5_csharp.CustomFormElements;
using diploma5_csharp.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Accord.Imaging.Converters;
using Accord.Math;
using diploma5_csharp.Helpers;

namespace diploma5_csharp
{
    public partial class Form1 : Form
    {
        private AppState _appState;

        public Form1()
        {
            InitializeComponent();
            _appState = new AppState(this);
        }

        ////DISABLE X Button
        //private const int CP_NOCLOSE_BUTTON = 0x200;
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams myCp = base.CreateParams;
        //        myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
        //        return myCp;
        //    }
        //}

        #region GENERAL METHODS

        private void DisplayImageInPictureBox(PictureBox pictureBox, Image image)
        {
            //pictureBox.Dispose();
            Image displayImage = new Bitmap(image, pictureBox.Width, pictureBox.Height);
            pictureBox.Image = displayImage;
            pictureBox.Refresh();
        }

        private Nullable<T> GetTextBoxValue<T>(TextBox textBox) where T : struct
        {
            var value = textBox.Text;
            T? result;
            var type = typeof(T);

//            if (String.IsNullOrEmpty(value))
//            {
//                var test = type.GetGenericTypeDefinition();
//                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof (Nullable<>))
//                {
//                    return default(T);
//                }
//            }

            T? val = ToNullable<T>(value);
            result = val;
//            switch (Type.GetTypeCode(type))
//            {
//                case TypeCode.Double:
//                    result = Convert.ToDouble(value).To<T>();
//                    break;
//                default:
//                    throw new Exception("Unsuported type");
//                    break;
//            }
            return result;
        }

        private void SetTextBoxValue(TextBox textBox, string value)
        {
            textBox.Text = value;
        }

        private bool GetCheckBoxValue(CheckBox checkBox)
        {
            return checkBox.Checked;
        }

        public static Nullable<T> ToNullable<T>(string s) where T : struct
        {
            Nullable<T> result = new Nullable<T>();
            try
            {
                if (!string.IsNullOrEmpty(s) && s.Trim().Length > 0)
                {
                    TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                    result = (T)conv.ConvertFrom(s);
                }
            }
            catch { }
            return result;
        }
        #endregion GENERAL METHODS

        #region EVENT HANDLES

        private void button1_Click(object sender, EventArgs e)
        {
            //            String win1 = "Test Window"; //The name of the window
            //            CvInvoke.NamedWindow(win1); //Create the window using the specific name

            //            Mat img = new Mat(200, 400, DepthType.Cv8U, 3); //Create a 3 channel image of 400x200
            //            img.SetTo(new Bgr(255, 0, 0).MCvScalar); // set it to Blue color

            //            //Draw "Hello, world." on the image using the specific font
            //            CvInvoke.PutText(
            //               img,
            //               "Hello, world",
            //               new System.Drawing.Point(10, 80),
            //               FontFace.HersheyComplex,
            //               1.0,
            //               new Bgr(0, 255, 0).MCvScalar);


            ////            CvInvoke.Imshow(win1, img); //Show the image
            ////            CvInvoke.WaitKey(0);  //Wait for the key pressing event
            ////            CvInvoke.DestroyWindow(win1); //Destroy the window if key is pressed

            //            Image<Bgr, Byte> imgeOrigenal = img.ToImage<Bgr, Byte>();
            //            Mat im2 = imgeOrigenal.Mat;
            ////            pictureBox1.Image = imgeOrigenal.Bitmap;
            //            this.DisplayImageInPictureBox(pictureBox1, imgeOrigenal.Bitmap);


            //            //////////////////MEAN SHIFT
            //            // Use a fixed seed for reproducibility
            //            Accord.Math.Random.Generator.Seed = 0;

            //            // Declare some data to be clustered
            //            double[][] input =
            //            {
            //                new double[] { -5, -2, -4 },
            //                new double[] { -5, -5, -6 },
            //                new double[] {  2,  1,  1 },
            //                new double[] {  1,  1,  2 },
            //                new double[] {  1,  2,  2 },
            //                new double[] {  3,  1,  2 },
            //                new double[] { 11,  5,  4 },
            //                new double[] { 15,  5,  6 },
            //                new double[] { 10,  5,  6 },
            //            };

            //            // Create a uniform kernel density function
            //            UniformKernel kernel = new UniformKernel();

            //            // Create a new Mean-Shift algorithm for 3 dimensional samples
            //            MeanShift meanShift = new MeanShift(dimension: 3, kernel: kernel, bandwidth: 2);

            //            // Learn a data partitioning using the Mean Shift algorithm
            //            MeanShiftClusterCollection clustering = meanShift.Learn(input);

            //            // Predict group labels for each point
            //            int[] labels = clustering.Decide(input);

            //            // As a result, the first two observations should belong to the
            //            //  same cluster (thus having the same label). The same should
            //            //  happen to the next four observations and to the last three.
            //            int aasd = 0;

            
        }

        private void buttonMSTest_Click(object sender, EventArgs e)
        {
            ////////////////////MEAN SHIFT FOR IMAGE
            var msParams = new MeanShiftClusteringAcordParams() {
                Kernel = Convert.ToInt32(this.textBoxTestMsKernel.Text),
                Sigma = Convert.ToDouble(this.textBoxTestMsSigma.Text)
            };
            var msResult = Clustering.MeanShiftAccord(_appState.InputImageBgr, msParams);
            EmguCvWindowManager.Display(msResult.Image, "msResult");
        }

        private void buttonTestEmguCVCudaMeanShift_Click(object sender, EventArgs e)
        {
            try
            {
                var result = Clustering.MeanShiftEmguCVCuda(_appState.InputImageBgr);
                _appState.SetOutputImage(result);
                this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //TOP MENU
        #region TOP MENU

        //Open
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Bitmap image2 = new Bitmap("D:\\Google Drive\\Diploma5\\images_converted\\8_2.png");
            //_appState.SetInputImage(new Image<Bgr, Byte>(image2));
            //this.DisplayImageInPictureBox(pictureBox1, image2);
            //return;

            // Displays an OpenFileDialog so the user can select a Cursor.
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Image Files (*.png; *.jpg; *.jpeg; *.bmp)|*.png; *.jpg; *.jpeg; *.bmp"; // "Файлы Excel (*.xls; *.xlsx) | *.xls; *.xlsx";
            openFileDialog1.InitialDirectory = "<путь к папке>";
            openFileDialog1.Title = "Select an Image File";

            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = openFileDialog1.FileName;
                //System.IO.StreamReader sr = new System.IO.StreamReader(fileName);
                //string fileContent = sr.ReadToEnd();
                //sr.Close();
                Bitmap image = new Bitmap(fileName);
                _appState.SetInputImage(new Image<Bgr, Byte>(image));
                this.DisplayImageInPictureBox(pictureBox1, image);

                //Reset textBox params
                SetTextBoxValue(textBoxShadowDetectionLabThreshold, "");
                SetTextBoxValue(textBoxShadowDetectionLMSThreshold, "");
            }
        }

        //Save
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Select what to save
            //SaveImagePrompt.ShowDialog("sd", "sd");

            //save this in file
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Image Files (*.png; *.jpg; *.jpeg; *.bmp)|*.png; *.jpg; *.jpeg; *.bmp"; // "Файлы Excel (*.xls; *.xlsx) | *.xls; *.xlsx";
            saveFileDialog1.InitialDirectory = "<путь к папке>";
            saveFileDialog1.Title = "Save Image in File";

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _appState.OutputImageBgr.Save(saveFileDialog1.FileName);
            }
        }

        //Exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Close all windows
        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmguCvWindowManager.CloseAll();
        }

        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        #endregion

        //SHADOW DETECTION
        private void buttonShadowDetectionLab_Click(object sender, EventArgs e)
        {
            ShadowDetectionLabParams _params = new ShadowDetectionLabParams() { Threshold = GetTextBoxValue<double>(textBoxShadowDetectionLabThreshold), ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) };
            var result = _appState.Shadow.DetectUsingLabMethod(_appState.InputImageLab, _params);
            _appState.SetShadowMaskImage(result);
            this.DisplayImageInPictureBox(pictureBox2, result.Bitmap);

            //Set params from method
            SetTextBoxValue(this.textBoxShadowDetectionLabThreshold, Math.Round((double)_params.Threshold, _appState.FORM_DISPLAY_DOUBLE_PRECISION).ToString());
        }

        private void buttonShadowDetectionMS_Click(object sender, EventArgs e)
        {
            ShadowDetectionMSParams _params = new ShadowDetectionMSParams() {Threshold = GetTextBoxValue<double>(textBoxShadowDetectionLMSThreshold), ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) };
            var result = _appState.Shadow.DetectUsingMSMethod(_appState.InputImageBgr, _params);
            _appState.SetShadowMaskImage(result);
            this.DisplayImageInPictureBox(pictureBox2, result.Bitmap);

            //Set params from method
            SetTextBoxValue(this.textBoxShadowDetectionLMSThreshold, Math.Round((double)_params.Threshold, _appState.FORM_DISPLAY_DOUBLE_PRECISION).ToString());
        }


        //Modified Ratio Of Hue Over Intensity Method
        private void buttonDetectUsingModifiedRatioOfHueOverIntensityMethod_Click(object sender, EventArgs e)
        {
            var result = _appState.Shadow.DetectUsingModifiedRatioOfHueOverIntensityMethod(_appState.InputImageBgr);
            _appState.SetShadowMaskImage(result);
            this.DisplayImageInPictureBox(pictureBox2, result.Bitmap);
        }


        //SHADOW REMOVAL
        private void buttonShadowRemovalAditiveMethod_Click(object sender, EventArgs e)
        {
            var result = _appState.Shadow.RemoveUsingAditiveMethod(_appState.InputImageBgr, _appState.ShadowMaskImageGray, new ShadowRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) } );
            _appState.SetOutputImage(result);
            _appState.SetOutputImageOrigin(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);
        }

        private void buttonShadowRemovalBasicLightModelMethod_Click(object sender, EventArgs e)
        {
            var result = _appState.Shadow.RemoveUsingBasicLightModelMethod(_appState.InputImageBgr, _appState.ShadowMaskImageGray, new ShadowRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) });
            _appState.SetOutputImage(result);
            _appState.SetOutputImageOrigin(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);
        }

        private void buttonShadowRemovalCombinedMethod_Click(object sender, EventArgs e)
        {
            var result = _appState.Shadow.RemoveUsingCombinedMethod(_appState.InputImageBgr, _appState.ShadowMaskImageGray, new ShadowRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) });
            _appState.SetOutputImage(result);
            _appState.SetOutputImageOrigin(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);
        }

        private void buttonShadowRemovalLabMethod_Click(object sender, EventArgs e)
        {
            var result = _appState.Shadow.RemoveUsingLabMethod2(_appState.InputImageBgr, _appState.ShadowMaskImageGray, new ShadowRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) });
            _appState.SetOutputImage(result);
            _appState.SetOutputImageOrigin(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);
        }

        private void buttonShadowRemovalLabMethod2_Click(object sender, EventArgs e)
        {
            var result = _appState.Shadow.RemoveUsingLabMethod2(_appState.InputImageBgr, _appState.ShadowMaskImageGray, new ShadowRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) });
            _appState.SetOutputImage(result);
            _appState.SetOutputImageOrigin(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);
        }

        private void buttonShadowRemovalConstantMethod_Click(object sender, EventArgs e)
        {
            var result = _appState.Shadow.RemoveUsingConstantMethod(_appState.InputImageBgr, _appState.ShadowMaskImageGray, new ShadowRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) });
            _appState.SetOutputImage(result);
            _appState.SetOutputImageOrigin(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);
        }


        //
        //SHADOW EDGE PROCESSING
        //
        private void buttonImpaintShadowEdges_Click(object sender, EventArgs e)
        {
            var result = _appState.Shadow.InpaintShadowEdges(_appState.OutputImageBgr, _appState.ShadowMaskImageGray);
            _appState.SetOutputImage(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);
        }

        private void buttonSmoothShadowEdgesUsingGaussianFilter_Click(object sender, EventArgs e)
        {
            var result = _appState.Shadow.SmoothShadowEdgesUsingGaussianFilter(_appState.OutputImageBgr, _appState.ShadowMaskImageGray);
            _appState.SetOutputImage(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);
        }

        private void buttonSmoothShadowEdgesUsingMedianFilter_Click(object sender, EventArgs e)
        {
            var result = _appState.Shadow.SmoothShadowEdgesUsingMedianFilter(_appState.OutputImageBgr, _appState.ShadowMaskImageGray);
            _appState.SetOutputImage(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);
        }

        //
        //FOG REMOVAL
        //
        private void buttonRemoveFogUsingDarkChannelMethod_Click(object sender, EventArgs e)
        {
            var result = _appState.Fog.RemoveFogUsingDarkChannelPrior(_appState.InputImageBgr, out _appState.ShadowMaskImageGray, new FogRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) });
            _appState.SetOutputImage(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);
            this.DisplayImageInPictureBox(pictureBox2, _appState.ShadowMaskImageGray.Bitmap);
        }

        private void buttonRobbyTanFogRemovalMethod_Click(object sender, EventArgs e)
        {
            var result = _appState.Fog.RemoveUsingRobbyTanMethod(_appState.InputImageBgr, new FogRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) });
            _appState.SetOutputImage(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);
            //this.DisplayImageInPictureBox(pictureBox2, _appState.ShadowMaskImageGray.Bitmap);
        }

        private void buttonRemoveFogUsingMedianChannelPrior_Click(object sender, EventArgs e)
        {
            var result = _appState.Fog.RemoveFogUsingMedianChannelPrior(_appState.InputImageBgr, out _appState.ShadowMaskImageGray, new FogRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) });
            _appState.SetOutputImage(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);
            this.DisplayImageInPictureBox(pictureBox2, _appState.ShadowMaskImageGray.Bitmap);
        }

        private void buttonRemoveFogUsingIdcpWithClahe_Click(object sender, EventArgs e)
        {
            var result = _appState.Fog.RemoveFogUsingIdcpWithClahe(_appState.InputImageBgr, out _appState.ShadowMaskImageGray, new FogRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) });
            _appState.SetOutputImage(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);
            this.DisplayImageInPictureBox(pictureBox2, _appState.ShadowMaskImageGray.Bitmap);
        }

        //
        //VISIBILITY ENHANCEMENT
        //
        private void buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod_Click(object sender, EventArgs e)
        {
            var _params = new TriThresholdFuzzyIntensificationOperatorsMethodParams();
            if (!String.IsNullOrEmpty(textBoxTriThresholdFuzzyIntensificationOperatorsMethod_Dzeta.Text))
                _params.Dzeta = double.Parse(textBoxTriThresholdFuzzyIntensificationOperatorsMethod_Dzeta.Text);

            var result = _appState.Dust.VisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod(_appState.InputImageBgr, _params);
            _appState.SetOutputImage(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);
        }
        private void buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod_Click(object sender, EventArgs e)
        {
            string kernel = textBox_RatioConstancyMethod_kernel.Text;
            string sigma = textBox_RatioConstancyMethod_sigma.Text;

            var _params = new RGBResponseRatioConstancyMethodParams();

            if(!String.IsNullOrEmpty(kernel) && !String.IsNullOrEmpty(sigma))
            {
                _params.MeanShiftParams = new MeanShiftClusteringAcordParams()
                {
                    Kernel = int.Parse(kernel),
                    Sigma = double.Parse(sigma)
                };
            }

            var result = _appState.Dust.RecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod(_appState.InputImageBgr, _params);
            _appState.SetOutputImage(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);
        }

        //RESTORE RESULT IMAGE
        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _appState.RestoreOutputImage();
            this.DisplayImageInPictureBox(pictureBox3, _appState.OutputImageBgr.Bitmap);
        }

        // AGC
        private void buttonApplyAGC_Click(object sender, EventArgs e)
        {
            var result = GammaCorrection.Adaptive(_appState.InputImageBgr, showWindows: true);
            _appState.SetOutputImage(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);
        }

        private void buttonTestFilters_Click(object sender, EventArgs e)
        {
            var image = _appState.InputImageBgr;

            Image<Bgr, byte> blur = image.SmoothBlur(10, 10, true);
            Image<Bgr, byte> mediansmooth = image.SmoothMedian(15);
            Image<Bgr, byte> bilat = image.SmoothBilatral(7, 255, 34);
            Image<Bgr, byte> gauss = image.SmoothGaussian(3, 3, 34.3, 45.3);

            EmguCvWindowManager.Display(blur, "0 blur");
            EmguCvWindowManager.Display(mediansmooth, "0 mediansmooth");
            EmguCvWindowManager.Display(bilat, "0 bilat");
            EmguCvWindowManager.Display(gauss, "0 gauss");
        }
    }
}
