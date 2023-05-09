using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject.Migrations
{
    /// <inheritdoc />
    public partial class Add_Table_User : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    LastLogin = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LastLogout = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DelFlag = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    JwtToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ValidateToken = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LatOfMap = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LongOfMap = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Browser = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PlayerId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Serial = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LoginFrom = table.Column<int>(type: "int", nullable: false),
                    Timeout = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DelFlag = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_UserId",
                table: "Employees",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_UserId",
                table: "UserTokens",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Users_UserId",
                table: "Employees",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Users_UserId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Employees_UserId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Employees");
        }
    }
}
