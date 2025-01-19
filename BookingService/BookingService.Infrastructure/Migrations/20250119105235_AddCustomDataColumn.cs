using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomDataColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2418ca95-a923-4e9e-970a-984a04dd9ac2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3f5af12b-f962-447a-b60a-d31fc90c8525");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4b3d8121-8760-4e7a-a201-f0684ee56bb1");

            migrationBuilder.AddColumn<string>(
                name: "CustomData",
                table: "Apartments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "18717da5-cf36-45a0-a578-4d108aa55737", null, "Admin", "ADMIN" },
                    { "44d622ea-36d3-4896-ab37-8a2c68bb71e5", null, "User", "USER" },
                    { "ce814dad-bde0-42fb-b6a4-ecfb572d8497", null, "Host", "HOST" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "18717da5-cf36-45a0-a578-4d108aa55737");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "44d622ea-36d3-4896-ab37-8a2c68bb71e5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ce814dad-bde0-42fb-b6a4-ecfb572d8497");

            migrationBuilder.DropColumn(
                name: "CustomData",
                table: "Apartments");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2418ca95-a923-4e9e-970a-984a04dd9ac2", null, "Host", "HOST" },
                    { "3f5af12b-f962-447a-b60a-d31fc90c8525", null, "User", "USER" },
                    { "4b3d8121-8760-4e7a-a201-f0684ee56bb1", null, "Admin", "ADMIN" }
                });
        }
    }
}
