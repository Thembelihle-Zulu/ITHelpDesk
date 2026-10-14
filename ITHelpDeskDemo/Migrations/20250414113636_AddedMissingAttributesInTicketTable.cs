using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITHelpDeskDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddedMissingAttributesInTicketTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_agents_AgentId",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "FK_comments_tickets_TicketId",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "FK_tickets_agents_AgentId",
                table: "tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_tickets_users_UserId",
                table: "tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_tickets",
                table: "tickets");

            migrationBuilder.DropPrimaryKey(
                name: "PK_comments",
                table: "comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_agents",
                table: "agents");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "tickets",
                newName: "Ticket");

            migrationBuilder.RenameTable(
                name: "comments",
                newName: "Comment");

            migrationBuilder.RenameTable(
                name: "agents",
                newName: "Agent");

            migrationBuilder.RenameIndex(
                name: "IX_tickets_UserId",
                table: "Ticket",
                newName: "IX_Ticket_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_tickets_AgentId",
                table: "Ticket",
                newName: "IX_Ticket_AgentId");

            migrationBuilder.RenameIndex(
                name: "IX_comments_TicketId",
                table: "Comment",
                newName: "IX_Comment_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_comments_AgentId",
                table: "Comment",
                newName: "IX_Comment_AgentId");

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "Ticket",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Ticket",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ticket",
                table: "Ticket",
                column: "TicketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comment",
                table: "Comment",
                column: "CommentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Agent",
                table: "Agent",
                column: "AgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Agent_AgentId",
                table: "Comment",
                column: "AgentId",
                principalTable: "Agent",
                principalColumn: "AgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Ticket_TicketId",
                table: "Comment",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Agent_AgentId",
                table: "Ticket",
                column: "AgentId",
                principalTable: "Agent",
                principalColumn: "AgentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_User_UserId",
                table: "Ticket",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Agent_AgentId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Ticket_TicketId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Agent_AgentId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_User_UserId",
                table: "Ticket");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ticket",
                table: "Ticket");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comment",
                table: "Comment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Agent",
                table: "Agent");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "Ticket");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Ticket");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "Ticket",
                newName: "tickets");

            migrationBuilder.RenameTable(
                name: "Comment",
                newName: "comments");

            migrationBuilder.RenameTable(
                name: "Agent",
                newName: "agents");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_UserId",
                table: "tickets",
                newName: "IX_tickets_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Ticket_AgentId",
                table: "tickets",
                newName: "IX_tickets_AgentId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_TicketId",
                table: "comments",
                newName: "IX_comments_TicketId");

            migrationBuilder.RenameIndex(
                name: "IX_Comment_AgentId",
                table: "comments",
                newName: "IX_comments_AgentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tickets",
                table: "tickets",
                column: "TicketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_comments",
                table: "comments",
                column: "CommentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_agents",
                table: "agents",
                column: "AgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_agents_AgentId",
                table: "comments",
                column: "AgentId",
                principalTable: "agents",
                principalColumn: "AgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_tickets_TicketId",
                table: "comments",
                column: "TicketId",
                principalTable: "tickets",
                principalColumn: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_agents_AgentId",
                table: "tickets",
                column: "AgentId",
                principalTable: "agents",
                principalColumn: "AgentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_tickets_users_UserId",
                table: "tickets",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
