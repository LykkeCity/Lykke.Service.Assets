using System;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.CommandLineUtils;
using TokenExporter.Commands;
using TokenExporter.CommandsRegistration;
using TokenExporter.Helpers;

namespace TokenExporter
{
    class Program
    {
        static void Main(string[] args)
        {
            Microsoft.Extensions.CommandLineUtils.CommandLineApplication application =
                new CommandLineApplication(throwOnUnexpectedArg: false);
            application.Name = "TokenExporter";
            application.Description =
                ".NET Core console app to retrieve information about current erc 20 supported assets.";
            application.HelpOption("-?|-h|--help");
            CommandFactory commandFactory = new CommandFactory(new ConfigurationHelper());

            var commanRegistrationAttributeType = typeof(CommandRegistrationAttribute);
            var currentAssemblyTypes = typeof(Program).Assembly.GetTypes();
            var commandRegistrations = currentAssemblyTypes.Where(type =>
                type.CustomAttributes.FirstOrDefault(x => x.AttributeType == commanRegistrationAttributeType) !=
                null && typeof(ICommandRegistration).IsAssignableFrom(type));

            foreach (var commandRegistration in commandRegistrations)
            {
                var attribute = (CommandRegistrationAttribute)
                    commandRegistration.GetCustomAttributes(commanRegistrationAttributeType).FirstOrDefault();
                var constructor = commandRegistration.GetConstructor(new Type[] {typeof(CommandFactory)});
                var commandReg = (ICommandRegistration) constructor.Invoke(new object[] {commandFactory});

                if (string.IsNullOrEmpty(attribute.CommandName))
                    throw new InvalidOperationException("InvalidRegistration of " + commandRegistration.FullName);

                application.Command(attribute.CommandName, commandReg.StartExecution, throwOnUnexpectedArg: false);
            }

            application.Execute(args);
        }
    }
}
