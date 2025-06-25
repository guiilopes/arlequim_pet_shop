using FluentMigrator;

namespace ArlequimPetShop.Migrations
{
    [Migration(20250625185600)]
    public class CreateTableSale : Migration
    {
        public override void Up()
        {
            Create.Table("Sale")
                .WithColumn("Id").AsGuid().PrimaryKey().Unique()
                .WithColumn("ClientId").AsGuid().NotNullable().ForeignKey("FK_Client_Sale", "Client", "Id")
                .WithColumn("CreatedOn").AsDateTime().NotNullable()
                .WithColumn("UpdatedOn").AsDateTime().NotNullable()
                .WithColumn("DeletedOn").AsDateTime().Nullable();

            Create.Table("SaleProduct")
                .WithColumn("Id").AsInt32().Unique().PrimaryKey().Identity()
                .WithColumn("SaleId").AsGuid().NotNullable().ForeignKey("FK_SaleProduct_Sale", "Sale", "Id")
                .WithColumn("ProductId").AsGuid().NotNullable().ForeignKey("FK_SaleProduct_Product", "Product", "Id")
                .WithColumn("Quantity").AsDecimal(20, 10).NotNullable()
                .WithColumn("Price").AsDecimal(20, 10).NotNullable()
                .WithColumn("Discount").AsDecimal(20, 10).NotNullable()
                .WithColumn("NetPrice").AsDecimal(20, 10).NotNullable()
                .WithColumn("CreatedOn").AsDateTime().NotNullable()
                .WithColumn("UpdatedOn").AsDateTime().NotNullable()
                .WithColumn("DeletedOn").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Table("SaleProduct");
            Delete.Table("Sale");
        }
    }
}