using Microsoft.EntityFrameworkCore.Migrations;

namespace UF5423_SuperShop.Migrations
{
    public partial class RenameProductColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductStock",
                table: "Products",
                newName: "Stock");

            migrationBuilder.RenameColumn(
                name: "ProductPrice",
                table: "Products",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "ProductName",
                table: "Products",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "ProductLastSale",
                table: "Products",
                newName: "LastSale");

            migrationBuilder.RenameColumn(
                name: "ProductLastPurchase",
                table: "Products",
                newName: "LastPurchase");

            migrationBuilder.RenameColumn(
                name: "ProductIsAvailable",
                table: "Products",
                newName: "IsAvailable");

            migrationBuilder.RenameColumn(
                name: "ProductImageUrl",
                table: "Products",
                newName: "ImageUrl");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "Products",
                newName: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Stock",
                table: "Products",
                newName: "ProductStock");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "ProductPrice");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Products",
                newName: "ProductName");

            migrationBuilder.RenameColumn(
                name: "LastSale",
                table: "Products",
                newName: "ProductLastSale");

            migrationBuilder.RenameColumn(
                name: "LastPurchase",
                table: "Products",
                newName: "ProductLastPurchase");

            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "Products",
                newName: "ProductIsAvailable");

            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Products",
                newName: "ProductImageUrl");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Products",
                newName: "ProductId");
        }
    }
}
