using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlantNests.Migrations
{
    public partial class customers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_shoppingCarts_Id",
                table: "shoppingCarts",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_shoppingCarts_customers_Id",
                table: "shoppingCarts",
                column: "Id",
                principalTable: "customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_shoppingCarts_customers_Id",
                table: "shoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_shoppingCarts_Id",
                table: "shoppingCarts");
        }
    }
}
