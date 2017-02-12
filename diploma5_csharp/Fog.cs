using System;
using System.Collections.Generic;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;

namespace diploma5_csharp
{
    public class Fog
    {
        public Image<Emgu.CV.Structure.Bgr, Byte> RemoveFogUsingDarkChannelPrior(Image<Bgr, Byte> image)
        {
            Image<Bgr, Byte> result = image.Clone();
            Image<Bgr, Byte> imgFog = image.Clone();
            Image<Gray, Byte> imgDarkChannel = new Image<Gray, byte>(image.Size);
            Image<Gray, Byte> T = new Image<Gray, byte>(image.Size);
            Image<Bgr, Byte> fogfree = image.Clone();

            int Airlight;

            //int patchSize = 5;
            int patchSize = 7;
            imgDarkChannel = GetDarkChannel(imgFog, patchSize);
            Airlight = EstimateAirlight(imgDarkChannel, imgFog);
            T = EstimateTransmission(imgDarkChannel, Airlight);
            fogfree = RemoveFog(imgFog, T, Airlight);

            if (true)
            {
                EmguCvWindowManager.Display(imgDarkChannel, "1 imgDarkChannel darkChannel MDCP");
                EmguCvWindowManager.Display(T, "2 estimateTransmission");
                EmguCvWindowManager.Display(imgFog, "3 imgFog");
                EmguCvWindowManager.Display(fogfree, "4 fogfree");
            }

            return fogfree;
        }

        private Image<Gray, Byte> GetDarkChannel(Image<Bgr, Byte> sourceImg, int patchSize)
        {
            //            Mat rgbMinImg = Mat::zeros(sourceImg.rows, sourceImg.cols, CV_8UC1);
            //            Mat MDCP;
            Image<Gray, Byte> rgbMinImg = new Image<Gray, byte>(sourceImg.Size);
            Image<Gray, Byte> MDCP = new Image<Gray, byte>(sourceImg.Size);

            for (int m = 0; m < sourceImg.Rows; m++)
            {
                for (int n = 0; n < sourceImg.Cols; n++)
                {
                    Bgr pixel = sourceImg[m, n];
                    double intensity = Math.Min(Math.Min(pixel.Blue, pixel.Green), pixel.Red);
                    rgbMinImg[m, n] = new Gray(intensity);
                }
            }
            CvInvoke.MedianBlur(rgbMinImg, MDCP, patchSize);
            return MDCP;
        }

        //estimate airlight by the 0.1% brightest pixels in dark channel
        private int EstimateAirlight(Image<Gray, Byte> DC, Image<Bgr, Byte> inputImage)
        {
            Image<Gray, Byte> rgbMinImg = new Image<Gray, byte>(inputImage.Size);
            Image<Gray, Byte> MDCP = new Image<Gray, byte>(inputImage.Size);
            Image<Lab, Byte> inputImageLab = new Image<Lab, byte>(inputImage.Size);

            double minDC = 0;
            double maxDC = 0;
            System.Drawing.Point minDCLoc = new Point();
            System.Drawing.Point maxDCLoc = new Point();
            int size = DC.Rows * DC.Cols;
            double requiredPercent = 0.001; //0.1%
            double requiredAmount = size * requiredPercent; //

            ////////////////////
            CvInvoke.MinMaxLoc(DC, ref minDC, ref maxDC, ref minDCLoc, ref maxDCLoc);
            double max = maxDC;
//            std::vector<int*> brightestDarkChannelPixels;
            List<List<int>> brightestDarkChannelPixels = new List<List<int>>();
            for (int k = 0; k < requiredAmount && max >= 0; max--)
            {
                for (int i = 0; i < DC.Rows; i++)
                {
                    bool _break = false;
                    for (int j = 0; j < DC.Cols; j++)
                    {
//                        uchar val = DC.at<uchar>(i, j);
                        Gray val = DC[i, j];
                        if (val.Intensity == max)
                            brightestDarkChannelPixels.Add(new List<int>() { i, j });

                        if (brightestDarkChannelPixels.Count >= requiredAmount - 1)
                        {
                            _break = true;
                            break;
                        }
                    }
                    if (_break)
                        break;
                }

                if (brightestDarkChannelPixels.Count >= requiredAmount)
                    break;
            }

            //take pixels with highest intensity in the input image
            CvInvoke.CvtColor(inputImage, inputImageLab, ColorConversion.Bgr2Lab);
            int airlight = -1;
            for (int r = 0; r != brightestDarkChannelPixels.Count; r++)
            {
                int i = brightestDarkChannelPixels[r][0];
                int j = brightestDarkChannelPixels[r][1];

                Lab pixel = inputImageLab[i, j];

                double L = pixel.X;
                double intensity = L; //take Lab L as insetsity
                if (intensity > airlight)
                    airlight = (int)intensity;
            }

            /////////////////

            return airlight;
        }

        //estimate transmission map
        private Image<Gray, Byte> EstimateTransmission(Image<Gray, Byte> DC, int airlight) //DC - darkChannel
        {
            double w = 0.75;
            //double w = 0.95;
            Image<Gray, Byte> transmission = DC.Clone();
            MCvScalar intensity;

            for (int m = 0; m < DC.Rows; m++)
            {
                for (int n = 0; n < DC.Cols; n++)
                {
                    intensity = DC[m, n].MCvScalar;
                    transmission[m, n] = new Gray((1 - w * (intensity.V0 / airlight)) * 255);
                }
            }
            return transmission;
        }

        //dehazing foggy image
        private Image<Bgr, Byte> RemoveFog(Image<Bgr, Byte> sourceImg, Image<Gray, Byte> transmissionImg, int airlight)
        {
            double t0 = 0.1;
            double tmax;

            int A = airlight;//airlight
//            Scalar t; //transmission
            Gray t; //transmission
            Bgr I; //I(x) - source image pixel
                     //            Mat dehazed = Mat::zeros(sourceImg.rows, sourceImg.cols, CV_8UC3);
            Image<Bgr, Byte> dehazed = new Image<Bgr, Byte>(sourceImg.Size);

            for (int i = 0; i < sourceImg.Rows; i++)
            {
                for (int j = 0; j < sourceImg.Cols; j++)
                {
                    t = transmissionImg[i, j];
                    I = sourceImg[i, j];
                    tmax = (t.Intensity / 255) < t0 ? t0 : (t.Intensity / 255);

                    double B = Math.Abs((I.Blue - A) / tmax + A) > 255 ? 255 : Math.Abs((I.Blue - A) / tmax + A);
                    double G = Math.Abs((I.Green - A) / tmax + A) > 255 ? 255 : Math.Abs((I.Green - A) / tmax + A);
                    double R = Math.Abs((I.Red - A) / tmax + A) > 255 ? 255 : Math.Abs((I.Red - A) / tmax + A);

                    dehazed[i,j] = new Bgr(B, G, R);
                }
            }
            return dehazed;
        }

    }
}