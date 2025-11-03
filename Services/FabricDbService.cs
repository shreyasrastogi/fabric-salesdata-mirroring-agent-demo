using System.Data;
using Microsoft.Data.SqlClient;
using Azure.Identity;

namespace FabricWebApp.Services
{
    public class FabricDbService
    {
        private readonly string _connectionString;
        private readonly DefaultAzureCredential _credential = new();

        public FabricDbService(string server, string database)
        {
            _connectionString = new SqlConnectionStringBuilder
            {
                DataSource = server,
                InitialCatalog = database,
                Encrypt = true,
                TrustServerCertificate = false
            }.ConnectionString;
        }

        private async Task<SqlConnection> GetConnectionAsync()
        {
            var conn = new SqlConnection(_connectionString);
            var token = await _credential.GetTokenAsync(
                new Azure.Core.TokenRequestContext(
                    new[] { "https://database.windows.net/.default" }));
            conn.AccessToken = token.Token;
            await conn.OpenAsync();
            return conn;
        }

        public async Task<DataTable> GetTableDataAsync(string tableName)
        {
            using var conn = await GetConnectionAsync();
            string query = $"SELECT TOP 10 * FROM {tableName}";
            using var cmd = new SqlCommand(query, conn);
            using var reader = await cmd.ExecuteReaderAsync();

            var dataTable = new DataTable();
            dataTable.Load(reader);
            return dataTable;
        }
    }
}
