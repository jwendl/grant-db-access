using Spectre.Console.Cli;

namespace GrantDatabase.CommandLine.Commands.Settings
{
    public class CreateUserSettings
        : SqlCommandSettings
    {
        [CommandArgument(2, "<Managed Identity Name>")]
        public string ManagedIdentityName { get; set; } = string.Empty!;
    }
}
