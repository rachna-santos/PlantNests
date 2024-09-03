using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantNests.Migrations
{
    public partial class inventory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_inventories_orders_OrderId",
                table: "inventories");

            migrationBuilder.DropIndex(
                name: "IX_inventories_OrderId",
                table: "inventories");

            migrationBuilder.DropColumn(
                name: "Inqty",
                table: "inventories");

            migrationBuilder.RenameColumn(
                name: "totalqty",
                table: "inventories",
                newName: "TotalQty");

            migrationBuilder.RenameColumn(
                name: "totalamount",
                table: "inventories",
                newName: "Totalamount");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "inventories",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "inventories",
                newName: "CurrentQty");

            migrationBuilder.AlterColumn<int>(
                name: "Totalamount",
                table: "inventories",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Totalamount",
                table: "inventories",
                newName: "totalamount");

            migrationBuilder.RenameColumn(
                name: "TotalQty",
                table: "inventories",
                newName: "totalqty");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "inventories",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "CurrentQty",
                table: "inventories",
                newName: "OrderId");

            migrationBuilder.AlterColumn<decimal>(
                name: "totalamount",
                table: "inventories",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Inqty",
                table: "inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_inventories_OrderId",
                table: "inventories",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_inventories_orders_OrderId",
                table: "inventories",
                column: "OrderId",
                principalTable: "orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
