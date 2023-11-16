using FluentMigrator;

namespace Api.Infra.Migrations
{
    [Migration(202311160118)]
    public class _202311160118_Create_Table_Test : Migration
    {
        public override void Down() { }

        public override void Up()
        {
            if (Schema.Table("Test").Exists())
                Delete.Table("Test");

            Create.Table("Test")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey()
                .WithColumn("Text").AsString().WithDefaultValue(string.Empty);
        }
    }
}
