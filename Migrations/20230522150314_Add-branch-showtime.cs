using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject.Migrations
{
    /// <inheritdoc />
    public partial class Addbranchshowtime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "ShowTime",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShowTime_BranchId",
                table: "ShowTime",
                column: "BranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShowTime_Branches_BranchId",
                table: "ShowTime",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShowTime_Branches_BranchId",
                table: "ShowTime");

            migrationBuilder.DropIndex(
                name: "IX_ShowTime_BranchId",
                table: "ShowTime");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "ShowTime");
        }
    }
}
