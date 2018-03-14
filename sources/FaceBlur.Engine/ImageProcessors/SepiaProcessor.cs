using System.Drawing;
using System.Drawing.Imaging;

namespace FaceBlur.Engine.ImageProcessors
{
    public class SepiaProcessor
    {
        public Bitmap ProcessSepia(Bitmap inputImage)
        {
            ColorMatrix colorMatrix = new ColorMatrix(new float[][]
            {
                new float[] {.393f, .349f, .272f, 0, 0},
                new float[] {.769f, .686f, .534f, 0, 0},
                new float[] {.189f, .168f, .131f, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {0, 0, 0, 0, 1}
            });

            return ApplyColorMatrix(inputImage, colorMatrix);
        }

        private static Bitmap ApplyColorMatrix(Image sourceImage, ColorMatrix colorMatrix)
        {
            Bitmap bmp32BppSource = GetArgbCopy(sourceImage);
            Bitmap bmp32BppDest = new Bitmap(bmp32BppSource.Width, bmp32BppSource.Height,
                PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(bmp32BppDest))
            {
                ImageAttributes bmpAttributes = new ImageAttributes();
                bmpAttributes.SetColorMatrix(colorMatrix);

                graphics.DrawImage(bmp32BppSource, new Rectangle(0, 0, bmp32BppSource.Width,
                        bmp32BppSource.Height),
                    0, 0, bmp32BppSource.Width,
                    bmp32BppSource.Height, GraphicsUnit.Pixel, bmpAttributes);

            }

            bmp32BppSource.Dispose();

            return bmp32BppDest;
        }
        private static Bitmap GetArgbCopy(Image sourceImage)
        {
            Bitmap bmpNew = new Bitmap(sourceImage.Width, sourceImage.Height, PixelFormat.Format32bppArgb);

            using (Graphics graphics = Graphics.FromImage(bmpNew))
            {
                graphics.DrawImage(sourceImage, new Rectangle(0, 0, bmpNew.Width, bmpNew.Height),
                    new Rectangle(0, 0, bmpNew.Width, bmpNew.Height), GraphicsUnit.Pixel);
                graphics.Flush();
            }

            return bmpNew;
        }
       
    }
}