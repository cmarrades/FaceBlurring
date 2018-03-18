//using System;
//using System.Drawing;
//using Emgu.CV;
//using Emgu.CV.Structure;

//namespace FaceBlur.Engine.ImageProcessors
//{
//    public class FaceBlurProcessor
//    {
//        public Bitmap BlurFaces(Bitmap inputImage)
//        {
//            Image<Bgr, byte> emguImage = new Image<Bgr, byte>(new Bitmap(inputImage));
//            Image<Gray, Byte> grayImage = emguImage.Convert<Gray, Byte>();

//            HaarCascade.Detect
//            // emguImage.
//            return inputImage;

//        }

//        private void ProcessFrame(object sender, EventArgs arg)
//        {
//            Image InputImg = Image.FromFile(@"C:\Emgu\c.jpg");
//            Image<Bgr, byte> ImageFrame = new Image<Bgr, byte>(new Bitmap(InputImg));

//            if (ImageFrame != null)   // confirm that image is valid 10             
//            {
//                Image<Gray, byte> grayframe = ImageFrame.Convert<Gray, byte>();
//                var faces = grayframe.DetectHaarCascade(haarCascade)[0];
//                var nos = grayframe.DetectHaarCascade(nose);
//                var eyes = grayframe.DetectHaarCascade(eye, 1.2, 5,HAAR_DETECTION_TYPE.DO_CANNY_PRUNING, new Size(20, 20));


//                foreach (var eye2 in eyes[0])
//                {
//                    Rectangle eyeRect1 = eye2.rect;

//                    ImageFrame.Draw(eyeRect1, new Bgr(Color.Pink), 2);


//                }


//                foreach (var noses1 in nos[0])
//                {
//                    Rectangle noserect = noses1.rect;

//                    ImageFrame.Draw(noserect, new Bgr(Color.Blue), 2);
//                }

//                foreach (var face in faces)
//                {

//                    ImageFrame.Draw(face.rect, new Bgr(Color.Green), 3);
//                    Rectangle facesnap = face.rect;
//                    int halfheight = facesnap.Height / 2;
//                    int start = facesnap.X;
//                    int start1 = facesnap.Y;
//                    Rectangle top = new Rectangle(start, start1, facesnap.Width, halfheight);
//                    int start2 = top.Bottom;
//                    Rectangle bottom = new Rectangle(start, start2, facesnap.Width, halfheight);
//                    //ImageFrame.Draw(bottom, new Bgr(Color.Yellow), 2);
//                    //Set the region of interest on the faces
//                    grayframe.ROI = bottom;

//                    var mouths = grayframe.DetectHaarCascade(mouth,
//                        1.5, 10,
//                        Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
//                        new Size(20, 20));
//                    // grayframe.ROI = Rectangle.Empty;

//                    foreach (var mouthsnap in mouths[0])
//                    {
//                        Rectangle mouthRect = mouthsnap.rect;
//                        mouthRect.Offset(bottom.X, bottom.Y);
//                        ImageFrame.Draw(mouthRect, new Bgr(Color.Red), 2);


//                    }



//                }



//            }
//        }
//    }
//}