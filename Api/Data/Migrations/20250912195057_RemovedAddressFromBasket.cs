using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedAddressFromBasket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Basket_Address_BillingAddressId",
                table: "Basket");

            migrationBuilder.DropForeignKey(
                name: "FK_Basket_Address_ShippingAddressId",
                table: "Basket");

            migrationBuilder.DropIndex(
                name: "IX_Basket_BillingAddressId",
                table: "Basket");

            migrationBuilder.DropIndex(
                name: "IX_Basket_ShippingAddressId",
                table: "Basket");

            migrationBuilder.DropColumn(
                name: "BillingAddressId",
                table: "Basket");

            migrationBuilder.DropColumn(
                name: "ShippingAddressId",
                table: "Basket");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BillingAddressId",
                table: "Basket",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShippingAddressId",
                table: "Basket",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Basket_BillingAddressId",
                table: "Basket",
                column: "BillingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Basket_ShippingAddressId",
                table: "Basket",
                column: "ShippingAddressId");

            migrationBuilder.AddForeignKey(
                name: "FK_Basket_Address_BillingAddressId",
                table: "Basket",
                column: "BillingAddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Basket_Address_ShippingAddressId",
                table: "Basket",
                column: "ShippingAddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
