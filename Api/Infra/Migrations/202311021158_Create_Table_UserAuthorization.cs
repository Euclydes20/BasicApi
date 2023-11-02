using FluentMigrator;

namespace Api.Infra.Migrations
{
    [Migration(202311021158)]
    public class _202311021158_Create_Table_UserAuthorization : Migration
    {
        public override void Down() { }

        public override void Up()
        {
            if (Schema.Table("UserAuthorization").Exists())
                Delete.Table("UserAuthorization");

            Create.Table("UserAuthorization")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey()
                .WithColumn("UserId").AsInt32()
                .WithColumn("AuthorizationCode").AsString(50)
                .WithColumn("AuthorizationCondition").AsInt32();
        }
    }
}
