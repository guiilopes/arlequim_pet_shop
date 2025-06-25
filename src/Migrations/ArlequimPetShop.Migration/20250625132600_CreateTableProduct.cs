using FluentMigrator;

namespace ArlequimPetShop.Migrations
{
    [Migration(20250625132600)]
    public class CreateTableProduct : Migration
    {
        public override void Up()
        {
            Create.Table("Product")
                .WithColumn("Id").AsGuid().PrimaryKey().Unique()
                .WithColumn("Barcode").AsString(50).NotNullable()
                .WithColumn("Name").AsString(500).NotNullable()
                .WithColumn("Description").AsString(1000).NotNullable()
                .WithColumn("Price").AsDecimal(20, 10).NotNullable()
                .WithColumn("ExpirationDate").AsDateTime().Nullable()
                .WithColumn("CreatedOn").AsDateTime().NotNullable()
                .WithColumn("UpdatedOn").AsDateTime().NotNullable()
                .WithColumn("DeletedOn").AsDateTime().Nullable();

            Create.Table("ProductStock")
                .WithColumn("Id").AsInt32().Unique().PrimaryKey().Identity()
                .WithColumn("ProductId").AsGuid().NotNullable().ForeignKey("FK_ProductStock_Product", "Product", "Id")
                .WithColumn("Quantity").AsDecimal(20, 10).NotNullable()
                .WithColumn("CreatedOn").AsDateTime().NotNullable()
                .WithColumn("UpdatedOn").AsDateTime().NotNullable()
                .WithColumn("DeletedOn").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Table("ProductStock");
            Delete.Table("Product");
        }
    }
}