using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITHelpDeskDemo.Migrations
{
    /// <inheritdoc />
    public partial class MergedAgentAndUsertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Agent_AgentId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Agent_AgentId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_User_UserId",
                table: "Ticket");

            migrationBuilder.DropTable(
                name: "Agent");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.UserId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AppUser_AgentId",
                table: "Comment",
                column: "AgentId",
                principalTable: "AppUser",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_AppUser_AgentId",
                table: "Ticket",
                column: "AgentId",
                principalTable: "AppUser",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_AppUser_UserId",
                table: "Ticket",
                column: "UserId",
                principalTable: "AppUser",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AppUser_AgentId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_AppUser_AgentId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_AppUser_UserId",
                table: "Ticket");

            migrationBuilder.DropTable(
                name: "AppUser");

            migrationBuilder.CreateTable(
                name: "Agent",
                columns: table => new
                {
                    AgentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agent", x => x.AgentId);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Department = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Agent_AgentId",
                table: "Comment",
                column: "AgentId",
                principalTable: "Agent",
                principalColumn: "AgentId");

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
    }
}
