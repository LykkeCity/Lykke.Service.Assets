using Autofac;
using Common.Log;
using Lykke.SettingsReader;

namespace TokenExporter.Helpers
{
    public interface IConfigurationHelper
    {
        (IContainer resolver, ILog logToConsole) GetResolver(string serviceUrl);
    }
}
