using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadeDelete22 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HashedPassword = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OTPCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OTPCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SystemLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PerformedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VoteSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TopicTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedByAdminId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Admins_CreatedByAdminId",
                        column: x => x.CreatedByAdminId,
                        principalTable: "Admins",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VoteQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    VoteSessionId = table.Column<int>(type: "int", nullable: false),
                    VoteSessionId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoteQuestions_VoteSessions_VoteSessionId",
                        column: x => x.VoteSessionId,
                        principalTable: "VoteSessions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VoteQuestions_VoteSessions_VoteSessionId1",
                        column: x => x.VoteSessionId1,
                        principalTable: "VoteSessions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AttendanceUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    VoteSessionId = table.Column<int>(type: "int", nullable: false),
                    OTPCodeID = table.Column<int>(type: "int", nullable: false),
                    CreatedByAdminId = table.Column<int>(type: "int", nullable: false),
                    AdminId = table.Column<int>(type: "int", nullable: true),
                    UserId1 = table.Column<int>(type: "int", nullable: true),
                    VoteSessionId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendanceUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AttendanceUsers_Admins_AdminId",
                        column: x => x.AdminId,
                        principalTable: "Admins",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AttendanceUsers_Admins_CreatedByAdminId",
                        column: x => x.CreatedByAdminId,
                        principalTable: "Admins",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AttendanceUsers_OTPCodes_OTPCodeID",
                        column: x => x.OTPCodeID,
                        principalTable: "OTPCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AttendanceUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AttendanceUsers_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AttendanceUsers_VoteSessions_VoteSessionId",
                        column: x => x.VoteSessionId,
                        principalTable: "VoteSessions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AttendanceUsers_VoteSessions_VoteSessionId1",
                        column: x => x.VoteSessionId1,
                        principalTable: "VoteSessions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VoteQuestionOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VoteQuestionId = table.Column<int>(type: "int", nullable: false),
                    VoteQuestionId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteQuestionOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoteQuestionOptions_VoteQuestions_VoteQuestionId",
                        column: x => x.VoteQuestionId,
                        principalTable: "VoteQuestions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VoteQuestionOptions_VoteQuestions_VoteQuestionId1",
                        column: x => x.VoteQuestionId1,
                        principalTable: "VoteQuestions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VotedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    VoteQuestionOptionId = table.Column<int>(type: "int", nullable: false),
                    UserId1 = table.Column<int>(type: "int", nullable: true),
                    VoteQuestionOptionId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Votes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Votes_Users_UserId1",
                        column: x => x.UserId1,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Votes_VoteQuestionOptions_VoteQuestionOptionId",
                        column: x => x.VoteQuestionOptionId,
                        principalTable: "VoteQuestionOptions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Votes_VoteQuestionOptions_VoteQuestionOptionId1",
                        column: x => x.VoteQuestionOptionId1,
                        principalTable: "VoteQuestionOptions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceUsers_AdminId",
                table: "AttendanceUsers",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceUsers_CreatedByAdminId",
                table: "AttendanceUsers",
                column: "CreatedByAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceUsers_OTPCodeID",
                table: "AttendanceUsers",
                column: "OTPCodeID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceUsers_UserId",
                table: "AttendanceUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceUsers_UserId1",
                table: "AttendanceUsers",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceUsers_VoteSessionId",
                table: "AttendanceUsers",
                column: "VoteSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceUsers_VoteSessionId1",
                table: "AttendanceUsers",
                column: "VoteSessionId1");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CreatedByAdminId",
                table: "Users",
                column: "CreatedByAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_VoteQuestionOptions_VoteQuestionId",
                table: "VoteQuestionOptions",
                column: "VoteQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_VoteQuestionOptions_VoteQuestionId1",
                table: "VoteQuestionOptions",
                column: "VoteQuestionId1");

            migrationBuilder.CreateIndex(
                name: "IX_VoteQuestions_VoteSessionId",
                table: "VoteQuestions",
                column: "VoteSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_VoteQuestions_VoteSessionId1",
                table: "VoteQuestions",
                column: "VoteSessionId1");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_UserId",
                table: "Votes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_UserId1",
                table: "Votes",
                column: "UserId1");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_VoteQuestionOptionId",
                table: "Votes",
                column: "VoteQuestionOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_VoteQuestionOptionId1",
                table: "Votes",
                column: "VoteQuestionOptionId1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AttendanceUsers");

            migrationBuilder.DropTable(
                name: "SystemLogs");

            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.DropTable(
                name: "OTPCodes");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "VoteQuestionOptions");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "VoteQuestions");

            migrationBuilder.DropTable(
                name: "VoteSessions");
        }
    }
}
