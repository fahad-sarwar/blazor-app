using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Data.Migrations
{
    /// <inheritdoc />
    public partial class CustomerNullAddresses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Address_BillingAddressId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Address_ShippingAddressId",
                table: "Customer");

            migrationBuilder.AlterColumn<int>(
                name: "ShippingAddressId",
                table: "Customer",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "BillingAddressId",
                table: "Customer",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Address_BillingAddressId",
                table: "Customer",
                column: "BillingAddressId",
                principalTable: "Address",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Address_ShippingAddressId",
                table: "Customer",
                column: "ShippingAddressId",
                principalTable: "Address",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Address_BillingAddressId",
                table: "Customer");

            migrationBuilder.DropForeignKey(
                name: "FK_Customer_Address_ShippingAddressId",
                table: "Customer");

            migrationBuilder.AlterColumn<int>(
                name: "ShippingAddressId",
                table: "Customer",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BillingAddressId",
                table: "Customer",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Address_BillingAddressId",
                table: "Customer",
                column: "BillingAddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Customer_Address_ShippingAddressId",
                table: "Customer",
                column: "ShippingAddressId",
                principalTable: "Address",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
