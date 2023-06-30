using Azure.Core;
using Azure.Identity;
using GrantDatabase.CommandLine.Commands.Settings;
using Microsoft.Data.SqlClient;
using Spectre.Console.Cli;

namespace GrantDatabase.CommandLine.Commands
{
    public class AlterRoleCommand
        : AsyncCommand<AlterRoleSettings>
    {
        public override async Task<int> ExecuteAsync(CommandContext context, AlterRoleSettings settings)
        {
            var defaultAzureCredential = new DefaultAzureCredential();
            var tokenRequestContext = new TokenRequestContext(new[] { "https://database.windows.net/.default" });
            var accessToken = await defaultAzureCredential.GetTokenAsync(tokenRequestContext);

            var connectionStringBuilder = new SqlConnectionStringBuilder
            {
                Authentication = SqlAuthenticationMethod.ActiveDirectoryManagedIdentity,
                DataSource = settings.SqlDatabaseName,
                InitialCatalog = settings.SqlDatabaseName,
                Encrypt = true
            };

            using var sqlConnection = new SqlConnection(connectionStringBuilder.ConnectionString);
            sqlConnection.AccessToken = accessToken.Token;

            using var sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = $"ALTER ROLE {settings.RoleName} ADD MEMBER [{settings.ManagedIdentityName}];";

            await sqlCommand.ExecuteNonQueryAsync();
            return 0;
        }
    }
}
