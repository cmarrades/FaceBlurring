using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FaceBlur.Console.Process;

//[assembly: log4net.Config.XmlConfigurator(Watch = true, ConfigFile = "log4net.config")]
namespace FaceBlur.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var mainProcess = new BlurVideoTrigger();
                mainProcess.Execute();
            }
            catch (Exception e)
            {
                System.Console.WriteLine(e.Message);
                System.Console.ReadLine();
            }
            
        }
    }
}
