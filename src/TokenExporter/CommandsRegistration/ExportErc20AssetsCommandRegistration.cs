using Microsoft.Extensions.CommandLineUtils;
using TokenExporter.Commands;

namespace TokenExporter.CommandsRegistration
{
    [CommandRegistration("export-erc20-assets")]
    public class ExportErc20AssetsCommandRegistration : ICommandRegistration
    {
        private readonly CommandFactory _factory;

        public ExportErc20AssetsCommandRegistration(CommandFactory factory)
        {
            _factory = factory;
        }

        public void StartExecution(CommandLineApplication lineApplication)
        {
            lineApplication.Description = "This is the description for scan-deposits-withdraw.";
            lineApplication.HelpOption("-?|-h|--help");

            var serviceUrlOption = lineApplication.Option("-u|--url <optionvalue>",
                "Assets service URL",
                CommandOptionType.SingleValue);

            lineApplication.OnExecute(async () =>
            {
                var command = _factory.CreateCommand((helper) => new ExportErc20AssetsCommand(helper,
                    serviceUrlOption.Value()));

                return await command.ExecuteAsync();
            });
        }
    }
}

