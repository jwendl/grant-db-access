using Azure.Core;
using Azure.Identity;
using GrantDatabase.CommandLine.Commands.Settings;
using Microsoft.Data.SqlClient;
using Spectre.Console.Cli;

namespace GrantDatabase.CommandLine.Commands
{
	public class GrantPermissionCommand
		: AsyncCommand<GrantPermissionSettings>
	{
		public override async Task<int> ExecuteAsync(CommandContext context, GrantPermissionSettings settings)
		{
			Console.WriteLine("Running grant command for {0} being given to {1}", settings.Permission, settings.Principal);

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
			sqlCommand.CommandText = $"GRANT {settings.Permission} TO [{settings.Principal}];";

			await sqlCommand.ExecuteNonQueryAsync();
			return 0;

		}
	}
}
