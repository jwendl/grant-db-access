using Azure.Core;
using Azure.Identity;
using GrantDatabase.CommandLine.Commands.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;

namespace GrantDatabase.CommandLine.Commands
{
    public class AlterRoleCommand
        : AsyncCommand<AlterRoleSettings>
    {
        private readonly ILogger<AlterRoleCommand> _logger;

        public AlterRoleCommand(ILogger<AlterRoleCommand> logger)
        {
            _logger = logger;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, AlterRoleSettings settings)
        {
            _logger.LogInformation("Running role command for {user} being added to {role}", settings.ManagedIdentityName, settings.RoleName);

            var defaultAzureCredential = new DefaultAzureCredential();
            var tokenRequestContext = new TokenRequestContext(new[] { "https://database.windows.net/.default" });
            var accessToken = await defaultAzureCredential.GetTokenAsync(tokenRequestContext);

            var connectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = settings.SqlDatabaseName,
                InitialCatalog = settings.SqlDatabaseName,
                Encrypt = true
            };

            using var sqlConnection = new SqlConnection(connectionStringBuilder.ConnectionString);
            sqlConnection.AccessToken = accessToken.Token;
            await sqlConnection.OpenAsync();

            using var sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = $"ALTER ROLE {settings.RoleName} ADD MEMBER [{settings.ManagedIdentityName}];";

            await sqlCommand.ExecuteNonQueryAsync();
            return 0;
        }
    }
}
