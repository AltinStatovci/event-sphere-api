using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EventSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventCategory",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCategory", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Location",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Location", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    reportId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    userName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userlastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    userEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reportName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reportDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reportAnsw = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.reportId);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<byte[]>(type: "varbinary(64)", nullable: false),
                    Salt = table.Column<byte[]>(type: "varbinary(64)", nullable: false),
                    RoleID = table.Column<int>(type: "int", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Role",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrganizerID = table.Column<int>(type: "int", nullable: false),
                    OrganizerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhotoData = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxAttendance = table.Column<int>(type: "int", nullable: false),
                    AvailableTickets = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Event_EventCategory_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "EventCategory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Event_Location_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Location",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Event_User_OrganizerID",
                        column: x => x.OrganizerID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ticket",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventID = table.Column<int>(type: "int", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TicketType = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    BookingReference = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ticket", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Ticket_Event_EventID",
                        column: x => x.EventID,
                        principalTable: "Event",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TicketID = table.Column<int>(type: "int", nullable: false),
                    TicketName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PaymentStatus = table.Column<bool>(type: "bit", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Payment_Ticket_TicketID",
                        column: x => x.TicketID,
                        principalTable: "Ticket",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payment_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "EventCategory",
                columns: new[] { "ID", "CategoryName" },
                values: new object[,]
                {
                    { 1, "Concerts" },
                    { 2, "Sports" },
                    { 3, "Outside Activities" }
                });

            migrationBuilder.InsertData(
                table: "Location",
                columns: new[] { "Id", "City", "Country" },
                values: new object[,]
                {
                    { 1, "Prishtina", "Kosovo" },
                    { 2, "Fushe Kosove", "Kosovo" },
                    { 3, "Prizren", "Kosovo" },
                    { 4, "Gjilan", "Kosovo" },
                    { 5, "Ferizaj", "Kosovo" },
                    { 6, "Peje", "Kosovo" },
                    { 7, "Mitrovice", "Kosovo" }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "ID", "RoleName" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "Organizer" },
                    { 3, "User" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Event_CategoryID",
                table: "Event",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_Event_LocationId",
                table: "Event",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_OrganizerID",
                table: "Event",
                column: "OrganizerID");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_TicketID",
                table: "Payment",
                column: "TicketID");

            migrationBuilder.CreateIndex(
                name: "IX_Payment_UserID",
                table: "Payment",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Ticket_EventID",
                table: "Ticket",
                column: "EventID");

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleID",
                table: "User",
                column: "RoleID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropTable(
                name: "Ticket");

            migrationBuilder.DropTable(
                name: "Event");

            migrationBuilder.DropTable(
                name: "EventCategory");

            migrationBuilder.DropTable(
                name: "Location");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
