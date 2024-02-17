using GrantDatabase.CommandLine.Commands;
using Spectre.Console.Cli;

var commandApplication = new CommandApp();
commandApplication.Configure(config =>
{
    config.ValidateExamples();

    config.AddCommand<CreateUserCommand>("user")
        .WithDescription("Creates a user managed identity in the database")
        .WithExample(new[] { "user" });

    config.AddCommand<AlterRoleCommand>("role")
        .WithDescription("Alters a role for a given user")
        .WithExample(new[] { "role" });

	config.AddCommand<GrantPermissionCommand>("grant")
	    .WithDescription("Grants a permission for a given principal")
	    .WithExample(new[] { "grant" });
});

await commandApplication.RunAsync(args);
