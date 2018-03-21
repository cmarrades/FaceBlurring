using System.Drawing;

namespace CMarrades.FaceBlurring.Global.Interfaces
{
    public interface IFaceProcessor
    {
        Bitmap ProcessFrame(Bitmap inputImage);
        string LibraryName { get; }
    }
}
