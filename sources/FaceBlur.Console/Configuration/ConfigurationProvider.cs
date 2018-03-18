using System.Configuration;

namespace FaceBlur.Console.Configuration
{
    public static class ConfigurationProvider
    {
        public static string OutputVideoRootPath => ConfigurationManager.AppSettings["OutputVideoRootPath"];
        public static string InputVideoRootPath => ConfigurationManager.AppSettings["InputVideoRootPath"];
        public static string HaarcascadeRootPath => ConfigurationManager.AppSettings["HaarcascadeRootPath"];
    }
}