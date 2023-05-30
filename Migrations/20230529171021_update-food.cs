using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject.Migrations
{
    /// <inheritdoc />
    public partial class updatefood : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Foods",
                newName: "NameOption1");

            migrationBuilder.AddColumn<string>(
                name: "NameOption2",
                table: "Foods",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameOption2",
                table: "Foods");

            migrationBuilder.RenameColumn(
                name: "NameOption1",
                table: "Foods",
                newName: "Name");
        }
    }
}
