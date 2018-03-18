namespace FaceBlur.Engine.Model
{
    public class VideoProcessorSettings
    {
        //public int FrameRate { get; set; }
        public string OutputFolder { get; set; }
        /// <summary>
        /// Step used to iterate the frames. 1 will process each of them. 5 will skip 5 frames, and so on.
        /// </summary>
        public int FrameProcessStep { get; set; } = 1;

        public string HaarcascadeFolder { get; set; }
    }
}