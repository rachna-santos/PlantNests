using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantNests.Migrations
{
    public partial class addpayments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardPayment",
                table: "paymenttypes");

            migrationBuilder.RenameColumn(
                name: "CashPayment",
                table: "paymenttypes",
                newName: "PaymentType");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentType",
                table: "paymenttypes",
                newName: "CashPayment");

            migrationBuilder.AddColumn<string>(
                name: "CardPayment",
                table: "paymenttypes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
