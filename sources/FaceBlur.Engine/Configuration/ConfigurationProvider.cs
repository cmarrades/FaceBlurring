using System.Configuration;

namespace FaceBlur.Engine.Configuration
{
    public static class ConfigurationProvider
    {


        public static bool VideoRootPath => bool.Parse(ConfigurationManager.AppSettings["VideoRootPath"]);
    }
}