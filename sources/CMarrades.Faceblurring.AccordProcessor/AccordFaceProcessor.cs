using System;
using System.Drawing;
using Accord.Vision.Detection;
using Accord.Vision.Detection.Cascades;
using CMarrades.FaceBlurring.Global.Interfaces;
using Emgu.CV;
using Emgu.CV.Structure;

namespace CMarrades.FaceBlurring.AccordProcessor
{
    public class AccordFaceProcessor: IFaceProcessor
    {
        public string LibraryName => "accord";

        private static readonly FaceHaarCascade _cascade = new FaceHaarCascade();
        //var cascade = new Accord.Vision.Detection.Cascades.FaceHaarCascade();

        public Bitmap ProcessFrame(Bitmap inputImage)
        {
            Image<Bgr, byte> imageFrame = new Image<Bgr, byte>(new Bitmap(inputImage));

            // In this example, we will be creating a cascade for a Face detector:
            // var cascade = HaarCascade.FromXml("filename.xml"); (OpenCV 2.0 format)

            //not overlapping objects!!
            //min size 50 pixels
            var detector = new HaarObjectDetector(_cascade, minSize: 50,
                searchMode: ObjectDetectorSearchMode.NoOverlap);
            
            Rectangle[] rectangles = detector.ProcessFrame(inputImage);

            foreach (var rectangle in rectangles)
            {
                imageFrame.ROI = rectangle;
                var facePatch = GetBlackRectangle(rectangle);
                CvInvoke.cvCopy(facePatch, imageFrame, IntPtr.Zero);

                //imageFrame.Draw(rectangle, new Bgr(Color.Indigo), 3);
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
    }
}
