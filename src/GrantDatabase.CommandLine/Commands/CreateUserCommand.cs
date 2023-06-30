using Azure.Core;
using Azure.Identity;
using GrantDatabase.CommandLine.Commands.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;

namespace GrantDatabase.CommandLine.Commands
{
    public class CreateUserCommand
        : AsyncCommand<CreateUserSettings>
    {
        private readonly ILogger<CreateUserCommand> _logger;

        public CreateUserCommand(ILogger<CreateUserCommand> logger)
        {
            _logger = logger;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, CreateUserSettings settings)
        {
            _logger.LogInformation("Running user command for {user}", settings.ManagedIdentityName);

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
            sqlCommand.CommandText = $"DROP USER IF EXISTS [{settings.ManagedIdentityName}]; CREATE USER [{settings.ManagedIdentityName}] FROM EXTERNAL PROVIDER;";

            await sqlCommand.ExecuteNonQueryAsync();
            return 0;
        }
    }
}
