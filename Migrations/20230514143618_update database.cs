using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject.Migrations
{
    /// <inheritdoc />
    public partial class updatedatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Films_TypeFilm_TypeFilmId",
                table: "Films");

            migrationBuilder.DropIndex(
                name: "IX_Films_TypeFilmId",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "TypeFilmId",
                table: "Films");

            migrationBuilder.RenameColumn(
                name: "TralerLink",
                table: "Films",
                newName: "TrailerLink");

            migrationBuilder.AddColumn<string>(
                name: "Director",
                table: "Films",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Introduce",
                table: "Films",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TypeFilmDetail",
                columns: table => new
                {
                    FilmId = table.Column<int>(type: "int", nullable: false),
                    TypeFilmId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    CreatedIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    UpdatedBy = table.Column<int>(type: "int", nullable: true),
                    UpdatedIp = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DelFlag = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeFilmDetail", x => new { x.FilmId, x.TypeFilmId });
                    table.ForeignKey(
                        name: "FK_TypeFilmDetail_Films_FilmId",
                        column: x => x.FilmId,
                        principalTable: "Films",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TypeFilmDetail_TypeFilm_TypeFilmId",
                        column: x => x.TypeFilmId,
                        principalTable: "TypeFilm",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_TypeFilmDetail_TypeFilmId",
                table: "TypeFilmDetail",
                column: "TypeFilmId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TypeFilmDetail");

            migrationBuilder.DropColumn(
                name: "Director",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "Introduce",
                table: "Films");

            migrationBuilder.RenameColumn(
                name: "TrailerLink",
                table: "Films",
                newName: "TralerLink");

            migrationBuilder.AddColumn<int>(
                name: "TypeFilmId",
                table: "Films",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Films_TypeFilmId",
                table: "Films",
                column: "TypeFilmId");

            migrationBuilder.AddForeignKey(
                name: "FK_Films_TypeFilm_TypeFilmId",
                table: "Films",
                column: "TypeFilmId",
                principalTable: "TypeFilm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
