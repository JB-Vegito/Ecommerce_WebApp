using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ecommerce_web.Migrations
{
    /// <inheritdoc />
    public partial class ShippingV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Item_Quantity",
                table: "Shipping",
                newName: "FinalAmount");

            migrationBuilder.AddColumn<string>(
                name: "Shipping",
                table: "Shipping",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Shipping",
                table: "Shipping");

            migrationBuilder.RenameColumn(
                name: "FinalAmount",
                table: "Shipping",
                newName: "Item_Quantity");
        }
    }
}
