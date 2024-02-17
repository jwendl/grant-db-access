using Azure.Core;
using Azure.Identity;
using GrantDatabase.CommandLine.Commands.Settings;
using Microsoft.Data.SqlClient;
using Spectre.Console.Cli;
using System.Text;

namespace GrantDatabase.CommandLine.Commands
{
    public class AlterRoleCommand
        : AsyncCommand<AlterRoleSettings>
    {
        public override async Task<int> ExecuteAsync(CommandContext context, AlterRoleSettings settings)
        {
            Console.WriteLine("Running role command for {0} being added to {1}", settings.ManagedIdentityName, settings.RoleName);

            var defaultAzureCredentialOptions = new DefaultAzureCredentialOptions
            {
                ManagedIdentityClientId = settings.SqlAdminManagedIdentityClientId,
            };

            var defaultAzureCredential = new DefaultAzureCredential(defaultAzureCredentialOptions);
            var tokenRequestContext = new TokenRequestContext(new[] { "https://database.windows.net/.default" });
            var accessToken = await defaultAzureCredential.GetTokenAsync(tokenRequestContext);

            var connectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = settings.SqlServerHostName,
                InitialCatalog = settings.SqlDatabaseName,
                Encrypt = true
            };

            using var sqlConnection = new SqlConnection(connectionStringBuilder.ConnectionString);
            sqlConnection.AccessToken = accessToken.Token;
            await sqlConnection.OpenAsync();

            using var sqlCommand = sqlConnection.CreateCommand();

            var command = new StringBuilder();

            if (settings.CreateIfNotExists)
            {
                command.AppendLine($"IF DATABASE_PRINCIPAL_ID('{settings.RoleName}') IS NULL CREATE ROLE [{settings.RoleName}]");
            }

            command.AppendLine($"ALTER ROLE {settings.RoleName} ADD MEMBER [{settings.ManagedIdentityName}];");

            sqlCommand.CommandText = command.ToString();

            await sqlCommand.ExecuteNonQueryAsync();
            return 0;
        }
    }
}
