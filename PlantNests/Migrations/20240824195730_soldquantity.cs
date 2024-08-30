using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantNests.Migrations
{
    public partial class soldquantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "soldProducts",
                columns: table => new
                {
                    soldid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    productId = table.Column<int>(type: "int", nullable: false),
                    quantitysold = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                name: "IX_soldProducts_OrderId",
                table: "soldProducts",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_soldProducts_productId",
                table: "soldProducts",
                column: "productId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "soldProducts");
        }
    }
}
