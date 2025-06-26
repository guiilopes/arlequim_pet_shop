using FluentMigrator;

namespace ArlequimPetShop.Migrations
{
    [Migration(20250625152000)]
    public class CreateTableProductStockHistory : Migration
    {
        public override void Up()
        {
            Create.Table("ProductHistory")
                .WithColumn("Id").AsInt32().PrimaryKey().Unique().Identity()
                .WithColumn("ProductId").AsGuid().NotNullable().ForeignKey("FK_ProductHistory_Product", "Product", "Id")
                .WithColumn("Description").AsString(int.MaxValue).Nullable()
                .WithColumn("Quantity").AsDecimal(20, 10).NotNullable()
                .WithColumn("DocumentFiscalNumber").AsString(100).Nullable()
                .WithColumn("CreatedOn").AsDateTime().NotNullable()
                .WithColumn("UpdatedOn").AsDateTime().NotNullable()
                .WithColumn("DeletedOn").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Table("ProductHistory");
        }
    }
}