using NUnit.Framework;

[assembly: log4net.Config.XmlConfigurator(Watch = true, ConfigFile = "log4net.config")]
namespace FaceBlurEngine.UnitTests
{
    [SetUpFixture]
    public class SetupFixture
    {
        
    }
}