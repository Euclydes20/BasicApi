using FluentMigrator;

namespace Api.Infra.Migrations
{
    [Migration(202404131756)]
    public class _202404131756_Create_Tables_Complex : Migration
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

            Create.Table("ComplexPrincipal")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey()
                .WithColumn("Description").AsString();

            Create.Table("ComplexAggregate")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey()
                .WithColumn("ComplexPrincipalId").AsInt32()
                .WithColumn("Description").AsString();

            Create.Table("ComplexSubAggregate")
                .WithColumn("Id").AsInt32().Identity().PrimaryKey()
                .WithColumn("ComplexAggregateId").AsInt32()
                .WithColumn("Description").AsString();
        }
    }
}
