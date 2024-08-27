using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UF5423_SuperShop.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) //'MigrationBuilder': design pattern.
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductLastPurchase = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductLastSale = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProductIsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    ProductStock = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
