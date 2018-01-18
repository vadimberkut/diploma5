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
using System.IO;
using System.Text.RegularExpressions;

namespace diploma5_csharp
{
    public partial class Form1 : Form
    {
        private AppState _appState;
        private MethodInfoStore _methodInfoStore;

        public Form1()
        {
            InitializeComponent();
            _appState = new AppState(this);
            _methodInfoStore = new MethodInfoStore();
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

        #region Prerequrements checks


        /// <summary>
        /// Checks different conditions before start image processing and displays appropriate messages to user
        /// </summary>
        /// <returns></returns>
        private bool CheckBasePrerequirements()
        {
            // Check image was opened
            if (_appState.InputImageBgr == null)
            {
                MessageBox.Show("You need to open input image first!");
                return false;
            }

            return true;
        }

        private bool CheckDetectionResultSavePrerequirements()
        {
            if(!this.CheckBasePrerequirements()) return false;

            // Check image was opened
            if (_appState.ShadowMaskImageGray == null)
            {
                MessageBox.Show("No detection result to save! E.g. you need to perform shadow detection");
                return false;
            }

            return true;
        }

        private bool CheckResultSavePrerequirements()
        {
            if (!this.CheckBasePrerequirements()) return false;

            // Check image was opened
            if (_appState.OutputImageBgr == null)
            {
                MessageBox.Show("You need to process input image. Then you can save result!");
                return false;
            }

            return true;
        }

        private bool CheckShadowDetectionPrerequirements()
        {
            if (!this.CheckBasePrerequirements()) return false;

            return true;
        }

        private bool CheckShadowRemovalPrerequirements()
        {
            if (!this.CheckBasePrerequirements()) return false;

            // Check that shadow mask is present
            if (_appState.ShadowMaskImageGray == null)
            {
                MessageBox.Show("You need to perform shadow detection in order to delete shadow from image!");
                return false;
            }

            return true;
        }

        private bool CheckShadowEdgeProcessingPrerequirements()
        {
            if (!this.CheckBasePrerequirements()) return false;
            if (!this.CheckShadowDetectionPrerequirements()) return false;
            if (!this.CheckShadowRemovalPrerequirements()) return false;

            // Check that shadow mask is present
            if (_appState.OutputImageBgr == null)
            {
                MessageBox.Show("You need to perform shadow removal in order to process shadow edges!");
                return false;
            }

            return true;
        }

        private bool CheckFogRemovalPrerequirements()
        {
            if (!this.CheckBasePrerequirements()) return false;

            return true;
        }

        private bool CheckDustRemovalPrerequirements()
        {
            if (!this.CheckBasePrerequirements()) return false;

            return true;
        }

        #endregion

        private void StartImageProcessing()
        {
            Cursor.Current = Cursors.WaitCursor;
        }
        private void EndImageProcessing()
        {
            Cursor.Current = Cursors.Default;
        }

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

        // FORM LOAD
        private void Form1_Load(object sender, EventArgs e)
        {
            // Set tooltips
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
                Bitmap image = new Bitmap(fileName);
                _appState.InputImageFileName = fileName;
                var image2 = new Image<Bgr, Byte>(image);

                // resize to be even
                if (image2.Rows % 2 != 0 || image2.Cols % 2 != 0)
                {
                    int rows = image2.Rows;
                    int cols = image2.Cols;
                    if (rows % 2 != 0)
                    {
                        rows += 1;
                    }
                    if (cols % 2 != 0)
                    {
                        cols += 1;
                    }

                    var image3 = image2.Resize(cols, rows, Inter.Linear);
                    image2.Dispose();
                    image2 = image3;
                }

                // resize to optimal size
                if (this.checkBoxMinifyLargeImages.Checked)
                {
                    if(image2.Width > _appState.OPTIMAL_INPUT_IMAGE_WIDTH || image2.Height > _appState.OPTIMAL_INPUT_IMAGE_HEIGHT)
                    {
                        var t = image2.Resize(_appState.OPTIMAL_INPUT_IMAGE_WIDTH, _appState.OPTIMAL_INPUT_IMAGE_HEIGHT, Inter.Linear);
                        image2.Dispose();
                        image2 = t;
                    }
                }

                _appState.SetInputImage(image2);
                this.DisplayImageInPictureBox(pictureBox1, image);

                //Reset textBox params
                SetTextBoxValue(textBoxShadowDetectionLabThreshold, "");
                SetTextBoxValue(textBoxShadowDetectionLMSThreshold, "");
            }
        }

        // Save Detection Result
        private void saveDetectionResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.CheckDetectionResultSavePrerequirements()) return;

            //save this in file
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Image Files (*.png; *.jpg; *.jpeg; *.bmp)|*.png; *.jpg; *.jpeg; *.bmp"; // "Файлы Excel (*.xls; *.xlsx) | *.xls; *.xlsx";
            saveFileDialog1.InitialDirectory = "<путь к папке>";
            saveFileDialog1.Title = "Save Detection Result Image in File";

            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                _appState.ShadowMaskImageGray.Save(saveFileDialog1.FileName);
            }
        }

        //Save
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!this.CheckResultSavePrerequirements()) return;

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

        // Export metrics to CSV
        private void exportMetricsToCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string folderPath = "";
            string savedPath = this._methodInfoStore.SaveToCsv(folderPath);
            MessageBox.Show($"Saved to {savedPath}");
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

        private void resetMethodsStatisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this._methodInfoStore.Reset();
        }

        #endregion

        

        #endregion

        //SHADOW DETECTION
        private void buttonShadowDetectionLab_Click(object sender, EventArgs e)
        {
            if (!this.CheckShadowDetectionPrerequirements()) return;
            StartImageProcessing();

            ShadowDetectionLabParams _params = new ShadowDetectionLabParams() { Threshold = GetTextBoxValue<double>(textBoxShadowDetectionLabThreshold), ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) };
            var result = _appState.Shadow.DetectUsingLabMethod(_appState.InputImageLab, _params);
            _appState.SetShadowMaskImage(result);
            this.DisplayImageInPictureBox(pictureBox2, result.Bitmap);

            //Set params from method
            SetTextBoxValue(this.textBoxShadowDetectionLabThreshold, Math.Round((double)_params.Threshold, _appState.FORM_DISPLAY_DOUBLE_PRECISION).ToString());
            EndImageProcessing();
        }

        private void buttonShadowDetectionMS_Click(object sender, EventArgs e)
        {
            if (!this.CheckShadowDetectionPrerequirements()) return;
            StartImageProcessing();

            ShadowDetectionMSParams _params = new ShadowDetectionMSParams() {Threshold = GetTextBoxValue<double>(textBoxShadowDetectionLMSThreshold), ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) };
            var result = _appState.Shadow.DetectUsingMSMethod(_appState.InputImageBgr, _params);
            _appState.SetShadowMaskImage(result);
            this.DisplayImageInPictureBox(pictureBox2, result.Bitmap);

            //Set params from method
            SetTextBoxValue(this.textBoxShadowDetectionLMSThreshold, Math.Round((double)_params.Threshold, _appState.FORM_DISPLAY_DOUBLE_PRECISION).ToString());
            EndImageProcessing();
        }

        //Modified Ratio Of Hue Over Intensity Method
        private void buttonDetectUsingModifiedRatioOfHueOverIntensityMethod_Click(object sender, EventArgs e)
        {
            if (!this.CheckShadowDetectionPrerequirements()) return;
            StartImageProcessing();

            ShadowDetectionParams _params = new ShadowDetectionParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) };
            var result = _appState.Shadow.DetectUsingModifiedRatioOfHueOverIntensityMethod(_appState.InputImageBgr, _params);
            _appState.SetShadowMaskImage(result);
            this.DisplayImageInPictureBox(pictureBox2, result.Bitmap);
            EndImageProcessing();
        }


        

        //SHADOW REMOVAL
        private void buttonShadowRemovalAditiveMethod_Click(object sender, EventArgs e)
        {
            if (!this.CheckShadowRemovalPrerequirements()) return;
            StartImageProcessing();

            var result = _appState.Shadow.RemoveUsingAditiveMethod(_appState.InputImageBgr, _appState.ShadowMaskImageGray, new ShadowRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) } );
            _appState.SetOutputImage(result);
            _appState.SetOutputImageOrigin(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);
            EndImageProcessing();
        }

        private void buttonShadowRemovalBasicLightModelMethod_Click(object sender, EventArgs e)
        {
            if (!this.CheckShadowRemovalPrerequirements()) return;
            StartImageProcessing();

            var result = _appState.Shadow.RemoveUsingBasicLightModelMethod(_appState.InputImageBgr, _appState.ShadowMaskImageGray, new ShadowRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) });
            _appState.SetOutputImage(result);
            _appState.SetOutputImageOrigin(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);
            EndImageProcessing();
        }

        private void buttonShadowRemovalCombinedMethod_Click(object sender, EventArgs e)
        {
            if (!this.CheckShadowRemovalPrerequirements()) return;
            StartImageProcessing();

            var result = _appState.Shadow.RemoveUsingCombinedMethod(_appState.InputImageBgr, _appState.ShadowMaskImageGray, new ShadowRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) });
            _appState.SetOutputImage(result);
            _appState.SetOutputImageOrigin(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);
            EndImageProcessing();
        }

        private void buttonShadowRemovalLabMethod_Click(object sender, EventArgs e)
        {
            if (!this.CheckShadowRemovalPrerequirements()) return;
            StartImageProcessing();

            var result = _appState.Shadow.RemoveUsingLabMethod2(_appState.InputImageBgr, _appState.ShadowMaskImageGray, new ShadowRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) });
            _appState.SetOutputImage(result);
            _appState.SetOutputImageOrigin(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);
            EndImageProcessing();
        }

        private void buttonShadowRemovalLabMethod2_Click(object sender, EventArgs e)
        {
            if (!this.CheckShadowRemovalPrerequirements()) return;
            StartImageProcessing();

            var result = _appState.Shadow.RemoveUsingLabMethod2(_appState.InputImageBgr, _appState.ShadowMaskImageGray, new ShadowRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) });
            _appState.SetOutputImage(result);
            _appState.SetOutputImageOrigin(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);
            EndImageProcessing();
        }

        private void buttonShadowRemovalConstantMethod_Click(object sender, EventArgs e)
        {
            if (!this.CheckShadowRemovalPrerequirements()) return;
            StartImageProcessing();

            var result = _appState.Shadow.RemoveUsingConstantMethod(_appState.InputImageBgr, _appState.ShadowMaskImageGray, new ShadowRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) });
            _appState.SetOutputImage(result);
            _appState.SetOutputImageOrigin(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);
            EndImageProcessing();
        }


        //
        //SHADOW EDGE PROCESSING
        //
        private void buttonImpaintShadowEdges_Click(object sender, EventArgs e)
        {
            if (!this.CheckShadowEdgeProcessingPrerequirements()) return;
            StartImageProcessing();

            EdgeInpaintModel _params = new EdgeInpaintModel();
            try
            {
                if (!String.IsNullOrEmpty(textBoxShadowEdgeInpaint_DilationKernelSize.Text))
                    _params.DilationKernelSize = int.Parse(textBoxShadowEdgeInpaint_DilationKernelSize.Text);
                if (!String.IsNullOrEmpty(textBoxShadowEdgeInpaint_KernelRadius.Text))
                    _params.KernelRadius = int.Parse(textBoxShadowEdgeInpaint_KernelRadius.Text);
            }
            catch(FormatException ex)
            {
                MessageBox.Show(ex.Message, "Incorrect parameters!");
            }

            var result = _appState.Shadow.InpaintShadowEdges(_appState.OutputImageBgr, _appState.ShadowMaskImageGray, _params);
            _appState.SetOutputImage(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);

            //Set params from method
            SetTextBoxValue(this.textBoxShadowEdgeInpaint_DilationKernelSize, _params.DilationKernelSize.ToString());
            SetTextBoxValue(this.textBoxShadowEdgeInpaint_KernelRadius, _params.KernelRadius.ToString());

            EndImageProcessing();
        }

        private void buttonSmoothShadowEdgesUsingGaussianFilter_Click(object sender, EventArgs e)
        {
            if (!this.CheckShadowEdgeProcessingPrerequirements()) return;
            StartImageProcessing();

            EdgeGaussianModel _params = new EdgeGaussianModel();
            
            try
            {
                if (!String.IsNullOrEmpty(textBoxShadowEdgeGaussian_DilationKernelSize.Text))
                    _params.DilationKernelSize = int.Parse(textBoxShadowEdgeGaussian_DilationKernelSize.Text);
                if (!String.IsNullOrEmpty(textBoxShadowEdgeGaussian_KernelRadius.Text))
                    _params.KernelRadius = int.Parse(textBoxShadowEdgeGaussian_KernelRadius.Text);
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message, "Incorrect parameters!");
            }

            var result = _appState.Shadow.SmoothShadowEdgesUsingGaussianFilter(_appState.OutputImageBgr, _appState.ShadowMaskImageGray, _params);
            _appState.SetOutputImage(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);

            //Set params from method
            SetTextBoxValue(this.textBoxShadowEdgeGaussian_DilationKernelSize, _params.DilationKernelSize.ToString());
            SetTextBoxValue(this.textBoxShadowEdgeGaussian_KernelRadius, _params.KernelRadius.ToString());

            EndImageProcessing();
        }

        private void buttonSmoothShadowEdgesUsingMedianFilter_Click(object sender, EventArgs e)
        {
            if (!this.CheckShadowEdgeProcessingPrerequirements()) return;
            StartImageProcessing();

            EdgeMedianModel _params = new EdgeMedianModel();
            try
            {
                if (!String.IsNullOrEmpty(textBoxShadowEdgeMedian_DilationKernelSIze.Text))
                    _params.DilationKernelSize = int.Parse(textBoxShadowEdgeMedian_DilationKernelSIze.Text);
                if (!String.IsNullOrEmpty(textBoxShadowEdgeMedian_KernelRadius.Text))
                    _params.KernelRadius = int.Parse(textBoxShadowEdgeMedian_KernelRadius.Text);
            }
            catch (FormatException ex)
            {
                MessageBox.Show(ex.Message, "Incorrect parameters!");
            }

            var result = _appState.Shadow.SmoothShadowEdgesUsingMedianFilter(_appState.OutputImageBgr, _appState.ShadowMaskImageGray, _params);
            _appState.SetOutputImage(result);
            this.DisplayImageInPictureBox(pictureBox3, result.Bitmap);

            //Set params from method
            SetTextBoxValue(this.textBoxShadowEdgeMedian_DilationKernelSIze, _params.DilationKernelSize.ToString());
            SetTextBoxValue(this.textBoxShadowEdgeMedian_KernelRadius, _params.KernelRadius.ToString());

            EndImageProcessing();
        }



        //
        //FOG REMOVAL
        //
        private void buttonRemoveFogUsingDarkChannelMethod_Click(object sender, EventArgs e)
        {
            if (!this.CheckFogRemovalPrerequirements()) return;
            StartImageProcessing();

            var result = _appState.Fog.RemoveFogUsingDarkChannelPrior(_appState.InputImageBgr, new FogRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) });
            _appState.SetOutputImage(result.EnhancementResult);
            _appState.SetShadowMaskImage(result.DetectionResult);
            this.DisplayImageInPictureBox(pictureBox3, result.EnhancementResult.Bitmap);
            this.DisplayImageInPictureBox(pictureBox2, result.DetectionResult.Bitmap);

            // save metrics
            _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            {
                ImageFileName = _appState.InputImageFileName,
                EnhanceMethodName = nameof(Fog.RemoveFogUsingDarkChannelPrior),
                Metrics = null,
                ExecutionTimeMs = result.ExecutionTimeMs
            });
            
            EndImageProcessing();
        }

        private void buttonRobbyTanFogRemovalMethod_Click(object sender, EventArgs e)
        {
            if (!this.CheckFogRemovalPrerequirements()) return;
            StartImageProcessing();

            var result = _appState.Fog.RemoveUsingRobbyTanMethod(_appState.InputImageBgr, new FogRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) });
            _appState.SetOutputImage(result.EnhancementResult);
            _appState.SetShadowMaskImage(result.DetectionResult);
            this.DisplayImageInPictureBox(pictureBox3, result.EnhancementResult.Bitmap);
            this.DisplayImageInPictureBox(pictureBox2, result.DetectionResult.Bitmap);

            // save metrics
            _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            {
                ImageFileName = _appState.InputImageFileName,
                EnhanceMethodName = nameof(Fog.RemoveUsingRobbyTanMethod),
                Metrics = null,
                ExecutionTimeMs = result.ExecutionTimeMs
            });
            
            EndImageProcessing();
        }

        private void buttonRemoveFogUsingMedianChannelPrior_Click(object sender, EventArgs e)
        {
            if (!this.CheckFogRemovalPrerequirements()) return;
            StartImageProcessing();

            var result = _appState.Fog.RemoveFogUsingMedianChannelPrior(_appState.InputImageBgr, new FogRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) });
            _appState.SetOutputImage(result.EnhancementResult);
            _appState.SetShadowMaskImage(result.DetectionResult);
            this.DisplayImageInPictureBox(pictureBox3, result.EnhancementResult.Bitmap);
            this.DisplayImageInPictureBox(pictureBox2, result.DetectionResult.Bitmap);

            // save metrics
            _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            {
                ImageFileName = _appState.InputImageFileName,
                EnhanceMethodName = nameof(Fog.RemoveFogUsingMedianChannelPrior),
                Metrics = null,
                ExecutionTimeMs = result.ExecutionTimeMs
            });
            
            EndImageProcessing();
        }

        private void buttonRemoveFogUsingIdcpWithClahe_Click(object sender, EventArgs e)
        {
            if (!this.CheckFogRemovalPrerequirements()) return;
            StartImageProcessing();

            var result = _appState.Fog.RemoveFogUsingIdcpWithClahe(_appState.InputImageBgr, new FogRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) });
            _appState.SetOutputImage(result.EnhancementResult);
            _appState.SetShadowMaskImage(result.DetectionResult);
            this.DisplayImageInPictureBox(pictureBox3, result.EnhancementResult.Bitmap);
            this.DisplayImageInPictureBox(pictureBox2, result.DetectionResult.Bitmap);

            // save metrics
            _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            {
                ImageFileName = _appState.InputImageFileName,
                EnhanceMethodName = nameof(Fog.RemoveFogUsingIdcpWithClahe),
                Metrics = null,
                ExecutionTimeMs = result.ExecutionTimeMs
            });
            
            EndImageProcessing();
        }

        private void buttonEnhaceVisibilityUsingRobbyTanMethodForRoads_Click(object sender, EventArgs e)
        {
            if (!this.CheckFogRemovalPrerequirements()) return;
            StartImageProcessing();

            var result = _appState.Fog.EnhaceVisibilityUsingRobbyTanMethodForRoads(_appState.InputImageBgr, new FogRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) });
            _appState.SetOutputImage(result.EnhancementResult);
            _appState.SetShadowMaskImage(result.DetectionResult);
            this.DisplayImageInPictureBox(pictureBox3, result.EnhancementResult.Bitmap);
            this.DisplayImageInPictureBox(pictureBox2, result.DetectionResult.Bitmap);

            // save metrics
            _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            {
                ImageFileName = _appState.InputImageFileName,
                EnhanceMethodName = nameof(Fog.EnhaceVisibilityUsingRobbyTanMethodForRoads),
                Metrics = null,
                ExecutionTimeMs = result.ExecutionTimeMs
            });
            
            EndImageProcessing();
        }

        private void buttonRemoveFogUsingDCPAndDFT_Click(object sender, EventArgs e)
        {
            if (!this.CheckFogRemovalPrerequirements()) return;
            StartImageProcessing();

            var result = _appState.Fog.RemoveFogUsingDCPAndDFT(_appState.InputImageBgr, new FogRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) });
            _appState.SetOutputImage(result.EnhancementResult);
            _appState.SetShadowMaskImage(result.DetectionResult);
            this.DisplayImageInPictureBox(pictureBox3, result.EnhancementResult.Bitmap);
            this.DisplayImageInPictureBox(pictureBox2, result.DetectionResult.Bitmap);

            // save metrics
            _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            {
                ImageFileName = _appState.InputImageFileName,
                EnhanceMethodName = nameof(Fog.RemoveFogUsingDCPAndDFT),
                Metrics = null,
                ExecutionTimeMs = result.ExecutionTimeMs
            });
            
            EndImageProcessing();
        }

        private void buttonRemoveFogUsingLocalExtremaMethod_Click(object sender, EventArgs e)
        {
            if (!this.CheckFogRemovalPrerequirements()) return;
            StartImageProcessing();

            var result = _appState.Fog.RemoveFogUsingLocalExtremaMethod(_appState.InputImageBgr, new FogRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) });
            _appState.SetOutputImage(result.EnhancementResult);
            _appState.SetShadowMaskImage(result.DetectionResult);
            this.DisplayImageInPictureBox(pictureBox3, result.EnhancementResult.Bitmap);
            this.DisplayImageInPictureBox(pictureBox2, result.DetectionResult.Bitmap);

            // save metrics
            _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            {
                ImageFileName = _appState.InputImageFileName,
                EnhanceMethodName = nameof(Fog.RemoveFogUsingLocalExtremaMethod),
                Metrics = null,
                ExecutionTimeMs = result.ExecutionTimeMs
            });
            
            EndImageProcessing();
        }

        private void buttonRemoveFogUsingPhysicsBasedMethod_Click(object sender, EventArgs e)
        {
            if (!this.CheckFogRemovalPrerequirements()) return;
            StartImageProcessing();

            var result = _appState.Fog.RemoveFogUsingPhysicsBasedMethod(_appState.InputImageBgr, new FogRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) });
            _appState.SetOutputImage(result.EnhancementResult);
            _appState.SetShadowMaskImage(result.DetectionResult);
            this.DisplayImageInPictureBox(pictureBox3, result.EnhancementResult.Bitmap);
            this.DisplayImageInPictureBox(pictureBox2, result.DetectionResult.Bitmap);

            // save metrics
            _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            {
                ImageFileName = _appState.InputImageFileName,
                EnhanceMethodName = nameof(Fog.RemoveFogUsingPhysicsBasedMethod),
                Metrics = null,
                ExecutionTimeMs = result.ExecutionTimeMs
            });
            
            EndImageProcessing();
        }

        private void buttonRemoveFogUsingMultiCoreDSPMethod_Click(object sender, EventArgs e)
        {
            if (!this.CheckFogRemovalPrerequirements()) return;
            StartImageProcessing();

            var result = _appState.Fog.RemoveFogUsingMultiCoreDSPMethod(_appState.InputImageBgr, new FogRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows) });
            _appState.SetOutputImage(result.EnhancementResult);
            _appState.SetShadowMaskImage(result.DetectionResult);
            this.DisplayImageInPictureBox(pictureBox3, result.EnhancementResult.Bitmap);
            this.DisplayImageInPictureBox(pictureBox2, result.DetectionResult.Bitmap);

            // save metrics
            _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            {
                ImageFileName = _appState.InputImageFileName,
                EnhanceMethodName = nameof(Fog.RemoveFogUsingMultiCoreDSPMethod),
                Metrics = null,
                ExecutionTimeMs = result.ExecutionTimeMs
            });
            
            EndImageProcessing();
        }

        private void buttonRemoveFogUsingCustomMethod_Click(object sender, EventArgs e)
        {
            if (!this.CheckFogRemovalPrerequirements()) return;
            StartImageProcessing();

            var result = _appState.Fog.RemoveFogUsingCustomMethod(_appState.InputImageBgr, new FogRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows), InputImageFileName = _appState.InputImageFileName });
            _appState.SetOutputImage(result.EnhancementResult);
            _appState.SetShadowMaskImage(result.DetectionResult);
            this.DisplayImageInPictureBox(pictureBox3, result.EnhancementResult.Bitmap);
            this.DisplayImageInPictureBox(pictureBox2, result.DetectionResult.Bitmap);

            // save metrics
            _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            {
                ImageFileName = _appState.InputImageFileName,
                EnhanceMethodName = nameof(Fog.RemoveFogUsingCustomMethod),
                Metrics = null,
                ExecutionTimeMs = result.ExecutionTimeMs
            });
            
            EndImageProcessing();
        }

        private void buttonRemoveFogUsingCustomMethodWithDepthEstimation_Click(object sender, EventArgs e)
        {
            if (!this.CheckFogRemovalPrerequirements()) return;

            //var confirmResult = MessageBox.Show("You need to setup secial environment in order to run this method. Are you sure to start it?",
            //                                     "Confirm action",
            //                                     MessageBoxButtons.YesNo);
            //if (confirmResult == DialogResult.No) return;

            StartImageProcessing();

            try
            {
                var result = _appState.Fog.RemoveFogUsingCustomMethodWithDepthEstimation(_appState.InputImageBgr, new FogRemovalParams() { ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows), InputImageFileName = _appState.InputImageFileName });
                _appState.SetOutputImage(result.EnhancementResult);
                _appState.SetShadowMaskImage(result.DetectionResult);
                this.DisplayImageInPictureBox(pictureBox3, result.EnhancementResult.Bitmap);
                this.DisplayImageInPictureBox(pictureBox2, result.DetectionResult.Bitmap);

                // save metrics
                _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
                {
                    ImageFileName = _appState.InputImageFileName,
                    EnhanceMethodName = nameof(Fog.RemoveFogUsingCustomMethodWithDepthEstimation),
                    Metrics = null,
                    ExecutionTimeMs = result.ExecutionTimeMs
                });
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Error occured!\n\n{ex.Message}", "Error");
            }
            
            EndImageProcessing();
        }




        //
        //VISIBILITY ENHANCEMENT
        //
        private void buttonVisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod_Click(object sender, EventArgs e)
        {
            if (!this.CheckDustRemovalPrerequirements()) return;
            StartImageProcessing();

            var _params = new TriThresholdFuzzyIntensificationOperatorsMethodParams();
            if (!String.IsNullOrEmpty(textBoxTriThresholdFuzzyIntensificationOperatorsMethod_Dzeta.Text))
                _params.Dzeta = double.Parse(textBoxTriThresholdFuzzyIntensificationOperatorsMethod_Dzeta.Text);

            var result = _appState.Dust.VisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod(_appState.InputImageBgr, _params);
            _appState.SetOutputImage(result.EnhancementResult);
            _appState.SetShadowMaskImage(result.DetectionResult);
            this.DisplayImageInPictureBox(pictureBox3, result.EnhancementResult.Bitmap);
            this.DisplayImageInPictureBox(pictureBox2, result.DetectionResult.Bitmap);

            // save metrics
            _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            {
                ImageFileName = _appState.InputImageFileName,
                EnhanceMethodName = nameof(Dust.VisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod),
                Metrics = null,
                ExecutionTimeMs = result.ExecutionTimeMs
            });

            GC.Collect();
            EndImageProcessing();
        }
        private void buttonRecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod_Click(object sender, EventArgs e)
        {
            if (!this.CheckDustRemovalPrerequirements()) return;
            StartImageProcessing();

            string kernel = textBox_RatioConstancyMethod_kernel.Text;
            string sigma = textBox_RatioConstancyMethod_sigma.Text;
            string Imin = textBox_RatioConstancyMethod_Imin.Text;
            string Imax = textBox_RatioConstancyMethod_Imax.Text;

            var _params = new RGBResponseRatioConstancyMethodParams();

            if(!String.IsNullOrEmpty(kernel) && !String.IsNullOrEmpty(sigma))
            {
                _params.MeanShiftParams = new MeanShiftClusteringAcordParams()
                {
                    Kernel = int.Parse(kernel),
                    Sigma = double.Parse(sigma)
                };
            }
            if (!String.IsNullOrEmpty(Imin))
            {
                if (double.TryParse(Imin, out double imin)) _params.Imin = imin;

            }
            if (!String.IsNullOrEmpty(Imax))
            {
                if (double.TryParse(Imax, out double imax)) _params.Imax = imax;

            }
            _params.ShowWindows = GetCheckBoxValue(checkBoxShowOptionalWindows);

            var result = _appState.Dust.RecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod(_appState.InputImageBgr, _params);
            _appState.SetOutputImage(result.EnhancementResult);
            _appState.SetShadowMaskImage(result.DetectionResult);
            this.DisplayImageInPictureBox(pictureBox3, result.EnhancementResult.Bitmap);
            this.DisplayImageInPictureBox(pictureBox2, result.DetectionResult.Bitmap);

            // save metrics
            _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            {
                ImageFileName = _appState.InputImageFileName,
                EnhanceMethodName = nameof(Dust.RecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod),
                Metrics = null,
                ExecutionTimeMs = result.ExecutionTimeMs
            });

            GC.Collect();
            EndImageProcessing();
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

            if (!this.CheckBasePrerequirements()) return;

            var result = GammaCorrection.AdaptiveWithBaseResponse(_appState.InputImageBgr, showWindows: GetCheckBoxValue(checkBoxShowOptionalWindows));
            _appState.SetOutputImage(result.EnhancementResult);
            _appState.SetShadowMaskImage(result.DetectionResult);
            this.DisplayImageInPictureBox(pictureBox3, result.EnhancementResult.Bitmap);
            this.DisplayImageInPictureBox(pictureBox2, result.DetectionResult.Bitmap);
        }

        // Run All Methods On All Test Images and save results
        private void buttonRunAllMethods_Click(object sender, EventArgs e)
        {
            var confirmResult = MessageBox.Show("Are you sure to start this task? It can take a lot of time and memory to process!",
                                                "Confirm action",
                                                MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.No) return;

            StartImageProcessing();
            EndImageProcessing();

            //   string imagesPath = @"D:\Google Drive\Diploma5\Images";
            //   string fogImagesPath = @"FogImagesConverted";
            //   string dustImagesPath = @"DustImagesConverted";
            //   string rainImagesPath = @"RainImagesConverted";
            //   string snowImagesPath = @"SnowImagesConverted";

            //   string resultsDestPath = Path.Combine(imagesPath, @"Results");
            //   string fogImagesDestPath = Path.Combine(imagesPath, @"Results", @"Fog");
            //   string dustImagesDestPath = Path.Combine(imagesPath, @"Results", @"Dust");

            //   if (!Directory.Exists(resultsDestPath)) Directory.CreateDirectory(resultsDestPath);
            //   if (!Directory.Exists(fogImagesDestPath)) Directory.CreateDirectory(fogImagesDestPath);
            //   if (!Directory.Exists(dustImagesDestPath)) Directory.CreateDirectory(dustImagesDestPath);


            //   // delete all files in target directories
            //   //foreach (var file in Directory.GetFiles(resultsDestPath))
            //   //{
            //   //   File.Delete(file);
            //   //}
            //   //foreach (var file in Directory.GetFiles(Path.Combine(resultsDestPath, fogImagesDestPath)))
            //   //{
            //   //   File.Delete(file);
            //   //}
            //   //foreach (var file in Directory.GetFiles(Path.Combine(resultsDestPath, dustImagesDestPath)))
            //   //{
            //   //   File.Delete(file);
            //   //}

            //   // Method_[Fog|Dust|Rain|Snow]_OriginFileName_DetailedResultNumber.Extension
            //   const string IMAGE_FILENAME_TEMPLATE = "{0}_{1}_{2}_{3}{4}";

            //   // fog (also test on dust, rain, snow images)
            //   List<string> fogFiles = new List<string>();
            //   fogFiles.AddRange(Directory.EnumerateFiles(Path.Combine(imagesPath, fogImagesPath)));
            //   //fogFiles.AddRange(Directory.EnumerateFiles(Path.Combine(imagesPath, dustImagesPath)));
            //   //fogFiles.AddRange(Directory.EnumerateFiles(Path.Combine(imagesPath, rainImagesPath)));
            //   //fogFiles.AddRange(Directory.EnumerateFiles(Path.Combine(imagesPath, snowImagesPath)));

            //   List<string> dustFiles = new List<string>();
            //   //dustFiles.AddRange(Directory.EnumerateFiles(Path.Combine(imagesPath, fogImagesPath)));
            //    dustFiles.AddRange(Directory.EnumerateFiles(Path.Combine(imagesPath, dustImagesPath)));
            //   // dustFiles.AddRange(Directory.EnumerateFiles(Path.Combine(imagesPath, rainImagesPath)));
            //   //dustFiles.AddRange(Directory.EnumerateFiles(Path.Combine(imagesPath, snowImagesPath)));

            //   int totalFilesToProcess = fogFiles.Count + dustFiles.Count;
            //   int filesProcessed = 0;

            //   Image<Bgr, byte> image;
            //   FogRemovalParams _params;
            //   string method;
            //   BaseMethodResponse result;
            //   string imageType;
            //   IInputArray img;
            //   string imgPath;

            //   // fog
            //   if (this.checkBoxRunAllMethodsFog.Checked)
            //   {
            //       foreach (var imageFileName in fogFiles)
            //       {
            //           if (!File.Exists(imageFileName)) continue;

            //           if (imageFileName.ToLower().Contains("fog")) imageType = "Fog";
            //           else if (imageFileName.ToLower().Contains("dust")) imageType = "Dust";
            //           else if (imageFileName.ToLower().Contains("rain")) imageType = "Rain";
            //           else if (imageFileName.ToLower().Contains("snow")) imageType = "Snow";
            //           else imageType = "Unknown";

            //           image = new Image<Bgr, byte>(imageFileName);
            //           _params = new FogRemovalParams { ShowWindows = false, InputImageFileName = imageFileName };

            //           // DCP
            //           method = this._methodInfoStore.MethodNameMap[nameof(Fog.RemoveFogUsingDarkChannelPrior)];
            //           result = this._appState.Fog.RemoveFogUsingDarkChannelPrior(image, _params);
            //           if (this.checkBoxUpdateStats.Checked)
            //           {
            //               _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            //               {
            //                   ImageFileName = imageFileName,
            //                   EnhanceMethodName = nameof(Fog.RemoveFogUsingDarkChannelPrior),
            //                   Metrics = result.Metrics,
            //                   ExecutionTimeMs = result.ExecutionTimeMs
            //               });
            //           }
            //           for (int i = result.DetailedResults.Count() - 1; i >= 0; i -= 1)
            //           {
            //               if (this.checkBoxRunAllMethodsSaveAllImages.Checked == false)
            //               {
            //                   // save only first ad last image
            //                   if (i != result.DetailedResults.Count() - 1 && i != 0)
            //                   {
            //                       continue;
            //                   }
            //               }
            //               img = result.DetailedResults[i];
            //               imgPath = Path.Combine(fogImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, imageType, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
            //               CvInvoke.Imwrite(imgPath, img);
            //           }

            //           result.EnhancementResult.Dispose();
            //           result.DetectionResult.Dispose();
            //           GC.Collect();

            //           // MCP
            //           method = this._methodInfoStore.MethodNameMap[nameof(Fog.RemoveFogUsingMedianChannelPrior)];
            //           result = this._appState.Fog.RemoveFogUsingMedianChannelPrior(image, _params);
            //           if (this.checkBoxUpdateStats.Checked)
            //           {
            //               _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            //               {
            //                   ImageFileName = imageFileName,
            //                   EnhanceMethodName = nameof(Fog.RemoveFogUsingMedianChannelPrior),
            //                   Metrics = result.Metrics,
            //                   ExecutionTimeMs = result.ExecutionTimeMs
            //               });
            //           }
            //           for (int i = result.DetailedResults.Count() - 1; i >= 0; i -= 1)
            //           {
            //               if (this.checkBoxRunAllMethodsSaveAllImages.Checked == false)
            //               {
            //                   // save only first ad last image
            //                   if (i != result.DetailedResults.Count() - 1 && i != 0)
            //                   {
            //                       continue;
            //                   }
            //               }
            //               img = result.DetailedResults[i];
            //               imgPath = Path.Combine(fogImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, imageType, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
            //               CvInvoke.Imwrite(imgPath, img);
            //           }
            //           result.EnhancementResult.Dispose();
            //           result.DetectionResult.Dispose();
            //           GC.Collect();

            //           // DCP and CLAHE
            //           method = this._methodInfoStore.MethodNameMap[nameof(Fog.RemoveFogUsingIdcpWithClahe)];
            //           result = this._appState.Fog.RemoveFogUsingIdcpWithClahe(image, _params);
            //           if (this.checkBoxUpdateStats.Checked)
            //           {
            //               _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            //               {
            //                   ImageFileName = imageFileName,
            //                   EnhanceMethodName = nameof(Fog.RemoveFogUsingIdcpWithClahe),
            //                   Metrics = result.Metrics,
            //                   ExecutionTimeMs = result.ExecutionTimeMs
            //               });
            //           }
            //           for (int i = result.DetailedResults.Count() - 1; i >= 0; i -= 1)
            //           {
            //               if (this.checkBoxRunAllMethodsSaveAllImages.Checked == false)
            //               {
            //                   // save only first ad last image
            //                   if (i != result.DetailedResults.Count() - 1 && i != 0)
            //                   {
            //                       continue;
            //                   }
            //               }
            //               img = result.DetailedResults[i];
            //               imgPath = Path.Combine(fogImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, imageType, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
            //               CvInvoke.Imwrite(imgPath, img);
            //           }
            //           result.EnhancementResult.Dispose();
            //           result.DetectionResult.Dispose();
            //           GC.Collect();

            //           // DCP and DFT
            //           // method = this._methodInfoStore.MethodNameMap[nameof(Fog.RemoveFogUsingDCPAndDFT)];
            //           //result = this._appState.Fog.RemoveFogUsingDCPAndDFT(image, _params);
            //           //if (this.checkBoxUpdateStats.Checked)
            //           //{
            //           //    _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            //           //    {
            //           //        ImageFileName = imageFileName,
            //           //        EnhanceMethodName = nameof(Fog.RemoveFogUsingDCPAndDFT),
            //           //        Metrics = result.Metrics,
            //           //        ExecutionTimeMs = result.ExecutionTimeMs
            //           //    });
            //           //}
            //           //for (int i = result.DetailedResults.Count() - 1; i >= 0; i -= 1)
            //           //{
            //           //if (this.checkBoxRunAllMethodsSaveAllImages.Checked == false)
            //           //{
            //           //    // save only first ad last image
            //           //    if (i != result.DetailedResults.Count() - 1 && i != 0)
            //           //    {
            //           //        continue;
            //           //    }
            //           //}
            //           //    img = result.DetailedResults[i];
            //           //    imgPath = Path.Combine(fogImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, imageType, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
            //           //    CvInvoke.Imwrite(imgPath, img);
            //           //}
            //           //result.EnhancementResult.Dispose();
            //           //result.DetectionResult.Dispose();
            //           //GC.Collect();

            //           // MultiCoreDSP
            //           method = this._methodInfoStore.MethodNameMap[nameof(Fog.RemoveFogUsingMultiCoreDSPMethod)];
            //           result = this._appState.Fog.RemoveFogUsingMultiCoreDSPMethod(image, _params);
            //           if (this.checkBoxUpdateStats.Checked)
            //           {
            //               _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            //               {
            //                   ImageFileName = imageFileName,
            //                   EnhanceMethodName = nameof(Fog.RemoveFogUsingMultiCoreDSPMethod),
            //                   Metrics = result.Metrics,
            //                   ExecutionTimeMs = result.ExecutionTimeMs
            //               });
            //           }
            //           for (int i = result.DetailedResults.Count() - 1; i >= 0; i -= 1)
            //           {
            //               if (this.checkBoxRunAllMethodsSaveAllImages.Checked == false)
            //               {
            //                   // save only first ad last image
            //                   if (i != result.DetailedResults.Count() - 1 && i != 0)
            //                   {
            //                       continue;
            //                   }
            //               }
            //               img = result.DetailedResults[i];
            //               imgPath = Path.Combine(fogImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, imageType, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
            //               CvInvoke.Imwrite(imgPath, img);
            //           }
            //           result.EnhancementResult.Dispose();
            //           result.DetectionResult.Dispose();
            //           GC.Collect();

            //           // RobbyTanMethodForRoads
            //           // method = this._methodInfoStore.MethodNameMap[nameof(Fog.EnhaceVisibilityUsingRobbyTanMethodForRoads)];
            //           //result = this._appState.Fog.EnhaceVisibilityUsingRobbyTanMethodForRoads(image, _params);
            //           //if (this.checkBoxUpdateStats.Checked)
            //           //{
            //           //    _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            //           //    {
            //           //        ImageFileName = imageFileName,
            //           //        EnhanceMethodName = nameof(Fog.EnhaceVisibilityUsingRobbyTanMethodForRoads),
            //           //        Metrics = result.Metrics,
            //           //        ExecutionTimeMs = result.ExecutionTimeMs
            //           //    });
            //           //}
            //           //for (int i = result.DetailedResults.Count() - 1; i >= 0; i -= 1)
            //           //{
            //           //if (this.checkBoxRunAllMethodsSaveAllImages.Checked == false)
            //           //{
            //           //    // save only first ad last image
            //           //    if (i != result.DetailedResults.Count() - 1 && i != 0)
            //           //    {
            //           //        continue;
            //           //    }
            //           //}
            //           //    img = result.DetailedResults[i];
            //           //    imgPath = Path.Combine(fogImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, imageType, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
            //           //    CvInvoke.Imwrite(imgPath, img);
            //           //}
            //           //result.EnhancementResult.Dispose();
            //           //result.DetectionResult.Dispose();
            //           //GC.Collect();

            //           // Custom
            //           method = this._methodInfoStore.MethodNameMap[nameof(Fog.RemoveFogUsingCustomMethod)];
            //           result = this._appState.Fog.RemoveFogUsingCustomMethod(image, _params);
            //           if (this.checkBoxUpdateStats.Checked)
            //           {
            //               _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            //               {
            //                   ImageFileName = imageFileName,
            //                   EnhanceMethodName = nameof(Fog.RemoveFogUsingCustomMethod),
            //                   Metrics = result.Metrics,
            //                   ExecutionTimeMs = result.ExecutionTimeMs
            //               });
            //           }
            //           for (int i = result.DetailedResults.Count() - 1; i >= 0; i -= 1)
            //           {
            //               if (this.checkBoxRunAllMethodsSaveAllImages.Checked == false)
            //               {
            //                   // save only first ad last image
            //                   if (i != result.DetailedResults.Count() - 1 && i != 0)
            //                   {
            //                       continue;
            //                   }
            //               }
            //               img = result.DetailedResults[i];
            //               imgPath = Path.Combine(fogImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, imageType, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
            //               CvInvoke.Imwrite(imgPath, img);
            //           }
            //           result.EnhancementResult.Dispose();
            //           result.DetectionResult.Dispose();
            //           GC.Collect();

            //           //// RemoveFogUsingCustomMethodWithDepthEstimation
            //           //try
            //           //{
            //           //    method = this._methodInfoStore.MethodNameMap[nameof(Fog.RemoveFogUsingCustomMethodWithDepthEstimation)];
            //           //    result = this._appState.Fog.RemoveFogUsingCustomMethodWithDepthEstimation(image, _params);
            //           //    if (this.checkBoxUpdateStats.Checked)
            //           //    {
            //           //        _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            //           //        {
            //           //            ImageFileName = imageFileName,
            //           //            EnhanceMethodName = nameof(Fog.RemoveFogUsingCustomMethodWithDepthEstimation),
            //           //            Metrics = result.Metrics,
            //           //            ExecutionTimeMs = result.ExecutionTimeMs
            //           //        });
            //           //    }
            //           //    for (int i = result.DetailedResults.Count() - 1; i >= 0; i -= 1)
            //           //    {
            //           //        if (this.checkBoxRunAllMethodsSaveAllImages.Checked == false)
            //           //        {
            //           //            // save only first ad last image
            //           //            if (i != result.DetailedResults.Count() - 1 && i != 0)
            //           //            {
            //           //                continue;
            //           //            }
            //           //        }
            //           //        img = result.DetailedResults[i];
            //           //        imgPath = Path.Combine(fogImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, imageType, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
            //           //        CvInvoke.Imwrite(imgPath, img);
            //           //    }
            //           //    result.EnhancementResult.Dispose();
            //           //    result.DetectionResult.Dispose();
            //           //    GC.Collect();
            //           //}
            //           //catch(Exception ex)
            //           //{
            //           //    // skip
            //           //}

            //           // update counter
            //           filesProcessed += 1;

            //           image.Dispose();
            //           GC.Collect();

            //           //long maxMemory = 2 * 1024 * 1024 * 1024; // 4 Gb
            //           // long memoryBytes = GC.GetTotalMemory(true);
            //       }
            //   }

            //   // dust (also test on fog, rain, snow images)
            //   if (this.checkBoxRunAllMethodsDust.Checked)
            //   {
            //       foreach (var imageFileName in dustFiles)
            //       {
            //           if (imageFileName.ToLower().Contains("fog")) imageType = "Fog";
            //           else if (imageFileName.ToLower().Contains("dust")) imageType = "Dust";
            //           else if (imageFileName.ToLower().Contains("rain")) imageType = "Rain";
            //           else if (imageFileName.ToLower().Contains("snow")) imageType = "Snow";
            //           else imageType = "Unknown";

            //           image = new Image<Bgr, byte>(imageFileName);

            //           // FuzzyOperators
            //           method = this._methodInfoStore.MethodNameMap[nameof(Dust.VisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod)];
            //           var result2 = this._appState.Dust.VisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod(image, new TriThresholdFuzzyIntensificationOperatorsMethodParams { Dzeta = null });
            //           if (this.checkBoxUpdateStats.Checked)
            //           {
            //               _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            //               {
            //                   ImageFileName = imageFileName,
            //                   EnhanceMethodName = nameof(Dust.VisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod),
            //                   Metrics = result2.Metrics,
            //                   ExecutionTimeMs = result2.ExecutionTimeMs
            //               });
            //           }
            //           for (int i = result2.DetailedResults.Count() - 1; i >= 0; i -= 1)
            //           {
            //               if (this.checkBoxRunAllMethodsSaveAllImages.Checked == false)
            //               {
            //                   // save only first ad last image
            //                   if (i != result2.DetailedResults.Count() - 1 && i != 0)
            //                   {
            //                       continue;
            //                   }
            //               }
            //               img = result2.DetailedResults[i];
            //               imgPath = Path.Combine(dustImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, imageType, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
            //               CvInvoke.Imwrite(imgPath, img);
            //           }
            //           result2.EnhancementResult.Dispose();
            //           result2.DetectionResult.Dispose();
            //           GC.Collect();

            //           // RGBResponseRatioConstancy
            //           try
            //           {
            //               method = this._methodInfoStore.MethodNameMap[nameof(Dust.RecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod)];
            //               result2 = this._appState.Dust.RecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod(image, new RGBResponseRatioConstancyMethodParams { ShowWindows = false });
            //               if (this.checkBoxUpdateStats.Checked)
            //               {
            //                   _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            //                   {
            //                       ImageFileName = imageFileName,
            //                       EnhanceMethodName = nameof(Dust.RecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod),
            //                       Metrics = result2.Metrics,
            //                       ExecutionTimeMs = result2.ExecutionTimeMs
            //                   });
            //               }
            //               for (int i = result2.DetailedResults.Count() - 1; i >= 0; i -= 1)
            //               {
            //                   if (this.checkBoxRunAllMethodsSaveAllImages.Checked == false)
            //                   {
            //                       // save only first ad last image
            //                       if (i != result2.DetailedResults.Count() - 1 && i != 0)
            //                       {
            //                           continue;
            //                       }
            //                   }
            //                   img = result2.DetailedResults[i];
            //                   imgPath = Path.Combine(dustImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, imageType, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
            //                   CvInvoke.Imwrite(imgPath, img);
            //               }
            //               result2.EnhancementResult.Dispose();
            //               result2.DetectionResult.Dispose();
            //               GC.Collect();
            //           }
            //           catch (Exception ex)
            //           {
            //               //Console.Error.WriteLine(ex.Message);
            //           }


            //           // RobbyTanMethodForRoads
            //           method = this._methodInfoStore.MethodNameMap[nameof(Fog.EnhaceVisibilityUsingRobbyTanMethodForRoads)];
            //           result2 = this._appState.Fog.EnhaceVisibilityUsingRobbyTanMethodForRoads(image, new FogRemovalParams() { ShowWindows = false, InputImageFileName = imageFileName });
            //           if (this.checkBoxUpdateStats.Checked)
            //           {
            //               _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            //               {
            //                   ImageFileName = imageFileName,
            //                   EnhanceMethodName = nameof(Fog.EnhaceVisibilityUsingRobbyTanMethodForRoads),
            //                   Metrics = result2.Metrics,
            //                   ExecutionTimeMs = result2.ExecutionTimeMs
            //               });
            //           }
            //           for (int i = result2.DetailedResults.Count() - 1; i >= 0; i -= 1)
            //           {
            //               if (this.checkBoxRunAllMethodsSaveAllImages.Checked == false)
            //               {
            //                   // save only first ad last image
            //                   if (i != result2.DetailedResults.Count() - 1 && i != 0)
            //                   {
            //                       continue;
            //                   }
            //               }
            //               img = result2.DetailedResults[i];
            //               imgPath = Path.Combine(dustImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, imageType, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
            //               CvInvoke.Imwrite(imgPath, img);
            //           }
            //           result2.EnhancementResult.Dispose();
            //           result2.DetectionResult.Dispose();
            //           GC.Collect();

            //           // update counter
            //           filesProcessed += 1;

            //           image.Dispose();
            //           GC.Collect();
            //       }
            //   }

            //MessageBox.Show("Done");
        }

        // Compute metrics for test images dataset
        private void buttonComputeMetrics_Click(object sender, EventArgs e)
        {
            // show alert about long execution time
            var confirmResult = MessageBox.Show("Are you sure to start this task? It can take a lot of time and memory to process!",
                                                 "Confirm action",
                                                 MessageBoxButtons.YesNo);
            if (confirmResult == DialogResult.No) return;

            StartImageProcessing();

            // use frida dataset
            string fogDatasetPath = Path.Combine(Directory.GetCurrentDirectory(), @"Datasets\frida");
            string saveResultsPath = Path.Combine(Directory.GetCurrentDirectory(), @"Datasets");

            string resultsDestPath = Path.Combine(saveResultsPath, @"Results");
            string fogImagesDestPath = Path.Combine(saveResultsPath, @"Results", @"Fog");
            string dustImagesDestPath = Path.Combine(saveResultsPath, @"Results", @"Dust");

            if (!Directory.Exists(fogDatasetPath))
            {
                MessageBox.Show($"Can't find fog images dataset in {fogDatasetPath}. Stopping!");
                return;
            }

            if (!Directory.Exists(resultsDestPath)) Directory.CreateDirectory(resultsDestPath);
            if (!Directory.Exists(fogImagesDestPath)) Directory.CreateDirectory(fogImagesDestPath);
            if (!Directory.Exists(dustImagesDestPath)) Directory.CreateDirectory(dustImagesDestPath);

            // delete all files in target directories
            foreach (var file in Directory.GetFiles(resultsDestPath))
            {
                File.Delete(file);
            }
            foreach (var file in Directory.GetFiles(Path.Combine(resultsDestPath, fogImagesDestPath)))
            {
                File.Delete(file);
            }
            foreach (var file in Directory.GetFiles(Path.Combine(resultsDestPath, dustImagesDestPath)))
            {
                File.Delete(file);
            }

            // frida - each image without fog is associated 4 foggy images and a depthmap
            // 4 fog types - uniform fog, heterogeneous fog, cloudy fog, and cloudy heterogeneous fog
            string groundTruthImgPrefix = "LIma";
            string uniformFogImgPrefix = "U080"; // +
            string heterogeneousFogImgPrefix = "K080";
            string cloudyFogImgPrefix = "L080"; // +
            string cloudyHeterogeneousFogImgPrefix = "M080";
            string imgExtension = "png";
            string depthMapPrefix = "Dmap-";
            string depthMapExtension = "Dmap-";
            char separator = '-';

            // set types of fog we use it test
            List<string> testFogTypes = new List<string>();
            if (this.checkBoxUniformFog.Checked)
            {
                testFogTypes.Add(uniformFogImgPrefix);
            }
            if (this.checkBoxHeterogeneousFog.Checked)
            {
                testFogTypes.Add(heterogeneousFogImgPrefix);
            }
            if (this.checkBoxCloudyFog.Checked)
            {
                testFogTypes.Add(cloudyFogImgPrefix);
            }
            if (this.checkBoxCloudyHeterogeneousFog.Checked)
            {
                testFogTypes.Add(cloudyHeterogeneousFogImgPrefix);
            }
            // default types
            if(testFogTypes.Count() == 0)
            {
                testFogTypes.Add(uniformFogImgPrefix);
                testFogTypes.Add(cloudyFogImgPrefix);
            }

            // Template = Method_OriginFileName_DetailedResultNumber.Extension
            const string IMAGE_FILENAME_TEMPLATE = "{0}_{1}_{2}_{3}";

            List<string> allFiles = new List<string>();
            allFiles.AddRange(Directory.EnumerateFiles(Path.Combine(fogDatasetPath)));
            List<string> groundTruthImageFiles = allFiles.Where(x => (new Regex($"{groundTruthImgPrefix}{separator}\\d+.{imgExtension}")).IsMatch(x)).ToList();
            groundTruthImageFiles = groundTruthImageFiles.Where(x => File.Exists(x)).ToList();

            object _lock = new object();
            int totalCount = groundTruthImageFiles.Count();
            int processedCount = 0;

            groundTruthImageFiles.AsParallel().ForAll((groundTruthImageFile) =>
            {
                Image<Bgr, byte> groundTruthImage;
                Image<Bgr, byte> image;
                FogRemovalParams _params;
                string method;
                BaseMethodResponse result;
                BaseMethodResponse result2;
                IInputArray img;
                const bool saveAllImages = false;
                string imgPath;

                groundTruthImage = new Image<Bgr, byte>(groundTruthImageFile);

                for (int n = 0; n < testFogTypes.Count(); n++)
                {
                    string testFogType = testFogTypes[n];
                    string imageNumber = Path.GetFileNameWithoutExtension(groundTruthImageFile).Split(separator)[1];
                    string imageFileName = $"{testFogType}{separator}{imageNumber}.{imgExtension}";
                    imageFileName = Path.Combine(fogDatasetPath, imageFileName);

                    if (!File.Exists(imageFileName)) continue;

                    image = new Image<Bgr, byte>(imageFileName);
                    _params = new FogRemovalParams { ShowWindows = false, InputImageFileName = imageFileName };

                    // DCP
                    method = this._methodInfoStore.MethodNameMap[nameof(Fog.RemoveFogUsingDarkChannelPrior)];
                    result = this._appState.Fog.RemoveFogUsingDarkChannelPrior(image, _params);
                    _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
                    {
                        ImageFileName = imageFileName,
                        EnhanceMethodName = nameof(Fog.RemoveFogUsingDarkChannelPrior),
                        Metrics = ImageMetricHelper.ComputeAll(image, result.EnhancementResult),
                        ExecutionTimeMs = result.ExecutionTimeMs
                    });
                    if (this.checkBox1ComputeMetricsSaveImages.Checked)
                    {
                        result.DetailedResults.Insert(0, groundTruthImage);
                        for (int i = result.DetailedResults.Count() - 1; i >= 0; i -= 1)
                        {
                            if (!saveAllImages && i != result.DetailedResults.Count() - 1 && i != 0 && i != 1)
                            {
                                continue;
                            }
                            img = result.DetailedResults[i];
                            imgPath = Path.Combine(fogImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
                            CvInvoke.Imwrite(imgPath, img);
                        }
                    }
                    result.EnhancementResult.Dispose();
                    result.DetectionResult.Dispose();
                    GC.Collect();

                    // MCP
                    method = this._methodInfoStore.MethodNameMap[nameof(Fog.RemoveFogUsingMedianChannelPrior)];
                    result = this._appState.Fog.RemoveFogUsingMedianChannelPrior(image, _params);
                    _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
                    {
                        ImageFileName = imageFileName,
                        EnhanceMethodName = nameof(Fog.RemoveFogUsingMedianChannelPrior),
                        Metrics = ImageMetricHelper.ComputeAll(image, result.EnhancementResult),
                        ExecutionTimeMs = result.ExecutionTimeMs
                    });
                    if (this.checkBox1ComputeMetricsSaveImages.Checked)
                    {
                        result.DetailedResults.Insert(0, groundTruthImage);
                        for (int i = result.DetailedResults.Count() - 1; i >= 0; i -= 1)
                        {
                            if (!saveAllImages && i != result.DetailedResults.Count() - 1 && i != 0 && i != 1)
                            {
                                continue;
                            }
                            img = result.DetailedResults[i];
                            imgPath = Path.Combine(fogImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
                            CvInvoke.Imwrite(imgPath, img);
                        }
                    }
                    
                    result.EnhancementResult.Dispose();
                    result.DetectionResult.Dispose();
                    GC.Collect();

                    // DCP & CLAHE
                    method = this._methodInfoStore.MethodNameMap[nameof(Fog.RemoveFogUsingIdcpWithClahe)];
                    result = this._appState.Fog.RemoveFogUsingIdcpWithClahe(image, _params);
                    _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
                    {
                        ImageFileName = imageFileName,
                        EnhanceMethodName = nameof(Fog.RemoveFogUsingIdcpWithClahe),
                        Metrics = ImageMetricHelper.ComputeAll(image, result.EnhancementResult),
                        ExecutionTimeMs = result.ExecutionTimeMs
                    });
                    if (this.checkBox1ComputeMetricsSaveImages.Checked)
                    {
                        result.DetailedResults.Insert(0, groundTruthImage);
                        for (int i = result.DetailedResults.Count() - 1; i >= 0; i -= 1)
                        {
                            if (!saveAllImages && i != result.DetailedResults.Count() - 1 && i != 0 && i != 1)
                            {
                                continue;
                            }
                            img = result.DetailedResults[i];
                            imgPath = Path.Combine(fogImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
                            CvInvoke.Imwrite(imgPath, img);
                        }
                    }
                    result.EnhancementResult.Dispose();
                    result.DetectionResult.Dispose();
                    GC.Collect();

                    // DCP&DFT
                    method = this._methodInfoStore.MethodNameMap[nameof(Fog.RemoveFogUsingDCPAndDFT)];
                    result = this._appState.Fog.RemoveFogUsingDCPAndDFT(image, _params);
                    _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
                    {
                        ImageFileName = imageFileName,
                        EnhanceMethodName = nameof(Fog.RemoveFogUsingMultiCoreDSPMethod),
                        Metrics = ImageMetricHelper.ComputeAll(image, result.EnhancementResult),
                        ExecutionTimeMs = result.ExecutionTimeMs
                    });
                    if (this.checkBox1ComputeMetricsSaveImages.Checked)
                    {
                        result.DetailedResults.Insert(0, groundTruthImage);
                        for (int i = result.DetailedResults.Count() - 1; i >= 0; i -= 1)
                        {
                            if (!saveAllImages && i != result.DetailedResults.Count() - 1 && i != 0 && i != 1)
                            {
                                continue;
                            }
                            img = result.DetailedResults[i];
                            imgPath = Path.Combine(fogImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
                            CvInvoke.Imwrite(imgPath, img);
                        }
                    }
                    result.EnhancementResult.Dispose();
                    result.DetectionResult.Dispose();
                    GC.Collect();

                    // DSP
                    method = this._methodInfoStore.MethodNameMap[nameof(Fog.RemoveFogUsingMultiCoreDSPMethod)];
                    result = this._appState.Fog.RemoveFogUsingMultiCoreDSPMethod(image, _params);
                    _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
                    {
                        ImageFileName = imageFileName,
                        EnhanceMethodName = nameof(Fog.RemoveFogUsingMultiCoreDSPMethod),
                        Metrics = ImageMetricHelper.ComputeAll(image, result.EnhancementResult),
                        ExecutionTimeMs = result.ExecutionTimeMs
                    });
                    if (this.checkBox1ComputeMetricsSaveImages.Checked)
                    {
                        result.DetailedResults.Insert(0, groundTruthImage);
                        for (int i = result.DetailedResults.Count() - 1; i >= 0; i -= 1)
                        {
                            if (!saveAllImages && i != result.DetailedResults.Count() - 1 && i != 0 && i != 1)
                            {
                                continue;
                            }
                            img = result.DetailedResults[i];
                            imgPath = Path.Combine(fogImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
                            CvInvoke.Imwrite(imgPath, img);
                        }
                    }
                    result.EnhancementResult.Dispose();
                    result.DetectionResult.Dispose();
                    GC.Collect();

                    // CUS
                    method = this._methodInfoStore.MethodNameMap[nameof(Fog.RemoveFogUsingCustomMethod)];
                    result = this._appState.Fog.RemoveFogUsingCustomMethod(image, _params);
                    _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
                    {
                        ImageFileName = imageFileName,
                        EnhanceMethodName = nameof(Fog.RemoveFogUsingCustomMethod),
                        Metrics = ImageMetricHelper.ComputeAll(image, result.EnhancementResult),
                        ExecutionTimeMs = result.ExecutionTimeMs
                    });
                    if (this.checkBox1ComputeMetricsSaveImages.Checked)
                    {
                        result.DetailedResults.Insert(0, groundTruthImage);
                        for (int i = result.DetailedResults.Count() - 1; i >= 0; i -= 1)
                        {
                            if (!saveAllImages && i != result.DetailedResults.Count() - 1 && i != 0 && i != 1)
                            {
                                continue;
                            }
                            img = result.DetailedResults[i];
                            imgPath = Path.Combine(fogImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
                            CvInvoke.Imwrite(imgPath, img);
                        }
                    }
                    result.EnhancementResult.Dispose();
                    result.DetectionResult.Dispose();
                    GC.Collect();

                    // CUSD
                    //try
                    //{
                    //    method = this._methodInfoStore.MethodNameMap[nameof(Fog.RemoveFogUsingCustomMethodWithDepthEstimation)];
                    //    result = this._appState.Fog.RemoveFogUsingCustomMethodWithDepthEstimation(image, _params);
                    //    _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
                    //    {
                    //        ImageFileName = imageFileName,
                    //        EnhanceMethodName = nameof(Fog.RemoveFogUsingCustomMethodWithDepthEstimation),
                    //        Metrics = ImageMetricHelper.ComputeAll(image, result.EnhancementResult),
                    //        ExecutionTimeMs = result.ExecutionTimeMs
                    //    });
                    //    if (this.checkBox1ComputeMetricsSaveImages.Checked)
                    //    {
                    //        result.DetailedResults.Insert(0, groundTruthImage);
                    //        for (int i = result.DetailedResults.Count() - 1; i >= 0; i -= 1)
                    //        {
                    //            if (!saveAllImages && i != result.DetailedResults.Count() - 1 && i != 0 && i != 1)
                    //            {
                    //                continue;
                    //            }
                    //            img = result.DetailedResults[i];
                    //            imgPath = Path.Combine(fogImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
                    //            CvInvoke.Imwrite(imgPath, img);
                    //        }
                    //    }
                    //    result.EnhancementResult.Dispose();
                    //    result.DetectionResult.Dispose();
                    //    GC.Collect();
                    //}
                    //catch (Exception ex)
                    //{
                    //    // skip
                    //}

                    if (checkBoxTestDustMethods.Checked)
                    {
                        // TTFIO
                        method = this._methodInfoStore.MethodNameMap[nameof(Dust.VisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod)];
                        result2 = this._appState.Dust.VisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod(image, new TriThresholdFuzzyIntensificationOperatorsMethodParams { Dzeta = null, ShowWindows = false, ImageGT = groundTruthImage });
                        _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
                        {
                            ImageFileName = imageFileName,
                            EnhanceMethodName = nameof(Dust.VisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod),
                            Metrics = ImageMetricHelper.ComputeAll(image, result.EnhancementResult),
                            ExecutionTimeMs = result2.ExecutionTimeMs
                        });
                        if (this.checkBox1ComputeMetricsSaveImages.Checked)
                        {
                            result2.DetailedResults.Insert(0, groundTruthImage);
                            for (int i = result2.DetailedResults.Count() - 1; i >= 0; i -= 1)
                            {
                                if (!saveAllImages && i != result2.DetailedResults.Count() - 1 && i != 0 && i != 1)
                                {
                                    continue;
                                }
                                img = result2.DetailedResults[i];
                                imgPath = Path.Combine(dustImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
                                CvInvoke.Imwrite(imgPath, img);
                            }
                        }
                        result2.EnhancementResult.Dispose();
                        result2.DetectionResult.Dispose();
                        GC.Collect();

                        // RGBRESP
                        try
                        {
                            method = this._methodInfoStore.MethodNameMap[nameof(Dust.RecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod)];
                            result2 = this._appState.Dust.RecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod(image, new RGBResponseRatioConstancyMethodParams { ShowWindows = false, ImageGT = groundTruthImage });
                            _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
                            {
                                ImageFileName = imageFileName,
                                EnhanceMethodName = nameof(Dust.RecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod),
                                Metrics = ImageMetricHelper.ComputeAll(image, result.EnhancementResult),
                                ExecutionTimeMs = result2.ExecutionTimeMs
                            });
                            if (this.checkBox1ComputeMetricsSaveImages.Checked)
                            {
                                result2.DetailedResults.Insert(0, groundTruthImage);
                                for (int i = result2.DetailedResults.Count() - 1; i >= 0; i -= 1)
                                {
                                    if (!saveAllImages && i != result2.DetailedResults.Count() - 1 && i != 0 && i != 1)
                                    {
                                        continue;
                                    }
                                    img = result2.DetailedResults[i];
                                    imgPath = Path.Combine(dustImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
                                    CvInvoke.Imwrite(imgPath, img);
                                }
                            }
                            result2.EnhancementResult.Dispose();
                            result2.DetectionResult.Dispose();
                            GC.Collect();
                        }
                        catch (Exception ex)
                        {
                            // skip
                        }

                        // RTFR
                        method = this._methodInfoStore.MethodNameMap[nameof(Fog.EnhaceVisibilityUsingRobbyTanMethodForRoads)];
                        result2 = this._appState.Fog.EnhaceVisibilityUsingRobbyTanMethodForRoads(image, new FogRemovalParams() { ShowWindows = false, InputImageFileName = imageFileName });
                        _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
                        {
                            ImageFileName = imageFileName,
                            EnhanceMethodName = nameof(Fog.EnhaceVisibilityUsingRobbyTanMethodForRoads),
                            Metrics = ImageMetricHelper.ComputeAll(image, result.EnhancementResult),
                            ExecutionTimeMs = result2.ExecutionTimeMs
                        });
                        if (this.checkBox1ComputeMetricsSaveImages.Checked)
                        {
                            result2.DetailedResults.Insert(0, groundTruthImage);
                            for (int i = result2.DetailedResults.Count() - 1; i >= 0; i -= 1)
                            {
                                if (!saveAllImages && i != result2.DetailedResults.Count() - 1 && i != 0 && i != 1)
                                {
                                    continue;
                                }
                                img = result2.DetailedResults[i];
                                imgPath = Path.Combine(dustImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
                                CvInvoke.Imwrite(imgPath, img);
                            }
                        }
                        result2.EnhancementResult.Dispose();
                        result2.DetectionResult.Dispose();
                        GC.Collect();
                    }

                }

                lock (_lock)
                {
                    processedCount += 1;

                    // when all processed
                    if(processedCount == totalCount)
                    {
                        EndImageProcessing();
                        MessageBox.Show($"Results (images) saved to {resultsDestPath}", "Done");
                    }
                }
            });
        }

        private void buttonTestVideo_Click(object sender, EventArgs e)
        {
            //int Time_millisecounds = 10 * 1000;
            //int MAX_IMAGES = 1000000;
            //List<Image<Bgr, Byte>> image_array = new List<Image<Bgr, Byte>>();
            //System.Diagnostics.Stopwatch SW = new System.Diagnostics.Stopwatch();

            //bool Reading = true;
            //string path = "Videos/Thick fog airplane cockpit view (1).mp4";
            //Emgu.CV.Capture _capture = new Emgu.CV.Capture(Path.Combine(path));
            //SW.Start();

            //while (Reading)
            //{
            //    Mat frame_ = _capture.QueryFrame();

            //    if (frame_ != null)
            //    {
            //        Image<Bgr, Byte> frame = new Image<Bgr, byte>(frame_.Bitmap);
            //        image_array.Add(frame);
            //        if (image_array.Count() >= MAX_IMAGES) Reading = false;
            //    }
            //    else
            //    {
            //        Reading = false;
            //    }
            //}
            //SW.Stop();
            //GC.Collect();

            //// defog
            //for (int i = 0; i < image_array.Count; i++)
            //{
            //    image_array[i] = DehazeDarkChannel.Dehaze_Image(image_array[i]);
            //    if (i % 10 == 0)
            //    {
            //        GC.Collect();
            //    }
            //}
            //GC.Collect();

            //// write new video
            //var vw = new VideoWriter(Path.Combine(@"Videos\result.avi"), 30, new Size(_capture.Width, _capture.Height), true);
            //_capture.Dispose();
            //for (int i = 0; i < image_array.Count; i++)
            //{
            //    vw.Write(image_array[i].Mat);
            //    image_array[i].Dispose();
            //}
            //vw.Dispose();

            //
            // REFACTORE
            List<Image<Bgr, Byte>> image_array = new List<Image<Bgr, Byte>>();
            string path = @"Videos/Pakistan Railway Train EMERGE FROM DENSE Heavy Fog Weather COMPILATION.mp4";
            Emgu.CV.Capture _capture = new Emgu.CV.Capture(Path.Combine(path));
            var vw = new VideoWriter(Path.Combine($"Videos/{Path.GetFileNameWithoutExtension(path)}_result.avi"), 30, new Size(_capture.Width, _capture.Height), true);
            int MAX_IMAGES = 10000000;

            for (int i = 0; true; i++)
            {
                Mat frame_ = _capture.QueryFrame();

                if (frame_ != null)
                {
                    Image<Bgr, Byte> frame = new Image<Bgr, byte>(frame_.Bitmap);
                    frame = DehazeDarkChannel.Dehaze_Image(frame);
                    vw.Write(frame.Mat);
                    frame.Dispose();
                    Console.WriteLine(i);
                    if(i % 20 == 0)
                    {
                        GC.Collect();
                    }
                    if(i >= MAX_IMAGES)
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            _capture.Dispose();
            vw.Dispose();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
