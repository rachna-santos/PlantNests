using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantNests.Migrations
{
    public partial class inventorys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_inventories_customers_Id",
                table: "inventories");

            migrationBuilder.DropTable(
                name: "soldProducts");

            migrationBuilder.DropIndex(
                name: "IX_inventories_Id",
                table: "inventories");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "inventories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "inventories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "soldProducts",
                columns: table => new
                {
                    soldid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    productId = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<int>(type: "int", nullable: false),
                    quantitysold = table.Column<int>(type: "int", nullable: false),
                    solddate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_soldProducts", x => x.soldid);
                    table.ForeignKey(
                        name: "FK_soldProducts_orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_soldProducts_products_productId",
                        column: x => x.productId,
                        principalTable: "products",
                        principalColumn: "productId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_inventories_Id",
                table: "inventories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_soldProducts_OrderId",
                table: "soldProducts",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_soldProducts_productId",
                table: "soldProducts",
                column: "productId");

            migrationBuilder.AddForeignKey(
                name: "FK_inventories_customers_Id",
                table: "inventories",
                column: "Id",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
