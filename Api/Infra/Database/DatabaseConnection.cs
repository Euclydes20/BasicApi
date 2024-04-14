using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using Npgsql;
using System.Text;

namespace Api.Infra.Database
{
    public class DatabaseConnection
    {
        internal static readonly string ConnectionStringPropertyName = "Default";

        internal static string Server { get; private set; } = string.Empty;
        internal static string User { get; private set; } = string.Empty;
        internal static string Password { get; private set; } = string.Empty;
        internal static string DatabaseName { get; private set; } = string.Empty;
        internal static int Port { get; private set; } = 0;
        internal static string SSLMode { get; private set; } = string.Empty;
        internal static string EntityAdminDatabase { get; private set; } = string.Empty;
        internal static int Timeout { get; private set; } = 0;
        internal static int CommandTimeout { get; private set; } = 0;

        internal static DbType DatabaseType { get; private set; } = DbType.Nenhum;

        internal static void LoadDatabaseConfig(DbType databaseType)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json");

            var apiConfig = builder.Build()
                ?? throw new Exception("Configuração da API inacessível.");

            string connectionString = ConfigurationExtensions.GetConnectionString(apiConfig, ConnectionStringPropertyName)
                ?? throw new Exception("String de conexão não localizada.");

            DatabaseType = databaseType;

            DbConnectionStringBuilder dbConnectionStringBuilder = GetStringBuilder(connectionString);

            Server = (databaseType is DbType.Postgres ? dbConnectionStringBuilder["host"].ToString() : dbConnectionStringBuilder["server"].ToString()) ?? string.Empty;
            DatabaseName = dbConnectionStringBuilder["database"].ToString() ?? string.Empty;
            User = (DatabaseType is DbType.SAPHana ? dbConnectionStringBuilder["UserName"].ToString() : dbConnectionStringBuilder["user id"].ToString()) ?? string.Empty; ;
            Password = dbConnectionStringBuilder["password"].ToString() ?? string.Empty;
            Port = dbConnectionStringBuilder.ContainsKey("port") ? Convert.ToInt32(dbConnectionStringBuilder["port"]) : 0;
            SSLMode = (dbConnectionStringBuilder.ContainsKey("SSL Mode") ? dbConnectionStringBuilder["SSL Mode"].ToString() : string.Empty) ?? string.Empty;
            EntityAdminDatabase = (dbConnectionStringBuilder.ContainsKey("EF Admin Database") ? dbConnectionStringBuilder["EF Admin Database"].ToString() : string.Empty) ?? string.Empty;
            Timeout = Math.Max(dbConnectionStringBuilder.ContainsKey("Timeout") ? Convert.ToInt32(dbConnectionStringBuilder["Timeout"]) : 60, 60);
            CommandTimeout = Timeout;

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        internal static void CreateDatabase()
        {
            switch (DatabaseType)
            {
                case DbType.SqlServer: 
                    CreateSqlDatabase();
                    break;
                case DbType.Postgres: 
                    CreatePostgresDatabase();
                    break;
                case DbType.SAPHana: throw new Exception("Database SapHana não está disponível atualmente."); //return new HanaConnectionStringBuilder(connectionstring);
                case DbType.Nenhum: throw new Exception("Database não informado.");
                default: throw new NotImplementedException();
            };
        }

        private static DbConnectionStringBuilder GetStringBuilder(string connectionstring)
        {
            return DatabaseType switch
            {
                DbType.SqlServer => new SqlConnectionStringBuilder(connectionstring),
                DbType.Postgres => new NpgsqlConnectionStringBuilder(connectionstring),
                DbType.SAPHana => throw new Exception("Database SapHana não está disponível atualmente."), //return new HanaConnectionStringBuilder(connectionstring);
                DbType.Nenhum => throw new Exception("Database não informado."),
                _ => throw new NotImplementedException(),
            };
        }

        private static void CreateSqlDatabase()
        {
            if (ExistsSqlDatabase())
                return;

            var newServerInfoWithMasterDatabase = GetConnectionString(true);

            using (var conn = new SqlConnection(newServerInfoWithMasterDatabase.ToString()))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = $"CREATE DATABASE \"{DatabaseName}\"";

                command.ExecuteNonQuery();

                conn.Close();
            }
        }

        private static bool ExistsSqlDatabase()
        {
            var newServerInfoWithMasterDatabase = GetConnectionString(true);

            using (var conn = new SqlConnection(newServerInfoWithMasterDatabase.ToString()))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = $"SELECT NAME FROM SYS.DATABASES WHERE NAME='{DatabaseName}'";

                var reader = command.ExecuteReader();
                var dataTable = new DataTable();
                dataTable.Load(reader);
                return dataTable.Rows.Count > 0;
            }
        }

        private static void CreatePostgresDatabase()
        {
            if (ExistsPostgresDatabase())
                return;

            var newServerInfoWithMasterDatabase = GetConnectionString(true);

            using (var conn = new NpgsqlConnection(newServerInfoWithMasterDatabase))
            {
                conn.Open();

                var command = conn.CreateCommand();
                command.CommandText = $"CREATE DATABASE \"{DatabaseName}\" ";

                command.ExecuteNonQuery();

                conn.Close();
            }
        }

        private static bool ExistsPostgresDatabase()
        {
            var newServerInfoWithMasterDatabase = GetConnectionString(true);

            using (var conn = new NpgsqlConnection(newServerInfoWithMasterDatabase))
            {
                conn.Open();

                var command = conn.CreateCommand();
                // * -> datname, pois em novas versões do Postgresql e Npgsql, um seguinte erro é apresentado:
                // "Reading as 'System.Object' is not supported for fields having DataTypeName 'aclitem[]'"
                command.CommandText = $"SELECT datname FROM pg_database WHERE datname = '{DatabaseName}'";

                var reader = command.ExecuteReader();
                var data = new DataTable();
                data.Load(reader);

                return data.Rows.Count > 0;
            }
        }

        internal static string GetConnectionString(bool master = false, bool noDatabase = false)
        {
            string connectionString;

            switch (DatabaseType)
            {
                case DbType.Postgres:
                    connectionString = $"host={Server}; database={(master ? "postgres" : DatabaseName)}; user id={User}; password={Password}";
                    if (Port > 0)
                        connectionString += $";port={Port};";
                    if (Timeout > 0)
                        connectionString += $"Timeout={Timeout};CommandTimeout={Timeout};";
                    if (!string.IsNullOrEmpty(SSLMode))
                        connectionString += $"SSL Mode={SSLMode};";
                    if (!string.IsNullOrEmpty(EntityAdminDatabase))
                        connectionString += $"EntityAdminDatabase={EntityAdminDatabase};";
                    return connectionString;
                case DbType.SqlServer:
                    connectionString = $"server={Server}; database={(master ? "master" : DatabaseName)}; user id={User}; password={Password}";
                    if (Port != 0)
                        return connectionString + $" port={Port};";
                    return connectionString;
                case DbType.SAPHana:
                    connectionString = $"server={Server}; database={(noDatabase ? string.Empty : (master ? "SYSTEM" : DatabaseName))}; user id={User}; password={Password}";
                    if (string.IsNullOrEmpty(DatabaseName))
                        return GetHanaConnectionStringWithoutDatabaseTag(connectionString);
                    if (Port != 0)
                        return connectionString + $" port={Port};";
                    return connectionString;
                default:
                    connectionString = $"server={Server}; databaseName={DatabaseName}; userId={User}; password={Password}";
                    if (Port != 0)
                        return connectionString.Replace(Server, $"{Server}:{Port}");
                    return connectionString;

            }
        }

        private static string GetHanaConnectionStringWithoutDatabaseTag(string connectionString)
        {
            var stringBuilder = new StringBuilder();
            foreach (var item in connectionString.Split(';'))
            {
                if (!item.ToLower().StartsWith("database"))
                    continue;

                stringBuilder.Append(item);
            }

            return stringBuilder.ToString();
        }
    }

    internal enum DbType
    {
        Nenhum = 0,
        SqlServer,
        Postgres,
        SAPHana
    }
}
