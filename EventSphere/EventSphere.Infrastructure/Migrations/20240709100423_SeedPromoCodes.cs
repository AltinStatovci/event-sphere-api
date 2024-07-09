using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EventSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedPromoCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "PromoCodes",
                columns: new[] { "ID", "Code", "DiscountPercentage", "ExpiryDate", "IsValid" },
                values: new object[,]
                {
                    { 3, "SUMMERFUN", 10.0, new DateTime(2024, 8, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true },
                    { 4, "SPRINGSALE", 15.0, new DateTime(2024, 5, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true },
                    { 5, "WINTERWONDER", 25.0, new DateTime(2024, 12, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "ID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "ID",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "PromoCodes",
                keyColumn: "ID",
                keyValue: 5);
        }
    }
}
