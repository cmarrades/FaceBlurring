using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using Accord.Video.FFMPEG;
using CMarrades.FaceBlurring.AccordProcessor;
using CMarrades.FaceBlurring.EmguProcessor;
using CMarrades.FaceBlurring.Global.Interfaces;
using CMarrades.FaceBlurring.Global.Model;
using log4net;

namespace CMarrades.FaceBlurring.Service.Video
{
    public class VideoProcessorService
    {
        public static ILog _logger = LogManager.GetLogger(typeof(VideoProcessorService));

        private const string _emguLibrary = "emgu";
        private const string _accordLibrary = "accord";
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
                _logger.Info($"Processing {processorName} video {inputVideoPath}");
                var outputVideoPath = Path.Combine(VideoProcessingSettings.OutputFolder, BuildOutputVideoName(inputVideoPath, processorName));
                using (var reader = new VideoFileReader())
                using (var writer = new VideoFileWriter())
                {
                    reader.Open(inputVideoPath);
                    var outputProperties = BuildOutputProperties(reader);
                    //writer.Open(outputVideoPath, outputProperties.Width, outputProperties.Height, reader.FrameRate, VideoCodec.MPEG4);
                    writer.Open(outputVideoPath, outputProperties.Width, outputProperties.Height);
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

                _logger.Info($"Finished processing {processorName} video {inputVideoPath}");
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

        private string BuildOutputVideoName(string inputVideoPath, string processorName)
        {
            var fileName = processorName + "_" + Path.GetFileNameWithoutExtension(inputVideoPath) +
                 $"_output{Stopwatch.GetTimestamp()}" + Path.GetExtension(inputVideoPath);

            return fileName;
        }
    }
}
