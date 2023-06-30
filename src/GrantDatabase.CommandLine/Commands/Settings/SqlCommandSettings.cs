using Spectre.Console.Cli;

namespace GrantDatabase.CommandLine.Commands.Settings
{
    public class SqlCommandSettings
        : CommandSettings
    {
        [CommandArgument(0, "<SQL Server Host Name>")]
        public string SqlServerHostName { get; set; } = string.Empty;

        [CommandArgument(1, "<SQL Database Name>")]
        public string SqlDatabaseName { get; set; } = string.Empty;
    }
}
