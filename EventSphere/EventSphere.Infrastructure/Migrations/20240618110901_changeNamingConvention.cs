using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventSphere.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeNamingConvention : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_EventCategory_CategoryID",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_User_OrganizerID",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Ticket_TicketID",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_User_UserID",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Event_EventID",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_RoleID",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "RoleID",
                table: "User",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "User",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_User_RoleID",
                table: "User",
                newName: "IX_User_RoleId");

            migrationBuilder.RenameColumn(
                name: "EventID",
                table: "Ticket",
                newName: "EventId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Ticket",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_EventID",
                table: "Ticket",
                newName: "IX_Ticket_EventId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Role",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "userlastName",
                table: "Report",
                newName: "UserLastName");

            migrationBuilder.RenameColumn(
                name: "userName",
                table: "Report",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "userId",
                table: "Report",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "userEmail",
                table: "Report",
                newName: "UserEmail");

            migrationBuilder.RenameColumn(
                name: "reportName",
                table: "Report",
                newName: "ReportName");

            migrationBuilder.RenameColumn(
                name: "reportDesc",
                table: "Report",
                newName: "ReportDesc");

            migrationBuilder.RenameColumn(
                name: "reportAnsw",
                table: "Report",
                newName: "ReportAnsw");

            migrationBuilder.RenameColumn(
                name: "reportId",
                table: "Report",
                newName: "ReportId");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Payment",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "TicketID",
                table: "Payment",
                newName: "TicketId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Payment",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_UserID",
                table: "Payment",
                newName: "IX_Payment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_TicketID",
                table: "Payment",
                newName: "IX_Payment_TicketId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "EventCategory",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "OrganizerID",
                table: "Event",
                newName: "OrganizerId");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                table: "Event",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Event",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Event_OrganizerID",
                table: "Event",
                newName: "IX_Event_OrganizerId");

            migrationBuilder.RenameIndex(
                name: "IX_Event_CategoryID",
                table: "Event",
                newName: "IX_Event_CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_EventCategory_CategoryId",
                table: "Event",
                column: "CategoryId",
                principalTable: "EventCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_User_OrganizerId",
                table: "Event",
                column: "OrganizerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Ticket_TicketId",
                table: "Payment",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_User_UserId",
                table: "Payment",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Event_EventId",
                table: "Ticket",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_RoleId",
                table: "User",
                column: "RoleId",
                principalTable: "Role",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Event_EventCategory_CategoryId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Event_User_OrganizerId",
                table: "Event");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Ticket_TicketId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_User_UserId",
                table: "Payment");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Event_EventId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_RoleId",
                table: "User");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "User",
                newName: "RoleID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "User",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_User_RoleId",
                table: "User",
                newName: "IX_User_RoleID");

            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "Ticket",
                newName: "EventID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Ticket",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_EventId",
                table: "Ticket",
                newName: "IX_Ticket_EventID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Role",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Report",
                newName: "userName");

            migrationBuilder.RenameColumn(
                name: "UserLastName",
                table: "Report",
                newName: "userlastName");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Report",
                newName: "userId");

            migrationBuilder.RenameColumn(
                name: "UserEmail",
                table: "Report",
                newName: "userEmail");

            migrationBuilder.RenameColumn(
                name: "ReportName",
                table: "Report",
                newName: "reportName");

            migrationBuilder.RenameColumn(
                name: "ReportDesc",
                table: "Report",
                newName: "reportDesc");

            migrationBuilder.RenameColumn(
                name: "ReportAnsw",
                table: "Report",
                newName: "reportAnsw");

            migrationBuilder.RenameColumn(
                name: "ReportId",
                table: "Report",
                newName: "reportId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Payment",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "TicketId",
                table: "Payment",
                newName: "TicketID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Payment",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_UserId",
                table: "Payment",
                newName: "IX_Payment_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_TicketId",
                table: "Payment",
                newName: "IX_Payment_TicketID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "EventCategory",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "OrganizerId",
                table: "Event",
                newName: "OrganizerID");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Event",
                newName: "CategoryID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Event",
                newName: "ID");

            migrationBuilder.RenameIndex(
                name: "IX_Event_OrganizerId",
                table: "Event",
                newName: "IX_Event_OrganizerID");

            migrationBuilder.RenameIndex(
                name: "IX_Event_CategoryId",
                table: "Event",
                newName: "IX_Event_CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Event_EventCategory_CategoryID",
                table: "Event",
                column: "CategoryID",
                principalTable: "EventCategory",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Event_User_OrganizerID",
                table: "Event",
                column: "OrganizerID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Ticket_TicketID",
                table: "Payment",
                column: "TicketID",
                principalTable: "Ticket",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_User_UserID",
                table: "Payment",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Event_EventID",
                table: "Ticket",
                column: "EventID",
                principalTable: "Event",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_RoleID",
                table: "User",
                column: "RoleID",
                principalTable: "Role",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
