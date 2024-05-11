using Api.Infra.Database;
using FluentMigrator;

namespace Api.Infra.Migrations
{
    [Migration(202404222311)]
    public class _202404222311_Create_Extension_Unaccent : Migration
    {
        public override void Down()
        {
            if (DatabaseConnection.DatabaseType is DbType.Postgres)
                Execute.Sql("DROP EXTENSION IF EXISTS \"unaccent\"");
        }

        public override void Up()
        {
            if (DatabaseConnection.DatabaseType is DbType.Postgres)
                Execute.Sql("CREATE EXTENSION IF NOT EXISTS \"unaccent\"");
        }
    }
}
