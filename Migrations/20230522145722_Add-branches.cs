using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject.Migrations
{
    /// <inheritdoc />
    public partial class Addbranches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxNumberOfScreen",
                table: "Films",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinNumberOfScreen",
                table: "Films",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "CinemaRooms",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Branches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CommuneId = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false),
                    DistrictId = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    ProvinceId = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
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
                    table.PrimaryKey("PK_Branches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Branches_Communes_CommuneId",
                        column: x => x.CommuneId,
                        principalTable: "Communes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Branches_Districts_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Districts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Branches_Provinces_ProvinceId",
                        column: x => x.ProvinceId,
                        principalTable: "Provinces",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BranchId",
                table: "Orders",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_BranchId",
                table: "Employees",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_CinemaRooms_BranchId",
                table: "CinemaRooms",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_CommuneId",
                table: "Branches",
                column: "CommuneId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_DistrictId",
                table: "Branches",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_ProvinceId",
                table: "Branches",
                column: "ProvinceId");

            migrationBuilder.AddForeignKey(
                name: "FK_CinemaRooms_Branches_BranchId",
                table: "CinemaRooms",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Branches_BranchId",
                table: "Employees",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Branches_BranchId",
                table: "Orders",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CinemaRooms_Branches_BranchId",
                table: "CinemaRooms");

            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Branches_BranchId",
                table: "Employees");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Branches_BranchId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Branches");

            migrationBuilder.DropIndex(
                name: "IX_Orders_BranchId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Employees_BranchId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_CinemaRooms_BranchId",
                table: "CinemaRooms");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "MaxNumberOfScreen",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "MinNumberOfScreen",
                table: "Films");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "CinemaRooms");
        }
    }
}
