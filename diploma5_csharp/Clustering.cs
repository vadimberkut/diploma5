using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Cuda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Accord.Statistics.Distributions.DensityKernels;
using Accord.Imaging.Converters;
using Accord.Math;
using diploma5_csharp.Models;
using System.Collections;

namespace diploma5_csharp
{
    public class Clustering
    {
        public static MeanShiftClusteringResult MeanShiftAccord(Image<Bgr, Byte> image, MeanShiftClusteringAcordParams msParams)
        {
            //Image<Bgr, byte> result = new Image<Bgr, byte>(image.Size);

            //int pixelSize = 3;   // RGB color pixel
            //int kernel = 3;
            //double sigma = 0.06; // kernel bandwidth
            int pixelSize = 3;   // RGB color pixel
            int kernel = msParams.Kernel;
            double sigma = msParams.Sigma; // kernel bandwidth

            // Load a test image (shown below)
            Bitmap msImage = image.Bitmap;

            // Create converters
            ImageToArray imageToArray = new ImageToArray(min: -1, max: +1);
            ArrayToImage arrayToImage = new ArrayToImage(msImage.Width, msImage.Height, min: -1, max: +1);

            // Transform the image into an array of pixel values
            double[][] pixels;
            imageToArray.Convert(msImage, out pixels);

            // Create a MeanShift algorithm using given bandwidth
            //   and a Gaussian density kernel as kernel function.
            Accord.MachineLearning.MeanShift meanShift = new Accord.MachineLearning.MeanShift(pixelSize, new GaussianKernel(kernel), sigma);

            // We will compute the mean-shift algorithm until the means
            // change less than 0.5 between two iterations of the algorithm
            meanShift.Tolerance = 0.05;
            meanShift.MaxIterations = 10;

            // Learn the clusters in the data
            var clustering = meanShift.Learn(pixels);

            // Use clusters to decide class labels
            int[] labels = clustering.Decide(pixels);
            int regionCount = labels.DistinctCount();

            // Replace every pixel with its corresponding centroid
            pixels.ApplyInPlace((x, i) => meanShift.Clusters.Modes[labels[i]]);

            // Retrieve the resulting image in a picture box
            Bitmap msResult;
            arrayToImage.Convert(pixels, out msResult);
            Image<Bgr, byte> result = new Image<Bgr, byte>(msResult);
            //EmguCvWindowManager.Display(result, "msResult");

            return new MeanShiftClusteringResult() {
                Image = result,
                Labels = labels,
                RegionCount = regionCount
            };
        }

        public static Image<Emgu.CV.Structure.Bgr, Byte> MeanShiftEmguCVCuda(Image<Bgr, Byte> image)
        {
            if (CudaInvoke.HasCuda == false)
                throw new Exception("OpenCV: The library is compiled without CUDA support");

            Image<Bgra, byte> imgInput = new Image<Bgra, byte>(image.Bitmap);
            Image<Bgra, byte> imgOutput = new Image<Bgra, byte>(imgInput.Size);

            CudaImage<Bgra, byte> cudaImage = new CudaImage<Bgra, byte>(imgInput);

            int sp = 5;
            int sr = 10;
            int minsize = 50;
            int criteria = 10; //No. iteration
            CudaInvoke.MeanShiftSegmentation(cudaImage, imgOutput, sp, sr, minsize, new MCvTermCriteria(criteria));

            Image<Bgr, byte> result = new Image<Bgr, byte>(imgOutput.Bitmap);

            return result;
        }

        /// <summary>
        /// Returns pixels coordinates for each region
        /// </summary>
        /// <param name="msResult"></param>
        public static Dictionary<int, List<PixelCoordinates>> GetRegionsPixelsCoordinates(MeanShiftClusteringResult msResult)
        {
            //key - region number
            //value - region pixels coordinates
            Dictionary<int, List<PixelCoordinates>> result = new Dictionary<int, List<PixelCoordinates>>();

            for (int m = 0; m < msResult.Image.Rows; m++)
            {
                for (int n = 0; n < msResult.Image.Cols; n++)
                {
                    Bgr pixel = msResult.Image[m, n];

                    int index = msResult.Image.Width * m + n;
                    int label = msResult.Labels[index];

                    int regionNumber = label;

                    //Init key in dictionary
                    if(result.ContainsKey(regionNumber) == false)
                        result.Add(regionNumber, new List<PixelCoordinates>());

                    //Add pixel
                    result[regionNumber].Add(new PixelCoordinates() { X = m, Y = n });
                    
                }
            }

            //throw new NotImplementedException();

            return result;
        }

        /// <summary>
        /// Returns pixels for each region
        /// </summary>
        /// <param name="msResult"></param>
        public static Dictionary<int, Bgr[]> GetRegionsPixels(MeanShiftClusteringResult msResult)
        {
            //key - region nuber
            //value - region pixels coordinates
            Dictionary<int, Bgr[]> result = new Dictionary<int, Bgr[]>();

            throw new NotImplementedException();

            return result;
        }
    }
}
