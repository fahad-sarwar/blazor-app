using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedBasketFromOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Basket_BasketId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_BasketId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "BasketId",
                table: "Order");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BasketId",
                table: "Order",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Order_BasketId",
                table: "Order",
                column: "BasketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Basket_BasketId",
                table: "Order",
                column: "BasketId",
                principalTable: "Basket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
