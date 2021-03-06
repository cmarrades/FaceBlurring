﻿using System;
using CMarrades.FaceBlurring.Console.Process;

[assembly: log4net.Config.XmlConfigurator(Watch = true, ConfigFile = "log4net.config")]
namespace CMarrades.FaceBlurring.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var mainProcess = new BlurVideoTrigger();
                mainProcess.Execute();
                System.Console.WriteLine("Anonymizer video process finished. Press enter to finish.");
                System.Console.ReadLine();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.ReadLine();
            }
            
        }
    }
}
