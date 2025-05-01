using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesAnalyticsApi.Migrations
{
    /// <inheritdoc />
    public partial class RecreateTable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Products",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Region",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Category",
                table: "Products",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DateOfSale",
                table: "Orders",
                column: "DateOfSale");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DateOfSale_Product",
                table: "Orders",
                columns: new[] { "DateOfSale", "ProductId" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DateOfSale_Region",
                table: "Orders",
                columns: new[] { "DateOfSale", "Region" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_Category",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DateOfSale",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DateOfSale_Product",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_DateOfSale_Region",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "Category",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Region",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
