using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Payment.API.Migrations
{
    public partial class MaskedPaymentDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "PaymentDetails",
                newName: "SecurityCode");

            migrationBuilder.AddColumn<string>(
                name: "CardHolder",
                table: "PaymentDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CreditCardNumber",
                table: "PaymentDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpirationDate",
                table: "PaymentDetails",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RequestId",
                table: "PaymentDetails",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardHolder",
                table: "PaymentDetails");

            migrationBuilder.DropColumn(
                name: "CreditCardNumber",
                table: "PaymentDetails");

            migrationBuilder.DropColumn(
                name: "ExpirationDate",
                table: "PaymentDetails");

            migrationBuilder.DropColumn(
                name: "RequestId",
                table: "PaymentDetails");

            migrationBuilder.RenameColumn(
                name: "SecurityCode",
                table: "PaymentDetails",
                newName: "Name");
        }
    }
}
