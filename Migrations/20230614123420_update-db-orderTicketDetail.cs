using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CapstoneProject.Migrations
{
    /// <inheritdoc />
    public partial class updatedborderTicketDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountPrice",
                table: "OrderTicketDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentPrice",
                table: "OrderTicketDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "OrderTicketDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SalePrice",
                table: "OrderTicketDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DiscountPrice",
                table: "OrderFoodDetail",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountPrice",
                table: "OrderTicketDetail");

            migrationBuilder.DropColumn(
                name: "PaymentPrice",
                table: "OrderTicketDetail");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "OrderTicketDetail");

            migrationBuilder.DropColumn(
                name: "SalePrice",
                table: "OrderTicketDetail");

            migrationBuilder.DropColumn(
                name: "DiscountPrice",
                table: "OrderFoodDetail");
        }
    }
}
