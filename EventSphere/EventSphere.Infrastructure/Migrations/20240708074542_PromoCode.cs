using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EventSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PromoCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PromoCodes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DiscountPercentage = table.Column<double>(type: "float", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsValid = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoCodes", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "PromoCodes",
                columns: new[] { "ID", "Code", "DiscountPercentage", "ExpiryDate", "IsValid" },
                values: new object[,]
                {
                    { 1, "B3LI3V3R", 20.0, new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true },
                    { 2, "FALLSALE", 15.0, new DateTime(2024, 10, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), true }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromoCodes");
        }
    }
}
