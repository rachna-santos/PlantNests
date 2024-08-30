using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantNests.Migrations
{
    public partial class cfffffff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ordernumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productId = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    subtotal = table.Column<int>(type: "int", nullable: false),
                    totalamount = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Lastmodifield = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_orders_customers_Id",
                        column: x => x.Id,
                        principalTable: "customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_orders_products_productId",
                        column: x => x.productId,
                        principalTable: "products",
                        principalColumn: "productId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "ratings",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    review = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    productId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Lastmodifield = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ratings", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_ratings_customers_Id",
                        column: x => x.Id,
                        principalTable: "customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_ratings_products_productId",
                        column: x => x.productId,
                        principalTable: "products",
                        principalColumn: "productId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "inventories",
                columns: table => new
                {
                    inventoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    productId = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    Inqty = table.Column<int>(type: "int", nullable: false),
                    totalqty = table.Column<int>(type: "int", nullable: false),
                    totalamount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Lastmodifield = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventories", x => x.inventoryId);
                    table.ForeignKey(
                        name: "FK_inventories_customers_Id",
                        column: x => x.Id,
                        principalTable: "customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_inventories_orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_inventories_products_productId",
                        column: x => x.productId,
                        principalTable: "products",
                        principalColumn: "productId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_inventories_Id",
                table: "inventories",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_inventories_OrderId",
                table: "inventories",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_inventories_productId",
                table: "inventories",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_Id",
                table: "orders",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_productId",
                table: "orders",
                column: "productId");

            migrationBuilder.CreateIndex(
                name: "IX_ratings_Id",
                table: "ratings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ratings_productId",
                table: "ratings",
                column: "productId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inventories");

            migrationBuilder.DropTable(
                name: "ratings");

            migrationBuilder.DropTable(
                name: "orders");
        }
    }
}
