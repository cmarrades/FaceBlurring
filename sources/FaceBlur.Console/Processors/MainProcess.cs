using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaceBlur.Console.Configuration;
using FaceBlur.Engine.Core;
using FaceBlur.Engine.Model;

namespace FaceBlur.Console.Processors
{
    public class MainProcess
    {
        //public static ILog _logger { get; set; }

        private string _inputFolder;
        private VideoProcessor _sut;
        private string _outputFolder;

        public MainProcess()
        {
            Setup();
        }

        public void Execute()
        {
            ProcessVideo(@"C:\_videoBlurring\data\UnlimitedFight\UnlimitedFight_480.mp4");
        }

        private void Setup()
        {
            _inputFolder = ConfigurationProvider.InputVideoRootPath;
            _outputFolder = ConfigurationProvider.OutputVideoRootPath; // + $"_test{Stopwatch.GetTimestamp()}";
            Directory.CreateDirectory(_outputFolder);
            var videoProcessingSettings = new VideoProcessorSettings()
            {
                OutputFolder = _outputFolder,
                FrameProcessStep = 1
            };


            _sut = new VideoProcessor() { VideoProcessingSettings = videoProcessingSettings };
        }

        public void ProcessVideo(string relativePath)
        {
            var inputPath = Path.GetFullPath(Path.Combine(_inputFolder, relativePath));
            string fileName = Path.GetFileNameWithoutExtension(inputPath).ToLower();
            _sut.ProcessVideo(inputPath);
            var entries = Directory.GetFileSystemEntries(_outputFolder);

            if (!entries.Any(s => s.ToLower().Contains(fileName)))
            {

            }
        }
    }
}
