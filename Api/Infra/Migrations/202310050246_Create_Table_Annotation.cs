using FluentMigrator;

namespace Api.Infra.Migrations
{
    [Migration(202310050246)]
    public class _202310050246_Create_Table_Annotation : Migration
    {
        public override void Down() { }

        public override void Up()
        {
            if (Schema.Table("Annotation").Exists())
                Delete.Table("Annotation");

            Create.Table("Annotation")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey()
                .WithColumn("Title").AsString(100)
                .WithColumn("Text").AsString()
                .WithColumn("CreationDate").AsDateTime()
                .WithColumn("LastChange").AsDateTime()
                .WithColumn("UserId").AsInt32();
        }
    }
}
