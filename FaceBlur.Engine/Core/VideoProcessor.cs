using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using Accord.Video.FFMPEG;
using FaceBlur.Engine.DateTimeProvider;
//using log4net;
using FaceBlur.Engine.Model;

namespace FaceBlur.Engine.Core
{
    public class VideoProcessor
    {
        public VideoProcessorSettings VideoProcessingSettings { get; set; }

        // public static ILog _logger = LogManager.GetLogger(typeof(BaseService<T>));
        public void ProcessVideo(string inputVideoPath)
        {
            try
            {
                var outputVideoPath = Path.Combine(VideoProcessingSettings.OutputFolder, BuildOutputVideoName(inputVideoPath));
                using (var reader = new VideoFileReader())
                using (var writer = new VideoFileWriter())
                {
                    reader.Open(inputVideoPath);
                    var outputProperties = BuildOutputProperties(reader);
                    writer.Open(outputVideoPath, outputProperties.Height, outputProperties.Width);
                    for (var i = 0; i < reader.FrameCount; i += VideoProcessingSettings.FrameProcessStep)
                    {
                        //reader.ReadVideoFrame(i,image);
                        var inputImage = reader.ReadVideoFrame();

                        writer.WriteVideoFrame(inputImage);

                    }
                    reader.Close();
                    writer.Close();
                }
            }
            catch (Exception e)
            {
                throw;
            }

        }

        private OutputProperties BuildOutputProperties(VideoFileReader reader)
        {
            return new OutputProperties()
            {
                Height = reader.Height,
                Width = reader.Width
            };
        }

        private string BuildOutputVideoName(string inputVideoPath)
        {
            return Path.GetFileNameWithoutExtension(inputVideoPath) +
                $"_output{Stopwatch.GetTimestamp()}" + Path.GetExtension(inputVideoPath);
        }




        private void CreateReader(String fileName)
        {
            try
            {
            }
            catch (System.Exception ex)
            {

            }

        }

    }
}
