using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Store.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedCompanyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Companies",
                columns: new[] { "Id", "City", "Name", "PhoneNumber", "PostalCode", "StreetAddress" },
                values: new object[,]
                {
                    { 1, "Floriano", "Tagtune", "362-555-3461", "64800-000", "5 Erie Court" },
                    { 2, "Salamina", "Jabbercube", "725-831-6046", "477047", "592 Porter Way" },
                    { 3, "Sankoutang", "Skynoodle", "251-502-3387", "1237", "73428 Kipling Junction" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Companies",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
