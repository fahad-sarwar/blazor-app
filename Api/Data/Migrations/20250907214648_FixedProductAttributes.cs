using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixedProductAttributes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttribute_Product_ProductId",
                table: "ProductAttribute");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductAttribute",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttribute_Product_ProductId",
                table: "ProductAttribute",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductAttribute_Product_ProductId",
                table: "ProductAttribute");

            migrationBuilder.AlterColumn<int>(
                name: "ProductId",
                table: "ProductAttribute",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductAttribute_Product_ProductId",
                table: "ProductAttribute",
                column: "ProductId",
                principalTable: "Product",
                principalColumn: "Id");
        }
    }
}
