using System.Diagnostics;

namespace CMarrades.FaceBlurring.Engine.DateTimeProvider
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public string TimeStamp()
        {
            return Stopwatch.GetTimestamp().ToString();
        }
    }
}