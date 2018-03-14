using System.Configuration;

namespace FaceBlurEngine.UnitTests.Configuration
{
    public static class ConfigurationProvider
    {
        public static string OutputVideoRootPath => ConfigurationManager.AppSettings["OutputVideoRootPath"];
        public static string InputVideoRootPath => ConfigurationManager.AppSettings["InputVideoRootPath"];
    }
}