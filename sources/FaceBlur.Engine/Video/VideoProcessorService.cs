using System;
using System.Diagnostics;
using System.IO;
using Accord.Video.FFMPEG;
using CMarrades.FaceBlurring.AccordProcessor;
using CMarrades.FaceBlurring.EmguProcessor;
using CMarrades.FaceBlurring.Global.Interfaces;
using CMarrades.FaceBlurring.Global.Logging;
using CMarrades.FaceBlurring.Global.Model;
using log4net;

namespace CMarrades.FaceBlurring.Service.Video
{
    public class VideoProcessorService
    {
        public VideoProcessorSettings VideoProcessingSettings { get; set; }

        public void ProcessVideo(string inputVideoPath)
        {
            ProcessEmguHaarcascadeVideo(inputVideoPath);
            ProcessAccordVideo(inputVideoPath);
        }

        private void ProcessAccordVideo(string inputVideoPath)
        {
            var processor = new AccordFaceProcessor();
            GenerateOutputFromVideo(inputVideoPath, processor);
        }

        private void ProcessEmguHaarcascadeVideo(string inputVideoPath)
        {
            var processor = EmguFaceProcessor.Setup(VideoProcessingSettings.HaarcascadeFolder);
            GenerateOutputFromVideo(inputVideoPath, processor);
        }

        public void GenerateOutputFromVideo(string inputVideoPath, IFaceProcessor faceProcessor)
        {
            try
            {
                var processorName = faceProcessor.GetType().Name.ToLower();
                Logger.Info($"Processing {processorName} video {inputVideoPath}");
                var outputVideoPath = Path.Combine(VideoProcessingSettings.OutputFolder, BuildOutputVideoName(inputVideoPath, processorName));

                using (var reader = new VideoFileReader())
                using (var writer = new VideoFileWriter())
                {
                    reader.Open(inputVideoPath);
                    var outputProperties = BuildOutputProperties(reader);
                    //reader.
                    writer.Open(outputVideoPath, outputProperties.Width, outputProperties.Height, (int)Math.Round(reader.FrameRate.Value), VideoCodec.MPEG4);
                    //writer.Open(outputVideoPath, outputProperties.Width, outputProperties.Height);
                    for (var i = 0; i < reader.FrameCount; i++)
                    {
                        using (var inputFrame = reader.ReadVideoFrame())
                        {
                            if (i % VideoProcessingSettings.FrameProcessStep == 0)
                            {
                                var processedImage = faceProcessor.ProcessFrame(inputFrame);
                                writer.WriteVideoFrame(processedImage);
                                processedImage.Dispose();
                            }
                        }
                    }

                    reader.Close();
                    writer.Close();
                }

                Logger.Info($"Finished processing {processorName} video {inputVideoPath}");
            }
            catch (Exception ex)
            {
                Logger.Error("GenerateOutputFromVideo error", ex);
                throw;
            }
        }

        private OutputProperties BuildOutputProperties(VideoFileReader reader)
        {
            return new OutputProperties()
            {
                Height = reader.Height,
                Width = reader.Width,
            };
        }

        private string BuildOutputVideoName(string inputVideoPath, string processorName)
        {
            var fileName = Path.GetFileNameWithoutExtension(inputVideoPath) +
                 $"_{processorName}_output_{Stopwatch.GetTimestamp()}" + Path.GetExtension(inputVideoPath);

            return fileName;
        }
    }
}
