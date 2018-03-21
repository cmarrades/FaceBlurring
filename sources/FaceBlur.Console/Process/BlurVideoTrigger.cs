using System;
using System.IO;
using CMarrades.FaceBlurring.Console.Configuration;
using CMarrades.FaceBlurring.Global.Model;
using CMarrades.FaceBlurring.Service.Video;
using log4net;

namespace CMarrades.FaceBlurring.Console.Process
{
    public class BlurVideoTrigger
    {
        public static ILog _logger = LogManager.GetLogger(typeof(BlurVideoTrigger));
        private string _inputFolder;
        private VideoProcessorService _sut;

        public BlurVideoTrigger()
        {
            Setup();
        }

        public void Execute()
        {
            try
            {
                _sut.ProcessVideo(@"C:\_videoBlurring\data\UnlimitedFight\UnlimitedFight_720.mp4");
                //_sut.ProcessVideo(@"C:\_videoBlurring\data\UnlimitedFight\UnlimitedFight_480.mp4");
                //ProcessVideo(@"C:\_videoBlurring\data\UnlimitedMoFarah\UnlimitedMoFarah_480.mp4");
                //UnlimitedMoFarah
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
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
                FrameProcessStep = 1
            };

            _sut = new VideoProcessorService() { VideoProcessingSettings = videoProcessingSettings };
        }

    }
}
