using Spectre.Console.Cli;
using System.ComponentModel;

namespace GrantDatabase.CommandLine.Commands.Settings
{
    public class AlterRoleSettings
        : SqlCommandSettings
    {
        [CommandArgument(3, "<Managed Identity Name>")]
        public string ManagedIdentityName { get; set; } = string.Empty;

        [CommandArgument(4, "<Role Name>")]
        public string RoleName { get; set; } = string.Empty;

		[CommandOption("--create")]
		[DefaultValue(false)]
		public bool CreateIfNotExists { get; set; }
	}
}
