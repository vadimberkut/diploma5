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

        private void DisplayImageMetrics(double executionTimeMs, MetricsResult data)
        {
            this.textBoxMEthodExecTime.Text = executionTimeMs.ToString();
            this.textBoxAdMetric.Text = data.AD.ToString();
            this.textBoxFvmMetric.Text = data.FVM.ToString();
            this.textBoxMseMetric.Text = data.MSE.ToString();
            this.textBoxNaeMetric.Text = data.NAE.ToString();
            this.textBoxScMetric.Text = data.SC.ToString();
            this.textBoxPsnrMEtric.Text = data.PSNR.ToString();
            this.textBoxRMSMetric.Text = data.RMS.ToString();
            this.textBoxRMSMetricDiff.Text = data.RMSDiff.ToString();
            this.textBoxShannonEntropy.Text = data.ShannonEntropy.ToString();
            this.textBoxShannonEntropyDiff.Text = data.ShannonEntropyDiff.ToString();
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
                _appState.SetInputImage(image2);
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

        // Export metrics to CSV
        private void exportMetricsToCSVToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string folderPath = "";
            this._methodInfoStore.SaveToCsv(folderPath);
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
                Metrics = result.Metrics,
                ExecutionTimeMs = result.ExecutionTimeMs
            });
            DisplayImageMetrics(result.ExecutionTimeMs, result.Metrics);
        }

        private void buttonRobbyTanFogRemovalMethod_Click(object sender, EventArgs e)
        {
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
                Metrics = result.Metrics,
                ExecutionTimeMs = result.ExecutionTimeMs
            });
            DisplayImageMetrics(result.ExecutionTimeMs, result.Metrics);
        }

        private void buttonRemoveFogUsingMedianChannelPrior_Click(object sender, EventArgs e)
        {
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
                Metrics = result.Metrics,
                ExecutionTimeMs = result.ExecutionTimeMs
            });
            DisplayImageMetrics(result.ExecutionTimeMs, result.Metrics);
        }

        private void buttonRemoveFogUsingIdcpWithClahe_Click(object sender, EventArgs e)
        {
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
                Metrics = result.Metrics,
                ExecutionTimeMs = result.ExecutionTimeMs
            });
            DisplayImageMetrics(result.ExecutionTimeMs, result.Metrics);
        }

        private void buttonEnhaceVisibilityUsingRobbyTanMethodForRoads_Click(object sender, EventArgs e)
        {
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
                Metrics = result.Metrics,
                ExecutionTimeMs = result.ExecutionTimeMs
            });
            DisplayImageMetrics(result.ExecutionTimeMs, result.Metrics);
        }

        private void buttonRemoveFogUsingDCPAndDFT_Click(object sender, EventArgs e)
        {
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
                Metrics = result.Metrics,
                ExecutionTimeMs = result.ExecutionTimeMs
            });
            DisplayImageMetrics(result.ExecutionTimeMs, result.Metrics);
        }

        private void buttonRemoveFogUsingLocalExtremaMethod_Click(object sender, EventArgs e)
        {
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
                Metrics = result.Metrics,
                ExecutionTimeMs = result.ExecutionTimeMs
            });
            DisplayImageMetrics(result.ExecutionTimeMs, result.Metrics);
        }

        private void buttonRemoveFogUsingPhysicsBasedMethod_Click(object sender, EventArgs e)
        {
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
                Metrics = result.Metrics,
                ExecutionTimeMs = result.ExecutionTimeMs
            });
            DisplayImageMetrics(result.ExecutionTimeMs, result.Metrics);
        }

        private void buttonRemoveFogUsingMultiCoreDSPMethod_Click(object sender, EventArgs e)
        {
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
                Metrics = result.Metrics,
                ExecutionTimeMs = result.ExecutionTimeMs
            });
            DisplayImageMetrics(result.ExecutionTimeMs, result.Metrics);
        }

        private void buttonRemoveFogUsingCustomMethod_Click(object sender, EventArgs e)
        {
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
                Metrics = result.Metrics,
                ExecutionTimeMs = result.ExecutionTimeMs
            });
            DisplayImageMetrics(result.ExecutionTimeMs, result.Metrics);
        }

        private void buttonRemoveFogUsingCustomMethodWithDepthEstimation_Click(object sender, EventArgs e)
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
                Metrics = result.Metrics,
                ExecutionTimeMs = result.ExecutionTimeMs
            });
            DisplayImageMetrics(result.ExecutionTimeMs, result.Metrics);
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
            _appState.SetOutputImage(result.EnhancementResult);
            _appState.SetShadowMaskImage(result.DetectionResult);
            this.DisplayImageInPictureBox(pictureBox3, result.EnhancementResult.Bitmap);
            this.DisplayImageInPictureBox(pictureBox2, result.DetectionResult.Bitmap);

            // save metrics
            _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            {
                ImageFileName = _appState.InputImageFileName,
                EnhanceMethodName = nameof(Dust.VisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod),
                Metrics = result.Metrics,
                ExecutionTimeMs = result.ExecutionTimeMs
            });
            DisplayImageMetrics(result.ExecutionTimeMs, result.Metrics);

            GC.Collect();
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
            _params.ShowOptionalWindows = GetCheckBoxValue(checkBoxShowOptionalWindows);

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
                Metrics = result.Metrics,
                ExecutionTimeMs = result.ExecutionTimeMs
            });
            DisplayImageMetrics(result.ExecutionTimeMs, result.Metrics);

            GC.Collect();
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
            var result = GammaCorrection.AdaptiveWithBaseResponse(_appState.InputImageBgr, showWindows: GetCheckBoxValue(checkBoxShowOptionalWindows));
            _appState.SetOutputImage(result.EnhancementResult);
            _appState.SetShadowMaskImage(result.DetectionResult);
            this.DisplayImageInPictureBox(pictureBox3, result.EnhancementResult.Bitmap);
            this.DisplayImageInPictureBox(pictureBox2, result.DetectionResult.Bitmap);

            // save metrics
            _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
            {
                ImageFileName = _appState.InputImageFileName,
                EnhanceMethodName = nameof(GammaCorrection.Adaptive),
                Metrics = result.Metrics,
                ExecutionTimeMs = result.ExecutionTimeMs
            });
            DisplayImageMetrics(result.ExecutionTimeMs, result.Metrics);

            // compare RMS
            double rms1 = ImageMetricHelper.RMS(_appState.InputImageBgr.Convert<Bgr, double>());
            double rms2 = ImageMetricHelper.RMS(result.EnhancementResult.Convert<Bgr, double>());
            double diff = rms2 - rms1;
            var metricRes = ImageMetricHelper.ComputeAll(_appState.InputImageBgr, result.EnhancementResult);

            // compare entropy
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


        // Run All Methods On All Test Images and save results
        private void buttonRunAllMethods_Click(object sender, EventArgs e)
        {
            string imagesPath = @"D:\Google Drive\Diploma5\Images";
            string fogImagesPath = @"FogImagesConverted";
            string dustImagesPath = @"DustImagesConverted";
            string rainImagesPath = @"RainImagesConverted";
            string snowImagesPath = @"SnowImagesConverted";

            string resultsDestPath = Path.Combine(imagesPath, @"Results");
            string fogImagesDestPath = Path.Combine(imagesPath, @"Results", @"Fog");
            string dustImagesDestPath = Path.Combine(imagesPath, @"Results", @"Dust");

            if (!Directory.Exists(resultsDestPath)) Directory.CreateDirectory(resultsDestPath);
            if (!Directory.Exists(fogImagesDestPath)) Directory.CreateDirectory(fogImagesDestPath);
            if (!Directory.Exists(dustImagesDestPath)) Directory.CreateDirectory(dustImagesDestPath);


            // delete all files in target directories
            //foreach (var file in Directory.GetFiles(resultsDestPath))
            //{
            //   File.Delete(file);
            //}
            //foreach (var file in Directory.GetFiles(Path.Combine(resultsDestPath, fogImagesDestPath)))
            //{
            //   File.Delete(file);
            //}
            //foreach (var file in Directory.GetFiles(Path.Combine(resultsDestPath, dustImagesDestPath)))
            //{
            //   File.Delete(file);
            //}

            // Method_[Fog|Dust|Rain|Snow]_OriginFileName_DetailedResultNumber.Extension
            const string IMAGE_FILENAME_TEMPLATE = "{0}_{1}_{2}_{3}{4}";

            // fog (also test on dust, rain, snow images)
            List<string> fogFiles = new List<string>();
            fogFiles.AddRange(Directory.EnumerateFiles(Path.Combine(imagesPath, fogImagesPath)));
            // fogFiles.AddRange(Directory.EnumerateFiles(Path.Combine(imagesPath, dustImagesPath)));
            //fogFiles.AddRange(Directory.EnumerateFiles(Path.Combine(imagesPath, rainImagesPath)));
            //fogFiles.AddRange(Directory.EnumerateFiles(Path.Combine(imagesPath, snowImagesPath)));

            List<string> dustFiles = new List<string>();
            //dustFiles.AddRange(Directory.EnumerateFiles(Path.Combine(imagesPath, fogImagesPath)));
            dustFiles.AddRange(Directory.EnumerateFiles(Path.Combine(imagesPath, dustImagesPath)));
            //dustFiles.AddRange(Directory.EnumerateFiles(Path.Combine(imagesPath, rainImagesPath)));
            //dustFiles.AddRange(Directory.EnumerateFiles(Path.Combine(imagesPath, snowImagesPath)));

            int totalFilesToProcess = fogFiles.Count + dustFiles.Count;
            int filesProcessed = 0;

            Image<Bgr, byte> image;
            FogRemovalParams _params;
            string method;
            BaseMethodResponse result;
            string imageType;
            IInputArray img;
            string imgPath;

            // fog
            if (this.checkBoxRunAllMethodsFog.Checked)
            {
                foreach (var imageFileName in fogFiles)
                {
                    if (!File.Exists(imageFileName)) continue;

                    if (imageFileName.ToLower().Contains("fog")) imageType = "Fog";
                    else if (imageFileName.ToLower().Contains("dust")) imageType = "Dust";
                    else if (imageFileName.ToLower().Contains("rain")) imageType = "Rain";
                    else if (imageFileName.ToLower().Contains("snow")) imageType = "Snow";
                    else imageType = "Unknown";

                    image = new Image<Bgr, byte>(imageFileName);
                    _params = new FogRemovalParams { ShowWindows = false, InputImageFileName = imageFileName };

                    // DCP
                    method = this._methodInfoStore.MethodNameMap[nameof(Fog.RemoveFogUsingDarkChannelPrior)];
                    result = this._appState.Fog.RemoveFogUsingDarkChannelPrior(image, _params);
                    if (this.checkBoxUpdateStats.Checked)
                    {
                        _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
                        {
                            ImageFileName = imageFileName,
                            EnhanceMethodName = nameof(Fog.RemoveFogUsingDarkChannelPrior),
                            Metrics = result.Metrics,
                            ExecutionTimeMs = result.ExecutionTimeMs
                        });
                    }
                    for (int i = result.DetailedResults.Count() - 1; i >= 0; i -= 1)
                    {
                        if (this.checkBoxRunAllMethodsSaveAllImages.Checked == false)
                        {
                            // save only first ad last image
                            if (i != result.DetailedResults.Count() - 1 && i != 0)
                            {
                                continue;
                            }
                        }
                        img = result.DetailedResults[i];
                        imgPath = Path.Combine(fogImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, imageType, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
                        CvInvoke.Imwrite(imgPath, img);
                    }

                    result.EnhancementResult.Dispose();
                    result.DetectionResult.Dispose();
                    GC.Collect();

                    // MCP
                    method = this._methodInfoStore.MethodNameMap[nameof(Fog.RemoveFogUsingMedianChannelPrior)];
                    result = this._appState.Fog.RemoveFogUsingMedianChannelPrior(image, _params);
                    if (this.checkBoxUpdateStats.Checked)
                    {
                        _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
                        {
                            ImageFileName = imageFileName,
                            EnhanceMethodName = nameof(Fog.RemoveFogUsingMedianChannelPrior),
                            Metrics = result.Metrics,
                            ExecutionTimeMs = result.ExecutionTimeMs
                        });
                    }
                    for (int i = result.DetailedResults.Count() - 1; i >= 0; i -= 1)
                    {
                        if (this.checkBoxRunAllMethodsSaveAllImages.Checked == false)
                        {
                            // save only first ad last image
                            if (i != result.DetailedResults.Count() - 1 && i != 0)
                            {
                                continue;
                            }
                        }
                        img = result.DetailedResults[i];
                        imgPath = Path.Combine(fogImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, imageType, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
                        CvInvoke.Imwrite(imgPath, img);
                    }
                    result.EnhancementResult.Dispose();
                    result.DetectionResult.Dispose();
                    GC.Collect();

                    // DCP and CLAHE
                    method = this._methodInfoStore.MethodNameMap[nameof(Fog.RemoveFogUsingIdcpWithClahe)];
                    result = this._appState.Fog.RemoveFogUsingIdcpWithClahe(image, _params);
                    if (this.checkBoxUpdateStats.Checked)
                    {
                        _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
                        {
                            ImageFileName = imageFileName,
                            EnhanceMethodName = nameof(Fog.RemoveFogUsingIdcpWithClahe),
                            Metrics = result.Metrics,
                            ExecutionTimeMs = result.ExecutionTimeMs
                        });
                    }
                    for (int i = result.DetailedResults.Count() - 1; i >= 0; i -= 1)
                    {
                        if (this.checkBoxRunAllMethodsSaveAllImages.Checked == false)
                        {
                            // save only first ad last image
                            if (i != result.DetailedResults.Count() - 1 && i != 0)
                            {
                                continue;
                            }
                        }
                        img = result.DetailedResults[i];
                        imgPath = Path.Combine(fogImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, imageType, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
                        CvInvoke.Imwrite(imgPath, img);
                    }
                    result.EnhancementResult.Dispose();
                    result.DetectionResult.Dispose();
                    GC.Collect();

                    // DCP and DFT
                    // method = this._methodInfoStore.MethodNameMap[nameof(Fog.RemoveFogUsingDCPAndDFT)];
                    //result = this._appState.Fog.RemoveFogUsingDCPAndDFT(image, _params);
                    //if (this.checkBoxUpdateStats.Checked)
                    //{
                    //    _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
                    //    {
                    //        ImageFileName = imageFileName,
                    //        EnhanceMethodName = nameof(Fog.RemoveFogUsingDCPAndDFT),
                    //        Metrics = result.Metrics,
                    //        ExecutionTimeMs = result.ExecutionTimeMs
                    //    });
                    //}
                    //for (int i = result.DetailedResults.Count() - 1; i >= 0; i -= 1)
                    //{
                    //if (this.checkBoxRunAllMethodsSaveAllImages.Checked == false)
                    //{
                    //    // save only first ad last image
                    //    if (i != result.DetailedResults.Count() - 1 && i != 0)
                    //    {
                    //        continue;
                    //    }
                    //}
                    //    img = result.DetailedResults[i];
                    //    imgPath = Path.Combine(fogImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, imageType, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
                    //    CvInvoke.Imwrite(imgPath, img);
                    //}
                    //result.EnhancementResult.Dispose();
                    //result.DetectionResult.Dispose();
                    //GC.Collect();

                    // MultiCoreDSP
                    method = this._methodInfoStore.MethodNameMap[nameof(Fog.RemoveFogUsingMultiCoreDSPMethod)];
                    result = this._appState.Fog.RemoveFogUsingMultiCoreDSPMethod(image, _params);
                    if (this.checkBoxUpdateStats.Checked)
                    {
                        _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
                        {
                            ImageFileName = imageFileName,
                            EnhanceMethodName = nameof(Fog.RemoveFogUsingMultiCoreDSPMethod),
                            Metrics = result.Metrics,
                            ExecutionTimeMs = result.ExecutionTimeMs
                        });
                    }
                    for (int i = result.DetailedResults.Count() - 1; i >= 0; i -= 1)
                    {
                        if (this.checkBoxRunAllMethodsSaveAllImages.Checked == false)
                        {
                            // save only first ad last image
                            if (i != result.DetailedResults.Count() - 1 && i != 0)
                            {
                                continue;
                            }
                        }
                        img = result.DetailedResults[i];
                        imgPath = Path.Combine(fogImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, imageType, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
                        CvInvoke.Imwrite(imgPath, img);
                    }
                    result.EnhancementResult.Dispose();
                    result.DetectionResult.Dispose();
                    GC.Collect();

                    // RobbyTanMethodForRoads
                    // method = this._methodInfoStore.MethodNameMap[nameof(Fog.EnhaceVisibilityUsingRobbyTanMethodForRoads)];
                    //result = this._appState.Fog.EnhaceVisibilityUsingRobbyTanMethodForRoads(image, _params);
                    //if (this.checkBoxUpdateStats.Checked)
                    //{
                    //    _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
                    //    {
                    //        ImageFileName = imageFileName,
                    //        EnhanceMethodName = nameof(Fog.EnhaceVisibilityUsingRobbyTanMethodForRoads),
                    //        Metrics = result.Metrics,
                    //        ExecutionTimeMs = result.ExecutionTimeMs
                    //    });
                    //}
                    //for (int i = result.DetailedResults.Count() - 1; i >= 0; i -= 1)
                    //{
                    //if (this.checkBoxRunAllMethodsSaveAllImages.Checked == false)
                    //{
                    //    // save only first ad last image
                    //    if (i != result.DetailedResults.Count() - 1 && i != 0)
                    //    {
                    //        continue;
                    //    }
                    //}
                    //    img = result.DetailedResults[i];
                    //    imgPath = Path.Combine(fogImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, imageType, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
                    //    CvInvoke.Imwrite(imgPath, img);
                    //}
                    //result.EnhancementResult.Dispose();
                    //result.DetectionResult.Dispose();
                    //GC.Collect();

                    // Custom
                    method = this._methodInfoStore.MethodNameMap[nameof(Fog.RemoveFogUsingCustomMethod)];
                    result = this._appState.Fog.RemoveFogUsingCustomMethod(image, _params);
                    if (this.checkBoxUpdateStats.Checked)
                    {
                        _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
                        {
                            ImageFileName = imageFileName,
                            EnhanceMethodName = nameof(Fog.RemoveFogUsingCustomMethod),
                            Metrics = result.Metrics,
                            ExecutionTimeMs = result.ExecutionTimeMs
                        });
                    }
                    for (int i = result.DetailedResults.Count() - 1; i >= 0; i -= 1)
                    {
                        if (this.checkBoxRunAllMethodsSaveAllImages.Checked == false)
                        {
                            // save only first ad last image
                            if (i != result.DetailedResults.Count() - 1 && i != 0)
                            {
                                continue;
                            }
                        }
                        img = result.DetailedResults[i];
                        imgPath = Path.Combine(fogImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, imageType, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
                        CvInvoke.Imwrite(imgPath, img);
                    }
                    result.EnhancementResult.Dispose();
                    result.DetectionResult.Dispose();
                    GC.Collect();

                    // RemoveFogUsingCustomMethodWithDepthEstimation
                    try
                    {
                        method = this._methodInfoStore.MethodNameMap[nameof(Fog.RemoveFogUsingCustomMethodWithDepthEstimation)];
                        result = this._appState.Fog.RemoveFogUsingCustomMethodWithDepthEstimation(image, _params);
                        if (this.checkBoxUpdateStats.Checked)
                        {
                            _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
                            {
                                ImageFileName = imageFileName,
                                EnhanceMethodName = nameof(Fog.RemoveFogUsingCustomMethodWithDepthEstimation),
                                Metrics = result.Metrics,
                                ExecutionTimeMs = result.ExecutionTimeMs
                            });
                        }
                        for (int i = result.DetailedResults.Count() - 1; i >= 0; i -= 1)
                        {
                            if (this.checkBoxRunAllMethodsSaveAllImages.Checked == false)
                            {
                                // save only first ad last image
                                if (i != result.DetailedResults.Count() - 1 && i != 0)
                                {
                                    continue;
                                }
                            }
                            img = result.DetailedResults[i];
                            imgPath = Path.Combine(fogImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, imageType, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
                            CvInvoke.Imwrite(imgPath, img);
                        }
                        result.EnhancementResult.Dispose();
                        result.DetectionResult.Dispose();
                        GC.Collect();
                    }
                    catch(Exception ex)
                    {
                        // skip
                    }

                    // update counter
                    filesProcessed += 1;

                    image.Dispose();
                    GC.Collect();

                    //long maxMemory = 2 * 1024 * 1024 * 1024; // 4 Gb
                    // long memoryBytes = GC.GetTotalMemory(true);
                }
            }

            // dust (also test on fog, rain, snow images)
            if (this.checkBoxRunAllMethodsDust.Checked)
            {
                foreach (var imageFileName in dustFiles)
                {
                    if (imageFileName.ToLower().Contains("fog")) imageType = "Fog";
                    else if (imageFileName.ToLower().Contains("dust")) imageType = "Dust";
                    else if (imageFileName.ToLower().Contains("rain")) imageType = "Rain";
                    else if (imageFileName.ToLower().Contains("snow")) imageType = "Snow";
                    else imageType = "Unknown";

                    image = new Image<Bgr, byte>(imageFileName);

                    // FuzzyOperators
                    method = this._methodInfoStore.MethodNameMap[nameof(Dust.VisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod)];
                    var result2 = this._appState.Dust.VisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod(image, new TriThresholdFuzzyIntensificationOperatorsMethodParams { Dzeta = null });
                    if (this.checkBoxUpdateStats.Checked)
                    {
                        _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
                        {
                            ImageFileName = imageFileName,
                            EnhanceMethodName = nameof(Dust.VisibilityEnhancementUsingTunedTriThresholdFuzzyIntensificationOperatorsMethod),
                            Metrics = result2.Metrics,
                            ExecutionTimeMs = result2.ExecutionTimeMs
                        });
                    }
                    for (int i = result2.DetailedResults.Count() - 1; i >= 0; i -= 1)
                    {
                        if (this.checkBoxRunAllMethodsSaveAllImages.Checked == false)
                        {
                            // save only first ad last image
                            if (i != result2.DetailedResults.Count() - 1 && i != 0)
                            {
                                continue;
                            }
                        }
                        img = result2.DetailedResults[i];
                        imgPath = Path.Combine(dustImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, imageType, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
                        CvInvoke.Imwrite(imgPath, img);
                    }
                    result2.EnhancementResult.Dispose();
                    result2.DetectionResult.Dispose();
                    GC.Collect();

                    // RGBResponseRatioConstancy
                    try
                    {
                        method = this._methodInfoStore.MethodNameMap[nameof(Dust.RecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod)];
                        result2 = this._appState.Dust.RecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod(image, new RGBResponseRatioConstancyMethodParams { ShowOptionalWindows = false });
                        if (this.checkBoxUpdateStats.Checked)
                        {
                            _methodInfoStore.AddOrUpdate(new EnhanceMethodInfoModel
                            {
                                ImageFileName = imageFileName,
                                EnhanceMethodName = nameof(Dust.RecoveringOfWeatherDegradedImagesBasedOnRGBResponseRatioConstancyMethod),
                                Metrics = result2.Metrics,
                                ExecutionTimeMs = result2.ExecutionTimeMs
                            });
                        }
                        for (int i = result2.DetailedResults.Count() - 1; i >= 0; i -= 1)
                        {
                            if (this.checkBoxRunAllMethodsSaveAllImages.Checked == false)
                            {
                                // save only first ad last image
                                if (i != result2.DetailedResults.Count() - 1 && i != 0)
                                {
                                    continue;
                                }
                            }
                            img = result2.DetailedResults[i];
                            imgPath = Path.Combine(dustImagesDestPath, String.Format(IMAGE_FILENAME_TEMPLATE, method, imageType, Path.GetFileNameWithoutExtension(imageFileName), i, Path.GetExtension(imageFileName)));
                            CvInvoke.Imwrite(imgPath, img);
                        }
                        result2.EnhancementResult.Dispose();
                        result2.DetectionResult.Dispose();
                        GC.Collect();
                    }
                    catch (Exception ex)
                    {
                        //Console.Error.WriteLine(ex.Message);
                    }

                    // update counter
                    filesProcessed += 1;

                    image.Dispose();
                    GC.Collect();
                }
            }

         MessageBox.Show("Done");
        }

        
    }
}
