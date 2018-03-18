using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Accord.Collections;
using Accord.IO;
using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.CV.CvEnum;
using Emgu.CV.Fuzzy;
using Emgu.CV.Structure;

namespace FaceBlur.Engine.VideoFrameProcessors
{
    public class CascadeClassifierProcessor
    {
        private const string outTempDir = @"C:\_videoBlurring\output";
        private static readonly List<string> _classifierXmlFiles = new List<string>()
        {
            "haarcascade_frontalface_alt_tree.xml",
        };
        private static readonly List<CascadeClassifier> _classifiers = new List<CascadeClassifier>();
        private static readonly Dictionary<Color, CascadeClassifier> _colouredClassifiers = new Dictionary<Color, CascadeClassifier>();

        public static CascadeClassifierProcessor Setup(string haarcascadesRootPath)
        {
            foreach (var classifierXml in _classifierXmlFiles)
            {
                var xmlFile = Path.Combine(haarcascadesRootPath, classifierXml);
                _classifiers.Add(new CascadeClassifier(xmlFile));
            }

            //_colouredClassifiers.Add(Color.Green, new CascadeClassifier(Path.Combine(haarcascadesRootPath,"haarcascade_frontalface_alt_tree.xml")));
            _colouredClassifiers.Add(Color.Red, new CascadeClassifier(Path.Combine(haarcascadesRootPath, "haarcascade_frontalface_alt.xml")));
            //_colouredClassifiers.Add(Color.Red, new CascadeClassifier(Path.Combine(haarcascadesRootPath, "haarcascade_frontalface_alt.xml")));
            var processor = new CascadeClassifierProcessor();
            return new CascadeClassifierProcessor();
        }

        public Bitmap ProcessFrame(Bitmap inputImage)
        {
            Image<Bgr, byte> imageFrame = new Image<Bgr, byte>(new Bitmap(inputImage));
            Image<Gray, byte> grayImage = imageFrame.Convert<Gray, byte>();

            foreach (var cascadeClassifier in _colouredClassifiers)
            {
                Rectangle[] rectangles = cascadeClassifier.Value.DetectMultiScale(grayImage, 1.1, 0, new Size(100, 100), new Size(800, 800));
                foreach (var rectangle in rectangles)
                {
                    Mat dst = new Mat();
                    imageFrame.ROI = rectangle;
                    var rectangleCopy = imageFrame.Copy();
                    CvInvoke.GaussianBlur(rectangleCopy, dst, new Size(45, 45), 0);

                    var blurredFrame = dst.ToImage<Bgr, byte>();
                    CvInvoke.cvCopy(blurredFrame, imageFrame, IntPtr.Zero);

                    //imageFrame.Draw( rectangle,  new  Bgr(cascadeClassifier.Key), 3 );
                }
            }
            imageFrame.ROI = Rectangle.Empty;
            return imageFrame.Bitmap;
        }

        private string GetTempFileName()
        {
            return Path.Combine(outTempDir, Path.GetTempFileName() + "bmp");
        }

        private Image<Bgr, byte> GetBlurredRectangle(Image<Bgr, byte> image)
        {
            var size = new Size(image.Width, image.Height);
            return new Image<Bgr, byte>(size);
        }
        private Image<Bgr, byte> GetBlackRectangle(Image<Bgr, byte> image, Rectangle rectangle)
        {
            var size = new Size(image.Width, image.Height);
            return new Image<Bgr, byte>(size);
        }
    }
}