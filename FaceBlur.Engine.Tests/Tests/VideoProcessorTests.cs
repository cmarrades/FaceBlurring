using System.Diagnostics;
using System.IO;
using System.Linq;
using FaceBlur.Engine.Core;
using FaceBlur.Engine.Model;
using FaceBlurEngine.UnitTests.Configuration;
using NUnit.Framework;

namespace FaceBlurEngine.UnitTests.Tests
{
    [TestFixture]
    public class VideoProcessorTests
    {
        private string _inputFolder;
        private VideoProcessor _sut;
        private string _outputFolder;
        [SetUp]
        public void Setup()
        {
            _inputFolder = ConfigurationProvider.InputVideoRootPath;
            _outputFolder = ConfigurationProvider.OutputVideoRootPath + $"_test{Stopwatch.GetTimestamp()}";
            var videoProcessingSettings = new VideoProcessorSettings()
            {
                OutputFolder = _outputFolder,
                FrameProcessStep = 1
            };

            _sut = new VideoProcessor() { VideoProcessingSettings = videoProcessingSettings };
        }

        [TestCase(@"unlimitedfight\UnlimitedFight_480.mp4")]
        //[TestCase(@"unlimitedfight\UnlimitedFight_720.mp4")]
        public void should_blur_video(string relativePath)
        {
            var inputPath = Path.Combine(_inputFolder, relativePath);
            string fileName = Path.GetFileNameWithoutExtension(inputPath).ToLower();
            _sut.ProcessVideo(inputPath);
            var entries = Directory.GetFileSystemEntries(_outputFolder);
            
            Assert.True(entries.Any(s => s.ToLower().Contains(fileName)));
        }
    }
}
