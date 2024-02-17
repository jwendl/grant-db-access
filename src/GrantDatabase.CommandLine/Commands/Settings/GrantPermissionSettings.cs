using Spectre.Console.Cli;

namespace GrantDatabase.CommandLine.Commands.Settings
{
	public class GrantPermissionSettings
		: SqlCommandSettings
	{
		[CommandArgument(3, "<Permission>")]
		public string Permission { get; set; } = string.Empty;

		[CommandArgument(4, "<Principal>")]
		public string Principal { get; set; } = string.Empty;
	}
}
