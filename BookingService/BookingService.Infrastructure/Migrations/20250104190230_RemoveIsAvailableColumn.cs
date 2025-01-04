using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveIsAvailableColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0d54a2e7-12a5-4f6c-b67c-9fe0db487197");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "69dce702-b844-4c60-a1d0-d915bf971300");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "dd126777-040a-4901-b146-c78565c4c5a8");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Apartments");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3ac52668-7612-4dab-8903-1a863ca1c2d2", null, "User", "USER" },
                    { "3d88752b-0276-4c1c-89aa-428af122fb4f", null, "Host", "HOST" },
                    { "6f4da681-bcd5-47e5-9abf-125351ea0cd2", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3ac52668-7612-4dab-8903-1a863ca1c2d2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3d88752b-0276-4c1c-89aa-428af122fb4f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6f4da681-bcd5-47e5-9abf-125351ea0cd2");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Apartments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0d54a2e7-12a5-4f6c-b67c-9fe0db487197", null, "Admin", "ADMIN" },
                    { "69dce702-b844-4c60-a1d0-d915bf971300", null, "Host", "HOST" },
                    { "dd126777-040a-4901-b146-c78565c4c5a8", null, "User", "USER" }
                });
        }
    }
}
