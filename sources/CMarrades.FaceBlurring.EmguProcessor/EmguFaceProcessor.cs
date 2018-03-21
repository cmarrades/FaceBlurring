using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using CMarrades.FaceBlurring.Global.Interfaces;
using Emgu.CV;
using Emgu.CV.Structure;

namespace CMarrades.FaceBlurring.EmguProcessor
{
    public class EmguFaceProcessor : IFaceProcessor
    {
        public string LibraryName => "emgu";

        private static readonly Dictionary<Color, CascadeClassifier> _colouredClassifiers = new Dictionary<Color, CascadeClassifier>();

        public static EmguFaceProcessor Setup(string haarcascadesRootPath)
        {
            if (!_colouredClassifiers.Any())
            {
                _colouredClassifiers.Add(Color.Green, new CascadeClassifier(Path.Combine(haarcascadesRootPath, "haarcascade_frontalface_alt_tree.xml")));
                _colouredClassifiers.Add(Color.Red, new CascadeClassifier(Path.Combine(haarcascadesRootPath, "haarcascade_frontalface_alt.xml")));
                _colouredClassifiers.Add(Color.Orange, new CascadeClassifier(Path.Combine(haarcascadesRootPath, "haarcascade_frontalface_alt2.xml")));
                _colouredClassifiers.Add(Color.Aqua, new CascadeClassifier(Path.Combine(haarcascadesRootPath, "haarcascade_frontalface_default.xml")));
            }
            return new EmguFaceProcessor();
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
                    imageFrame.ROI = rectangle;
                    var facePatch = GetBlackRectangle(rectangle);
                    CvInvoke.cvCopy(facePatch, imageFrame, IntPtr.Zero);

                    //imageFrame.Draw( rectangle,  new  Bgr(cascadeClassifier.Key), 3 );
                }
            }
            imageFrame.ROI = Rectangle.Empty;
            return imageFrame.Bitmap;
        }

        private Image<Bgr, byte> GetBlackRectangle(Rectangle rectangle)
        {
            var size = new Size(rectangle.Width, rectangle.Height);
            var blackRectangle = new Image<Bgr, byte>(size);
            return blackRectangle;
        }

        private Image<Bgr, byte> BlurImageRectangle(Image<Bgr, byte> image)
        {
            Mat dst = new Mat();
            //(roi is already set)
            CvInvoke.GaussianBlur(image, dst, new Size(45, 45), 0);
            return dst.ToImage<Bgr, byte>();
        }
    }
}