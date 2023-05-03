using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Store.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedCompanyProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CompanyProducts",
                columns: new[] { "CompanyId", "ProductId", "ListPrice", "Price" },
                values: new object[,]
                {
                    { 1, 1, 100.0, 90.0 },
                    { 1, 2, 50.0, 45.0 },
                    { 1, 3, 70.0, 60.0 },
                    { 2, 1, 110.0, 100.0 },
                    { 2, 2, 60.0, 55.0 },
                    { 2, 3, 80.0, 70.0 },
                    { 3, 1, 115.0, 105.0 },
                    { 3, 2, 65.0, 60.0 },
                    { 3, 3, 85.0, 75.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CompanyProducts",
                keyColumns: new[] { "CompanyId", "ProductId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "CompanyProducts",
                keyColumns: new[] { "CompanyId", "ProductId" },
                keyValues: new object[] { 1, 2 });

            migrationBuilder.DeleteData(
                table: "CompanyProducts",
                keyColumns: new[] { "CompanyId", "ProductId" },
                keyValues: new object[] { 1, 3 });

            migrationBuilder.DeleteData(
                table: "CompanyProducts",
                keyColumns: new[] { "CompanyId", "ProductId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "CompanyProducts",
                keyColumns: new[] { "CompanyId", "ProductId" },
                keyValues: new object[] { 2, 2 });

            migrationBuilder.DeleteData(
                table: "CompanyProducts",
                keyColumns: new[] { "CompanyId", "ProductId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "CompanyProducts",
                keyColumns: new[] { "CompanyId", "ProductId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "CompanyProducts",
                keyColumns: new[] { "CompanyId", "ProductId" },
                keyValues: new object[] { 3, 2 });

            migrationBuilder.DeleteData(
                table: "CompanyProducts",
                keyColumns: new[] { "CompanyId", "ProductId" },
                keyValues: new object[] { 3, 3 });
        }
    }
}
