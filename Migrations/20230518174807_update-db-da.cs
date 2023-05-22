using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject.Migrations
{
    /// <inheritdoc />
    public partial class updatedbda : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Users_Communes_CommunesId",
            //    table: "Users");

            //migrationBuilder.DropIndex(
            //    name: "IX_Users_CommunesId",
            //    table: "Users");

            //migrationBuilder.DropColumn(
            //    name: "CommunesId",
            //    table: "Users");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CommuneId",
                table: "Users",
                column: "CommuneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Communes_CommuneId",
                table: "Users",
                column: "CommuneId",
                principalTable: "Communes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Communes_CommuneId",
                table: "Users");

            //migrationBuilder.DropIndex(
            //    name: "IX_Users_CommuneId",
            //    table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "CommunesId",
                table: "Users",
                type: "nvarchar(5)",
                nullable: false,
                defaultValue: "");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Users_CommunesId",
            //    table: "Users",
            //    column: "CommunesId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Users_Communes_CommunesId",
            //    table: "Users",
            //    column: "CommunesId",
            //    principalTable: "Communes",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }
    }
}
