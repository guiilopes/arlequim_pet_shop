using FluentMigrator;

namespace ArlequimPetShop.Migrations
{
    [Migration(20250625185500)]
    public class CreateTableClient : Migration
    {
        public override void Up()
        {
            Create.Table("Client")
                .WithColumn("Id").AsGuid().PrimaryKey().Unique()
                .WithColumn("Name").AsString(500).Nullable()
                .WithColumn("Document").AsString(20).Nullable()
                .WithColumn("CreatedOn").AsDateTime().NotNullable()
                .WithColumn("UpdatedOn").AsDateTime().NotNullable()
                .WithColumn("DeletedOn").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Table("Client");
        }
    }
}