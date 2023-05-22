﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject.Migrations
{
    /// <inheritdoc />
    public partial class Addnameroom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "CinemaRooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "CinemaRooms");
        }
    }
}
