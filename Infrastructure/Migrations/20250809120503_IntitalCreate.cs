using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IntitalCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceUsers_OTPCodes_OTPCodeID",
                table: "AttendanceUsers");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceUsers_OTPCodeID",
                table: "AttendanceUsers");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "VoteSessions");

            migrationBuilder.AddColumn<int>(
                name: "VoteSessionStatus",
                table: "VoteSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "OTPCodeID",
                table: "AttendanceUsers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceUsers_OTPCodeID",
                table: "AttendanceUsers",
                column: "OTPCodeID",
                unique: true,
                filter: "[OTPCodeID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceUsers_OTPCodes_OTPCodeID",
                table: "AttendanceUsers",
                column: "OTPCodeID",
                principalTable: "OTPCodes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AttendanceUsers_OTPCodes_OTPCodeID",
                table: "AttendanceUsers");

            migrationBuilder.DropIndex(
                name: "IX_AttendanceUsers_OTPCodeID",
                table: "AttendanceUsers");

            migrationBuilder.DropColumn(
                name: "VoteSessionStatus",
                table: "VoteSessions");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "VoteSessions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "OTPCodeID",
                table: "AttendanceUsers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AttendanceUsers_OTPCodeID",
                table: "AttendanceUsers",
                column: "OTPCodeID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AttendanceUsers_OTPCodes_OTPCodeID",
                table: "AttendanceUsers",
                column: "OTPCodeID",
                principalTable: "OTPCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
