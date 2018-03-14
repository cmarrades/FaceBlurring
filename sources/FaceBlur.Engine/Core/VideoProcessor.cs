using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Accord.IO;
using Accord.Video.FFMPEG;
using FaceBlur.Engine.ImageProcessors;
using FaceBlur.Engine.Model;

namespace FaceBlur.Engine.Core
{
    public class VideoProcessor
    {
        public VideoProcessorSettings VideoProcessingSettings { get; set; }

        public void ProcessVideo(string inputVideoPath)
        {
            GenerateOutputFromVideo(inputVideoPath, new SepiaProcessor().ProcessSepia);
        }

        // public static ILog _logger = LogManager.GetLogger(typeof(BaseService<T>));
        public void GenerateOutputFromVideo(string inputVideoPath, Func<Bitmap, Bitmap> imageProcessor)
        {
            try
            {
                var outputVideoPath = Path.Combine(VideoProcessingSettings.OutputFolder, BuildOutputVideoName(inputVideoPath));
                using (var reader = new VideoFileReader())
                using (var writer = new VideoFileWriter())
                {
                    reader.Open(inputVideoPath);
                    var outputProperties = BuildOutputProperties(reader);
                    //writer.Open(outputVideoPath, outputProperties.Width, outputProperties.Height, reader.FrameRate, VideoCodec.MPEG4);
                    writer.Open(outputVideoPath, outputProperties.Width, outputProperties.Height);
                    for (var i = 0; i < reader.FrameCount; i += VideoProcessingSettings.FrameProcessStep)
                    {
                        using (var inputFrame = reader.ReadVideoFrame())
                        {
                            var processedImage = imageProcessor(inputFrame);
                            writer.WriteVideoFrame(processedImage);
                            processedImage.Dispose();
                        }
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
        public void FaceBlurringProcessor(Bitmap inputImage)
        {
            //WRITE SHIT


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
