using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedAddressesToOrderAndBasket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BillingAddressId",
                table: "Order",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShippingAddressId",
                table: "Order",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

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
                name: "IX_Order_BillingAddressId",
                table: "Order",
                column: "BillingAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_ShippingAddressId",
                table: "Order",
                column: "ShippingAddressId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Address_BillingAddressId",
                table: "Order",
                column: "BillingAddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Address_ShippingAddressId",
                table: "Order",
                column: "ShippingAddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Basket_Address_BillingAddressId",
                table: "Basket");

            migrationBuilder.DropForeignKey(
                name: "FK_Basket_Address_ShippingAddressId",
                table: "Basket");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Address_BillingAddressId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_Address_ShippingAddressId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_BillingAddressId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_ShippingAddressId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Basket_BillingAddressId",
                table: "Basket");

            migrationBuilder.DropIndex(
                name: "IX_Basket_ShippingAddressId",
                table: "Basket");

            migrationBuilder.DropColumn(
                name: "BillingAddressId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "ShippingAddressId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "BillingAddressId",
                table: "Basket");

            migrationBuilder.DropColumn(
                name: "ShippingAddressId",
                table: "Basket");
        }
    }
}
