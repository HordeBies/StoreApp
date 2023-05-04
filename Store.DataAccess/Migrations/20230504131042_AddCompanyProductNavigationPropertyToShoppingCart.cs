using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Store.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyProductNavigationPropertyToShoppingCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ShoppingCarts_CompanyId",
                table: "ShoppingCarts");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_CompanyId_ProductId",
                table: "ShoppingCarts",
                columns: new[] { "CompanyId", "ProductId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCarts_CompanyProducts_CompanyId_ProductId",
                table: "ShoppingCarts",
                columns: new[] { "CompanyId", "ProductId" },
                principalTable: "CompanyProducts",
                principalColumns: new[] { "CompanyId", "ProductId" },
                onDelete: ReferentialAction.NoAction,
                onUpdate: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCarts_CompanyProducts_CompanyId_ProductId",
                table: "ShoppingCarts");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCarts_CompanyId_ProductId",
                table: "ShoppingCarts");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_CompanyId",
                table: "ShoppingCarts",
                column: "CompanyId");
        }
    }
}
