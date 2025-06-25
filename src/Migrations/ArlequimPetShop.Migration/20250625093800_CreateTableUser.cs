using FluentMigrator;

namespace ArlequimPetShop.Migrations
{
    [Migration(20250625093800)]
    public class CreateTableUser : Migration
    {
        public override void Up()
        {
            Create.Table("User")
                .WithColumn("Id").AsGuid().PrimaryKey().Unique()
                .WithColumn("Type").AsString(100).NotNullable()
                .WithColumn("Name").AsString(200).NotNullable()
                .WithColumn("Email").AsString(200).NotNullable()
                .WithColumn("Password").AsString(200).NotNullable()
                .WithColumn("CreatedOn").AsDateTime().NotNullable()
                .WithColumn("UpdatedOn").AsDateTime().NotNullable()
                .WithColumn("DeletedOn").AsDateTime().Nullable();

            Create.Table("UserLogin")
                .WithColumn("Id").AsInt32().Unique().PrimaryKey().Identity()
                .WithColumn("UserId").AsGuid().NotNullable().ForeignKey("FK_UserLogin_User", "User", "Id")
                .WithColumn("Email").AsString(200).NotNullable()
                .WithColumn("CreatedOn").AsDateTime().NotNullable()
                .WithColumn("UpdatedOn").AsDateTime().NotNullable()
                .WithColumn("DeletedOn").AsDateTime().Nullable();
        }

        public override void Down()
        {
            Delete.Table("UserLogin");
            Delete.Table("User");
        }
    }
}