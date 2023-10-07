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
                .WithColumn("Password").AsString()
                .WithColumn("Super").AsBoolean()
                .WithColumn("ProvisoryPassword").AsBoolean()
                .WithColumn("CreationDate").AsDateTime()
                .WithColumn("LastLogin").AsDateTime().Nullable()
                .WithColumn("Blocked").AsBoolean();
        }
    }

    [Migration(202310061810)]
    public class _202310061810_Register_AdmUser : Migration
    {
        public override void Down() { }

        public override void Up()
        {
            Insert.IntoTable("User")
                .Row(new
                {
                    Name = "Administrador",
                    Login = "Adm",
                    Password = "123".Hash(),
                    Super = true,
                    ProvisoryPassword = false,
                    CreationDate = DateTime.Now,
                    Blocked = false
                });
        }
    }
}
