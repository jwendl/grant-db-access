using GrantDatabase.CommandLine.Commands;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using Spectre.Console.Cli.Extensions.DependencyInjection;

var serviceCollection = new ServiceCollection();

serviceCollection.AddLogging(configure =>
{
    configure.AddInlineSpectreConsole();
});

var serviceProvider = serviceCollection.BuildServiceProvider();

using var registrar = new DependencyInjectionRegistrar(serviceCollection);
var commandApplication = new CommandApp(registrar);
commandApplication.Configure(config =>
{
    config.ValidateExamples();

    config.AddCommand<CreateUserCommand>("user")
        .WithDescription("Creates a user managed identity in the database")
        .WithExample(new[] { "user" });

    config.AddCommand<AlterRoleCommand>("role")
        .WithDescription("Alters a role for a given user")
        .WithExample(new[] { "role" });
});

await commandApplication.RunAsync(args);
