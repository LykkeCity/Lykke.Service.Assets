using Microsoft.Extensions.CommandLineUtils;

namespace TokenExporter.CommandsRegistration
{
    public interface ICommandRegistration
    {
        void StartExecution(CommandLineApplication lineApplication);
    }
}
