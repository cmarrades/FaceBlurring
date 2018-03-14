using System.Diagnostics;

namespace FaceBlur.Engine.DateTimeProvider
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public string TimeStamp()
        {
            return Stopwatch.GetTimestamp().ToString();
        }
    }
}