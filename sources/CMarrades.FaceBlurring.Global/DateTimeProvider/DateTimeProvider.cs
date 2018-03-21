using System.Diagnostics;

namespace CMarrades.FaceBlurring.Global.DateTimeProvider
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public string TimeStamp()
        {
            return Stopwatch.GetTimestamp().ToString();
        }
    }
}