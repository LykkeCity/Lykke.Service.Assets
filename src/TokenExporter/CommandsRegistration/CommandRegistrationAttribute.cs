using System;

namespace TokenExporter.CommandsRegistration
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandRegistrationAttribute : Attribute
    {
        public CommandRegistrationAttribute(string commandName)
        {
            CommandName = commandName;
        }

        public string CommandName { get; }
    }
}
