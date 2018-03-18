using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.CV.Structure;
using log4net;

namespace FaceBlur.Engine.VideoFrameProcessors
{
    /// <summary>
    /// https://github.com/emgucv/emgucv/blob/master/Emgu.CV.Example/FaceDetection/DetectFace.cs
    /// </summary>
    public class FaceDetector
    {
        public static ILog _logger = LogManager.GetLogger(typeof(FaceDetector));
        private static class DetectFace
        {
            public static void Detect(Bitmap inputImage, IInputArray image, string faceFileName, string eyeFileName,List<Rectangle> faces, List<Rectangle> eyes, out long detectionTime)
            {
                try
                {

                //InputArray inputArray = new Image<Bgr, byte>(new Bitmap(inputImage));
                    
                using (InputArray iaImage = image.GetInputArray())
                {

#if !(__IOS__ || NETFX_CORE)
                    Stopwatch watch;
                    if (iaImage.Kind == InputArray.Type.CudaGpuMat && CudaInvoke.HasCuda)
                    {
                        using (CudaCascadeClassifier face = new CudaCascadeClassifier(faceFileName))
                        using (CudaCascadeClassifier eye = new CudaCascadeClassifier(eyeFileName))
                        {
                            face.ScaleFactor = 1.1;
                            face.MinNeighbors = 10;
                            face.MinObjectSize = Size.Empty;
                            eye.ScaleFactor = 1.1;
                            eye.MinNeighbors = 10;
                            eye.MinObjectSize = Size.Empty;
                            watch = Stopwatch.StartNew();
                            using (CudaImage<Bgr, Byte> gpuImage = new CudaImage<Bgr, byte>(image))
                            using (CudaImage<Gray, Byte> gpuGray = gpuImage.Convert<Gray, Byte>())
                            using (GpuMat region = new GpuMat())
                            {
                                face.DetectMultiScale(gpuGray, region);
                                Rectangle[] faceRegion = face.Convert(region);
                                faces.AddRange(faceRegion);
                                foreach (Rectangle f in faceRegion)
                                {
                                    using (CudaImage<Gray, Byte> faceImg = gpuGray.GetSubRect(f))
                                    {
                                        //For some reason a clone is required.
                                        //Might be a bug of CudaCascadeClassifier in opencv
                                        using (CudaImage<Gray, Byte> clone = faceImg.Clone(null))
                                        using (GpuMat eyeRegionMat = new GpuMat())
                                        {
                                            eye.DetectMultiScale(clone, eyeRegionMat);
                                            Rectangle[] eyeRegion = eye.Convert(eyeRegionMat);
                                            foreach (Rectangle e in eyeRegion)
                                            {
                                                Rectangle eyeRect = e;
                                                eyeRect.Offset(f.X, f.Y);
                                                eyes.Add(eyeRect);
                                            }
                                        }
                                    }
                                }
                            }
                            watch.Stop();
                        }
                    }
                    else
#endif
                    {
                        //Read the HaarCascade objects
                        using (CascadeClassifier face = new CascadeClassifier(faceFileName))
                        using (CascadeClassifier eye = new CascadeClassifier(eyeFileName))
                        {
                            watch = Stopwatch.StartNew();

                            using (UMat ugray = new UMat())
                            {
                                CvInvoke.CvtColor(image, ugray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

                                //normalizes brightness and increases contrast of the image
                                CvInvoke.EqualizeHist(ugray, ugray);

                                //Detect the faces  from the gray scale image and store the locations as rectangle
                                //The first dimensional is the channel
                                //The second dimension is the index of the rectangle in the specific channel                     
                                Rectangle[] facesDetected = face.DetectMultiScale(
                                    ugray,
                                    1.1,
                                    10,
                                    new Size(20, 20));

                                faces.AddRange(facesDetected);

                                foreach (Rectangle f in facesDetected)
                                {
                                    //Get the region of interest on the faces
                                    using (UMat faceRegion = new UMat(ugray, f))
                                    {
                                        Rectangle[] eyesDetected = eye.DetectMultiScale(
                                            faceRegion,
                                            1.1,
                                            10,
                                            new Size(20, 20));

                                        foreach (Rectangle e in eyesDetected)
                                        {
                                            Rectangle eyeRect = e;
                                            eyeRect.Offset(f.X, f.Y);
                                            eyes.Add(eyeRect);
                                        }
                                    }
                                }
                            }
                            watch.Stop();
                        }
                    }
                    detectionTime = watch.ElapsedMilliseconds;
                }
                }
                catch (Exception ex)
                {
                    _logger.Error(ex);
                    throw;
                }
            }

        }
    }
}