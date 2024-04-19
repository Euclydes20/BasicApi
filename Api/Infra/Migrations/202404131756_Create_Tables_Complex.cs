using FluentMigrator;

namespace Api.Infra.Migrations
{
    [Migration(202404131758)]
    public class _202404131758_Create_Tables_Complex : Migration
    {
        public override void Down() { }

        public override void Up()
        {
            if (Schema.Table("ComplexPrincipal").Exists())
                Delete.Table("ComplexPrincipal");

            if (Schema.Table("ComplexAggregate").Exists())
                Delete.Table("ComplexAggregate");

            if (Schema.Table("ComplexSubAggregate").Exists())
                Delete.Table("ComplexSubAggregate");

            if (Schema.Table("ComplexSimpleFK").Exists())
                Delete.Table("ComplexSimpleFK");

            Create.Table("ComplexPrincipal")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey()
                .WithColumn("ComplexSimpleFKId").AsInt32()
                .WithColumn("Description").AsString();

            Create.Table("ComplexAggregate")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey()
                .WithColumn("ComplexPrincipalId").AsInt32()
                .WithColumn("Description").AsString();

            Create.Table("ComplexSubAggregate")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey()
                .WithColumn("ComplexAggregateId").AsInt32()
                .WithColumn("Description").AsString();

            Create.Table("ComplexSimpleFK")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey()
                .WithColumn("Description").AsString();

            Insert.IntoTable("ComplexSimpleFK")
                .Row(new { Id = 1, Description = "Primeira FK" })
                .Row(new { Id = 2, Description = "Segunda FK" })
                .Row(new { Id = 3, Description = "Terceira FK" });
        }
    }
}
