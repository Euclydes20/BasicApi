using Api.Security;
using FluentMigrator;

namespace Api.Infra.Migrations
{
    [Migration(202310061620)]
    public class _202310061620_Create_Table_User : Migration
    {
        public override void Down() { }

        public override void Up()
        {
            if (Schema.Table("User").Exists())
                Delete.Table("User");

            Create.Table("User")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey()
                .WithColumn("Name").AsString(100)
                .WithColumn("Login").AsString(30)
                .WithColumn("Password").AsString(65)
                .WithColumn("Bibliography").AsString(500)
                .WithColumn("Super").AsBoolean()
                .WithColumn("ProvisoryPassword").AsBoolean()
                .WithColumn("CreationDate").AsDateTime()
                .WithColumn("LastLogin").AsDateTime().Nullable()
                .WithColumn("Blocked").AsBoolean();
        }
    }
}
