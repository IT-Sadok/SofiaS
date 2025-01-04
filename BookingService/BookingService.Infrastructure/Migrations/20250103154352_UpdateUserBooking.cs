using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookingService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_HostId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_TenantId",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_HostId",
                table: "Bookings");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2988b275-4116-4048-a617-c87dd3a2bdac");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "47575409-9673-4d41-9906-d010f12e6976");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b88d87d9-b5d2-46c0-a472-7a70d57ebf21");

            migrationBuilder.DropColumn(
                name: "HostId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "TenantId",
                table: "Bookings",
                newName: "ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_TenantId",
                table: "Bookings",
                newName: "IX_Bookings_ClientId");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5ffeef37-7091-4fe9-97b2-aaf91c526b57", null, "Admin", "ADMIN" },
                    { "7e7de068-46fd-4edc-b394-f794be4cdb48", null, "User", "USER" },
                    { "fce66a34-7efb-4c35-9d3b-217c5d3e5f95", null, "Host", "HOST" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_ClientId",
                table: "Bookings",
                column: "ClientId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_ClientId",
                table: "Bookings");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5ffeef37-7091-4fe9-97b2-aaf91c526b57");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7e7de068-46fd-4edc-b394-f794be4cdb48");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fce66a34-7efb-4c35-9d3b-217c5d3e5f95");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Bookings",
                newName: "TenantId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_ClientId",
                table: "Bookings",
                newName: "IX_Bookings_TenantId");

            migrationBuilder.AddColumn<string>(
                name: "HostId",
                table: "Bookings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2988b275-4116-4048-a617-c87dd3a2bdac", null, "Host", "HOST" },
                    { "47575409-9673-4d41-9906-d010f12e6976", null, "Admin", "ADMIN" },
                    { "b88d87d9-b5d2-46c0-a472-7a70d57ebf21", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_HostId",
                table: "Bookings",
                column: "HostId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_HostId",
                table: "Bookings",
                column: "HostId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_TenantId",
                table: "Bookings",
                column: "TenantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
