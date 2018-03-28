using System;
using System.Diagnostics;
using System.IO;
using CMarrades.FaceBlurring.Console.Configuration;
using CMarrades.FaceBlurring.Global.Logging;
using CMarrades.FaceBlurring.Global.Model;
using CMarrades.FaceBlurring.Service.Video;
using log4net;

namespace CMarrades.FaceBlurring.Console.Process
{
    public class BlurVideoTrigger
    {
        public static ILog _logger = LogManager.GetLogger(typeof(BlurVideoTrigger));
        private string _inputFolder;

        private VideoProcessorService _videoService;

        public BlurVideoTrigger()
        {
            Setup();
        }

        public void Execute()
        {
            try
            {

                var files = Directory.GetFiles(_inputFolder, "*", SearchOption.TopDirectoryOnly);

                for (var i = 0; i < files.Length; i++)
                {
                    var currentFile = files[i];
                    var stopWatch = Stopwatch.StartNew();
                    Logger.Info($"Processing video number {i}: {Path.GetFileName(currentFile)}");

                    _videoService.ProcessVideo(currentFile);

                    Logger.Info($"Finished Processing video number {i}: {Path.GetFileName(currentFile)}. Elapsed seconds: {stopWatch.Elapsed.TotalSeconds}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("BlurVideoTrigger.Execute", ex);
                throw;
            }
        }

        private void Setup()
        {
            _inputFolder = ConfigurationProvider.InputVideoRootPath;
            var outputFolder = ConfigurationProvider.OutputVideoRootPath; // + $"_test{Stopwatch.GetTimestamp()}";
            Directory.CreateDirectory(outputFolder);
            var videoProcessingSettings = new VideoProcessorSettings()
            {
                OutputFolder = outputFolder,
                HaarcascadeFolder = ConfigurationProvider.HaarcascadeRootPath,
                FrameProcessStep = ConfigurationProvider.FrameProcessorStep
            };

            _videoService = new VideoProcessorService() { VideoProcessingSettings = videoProcessingSettings };
        }
    }
}
