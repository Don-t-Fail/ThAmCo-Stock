using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Stock.Data.Migrations
{
    public partial class AddSupplierToOrderRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Supplier",
                table: "OrderRequests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Supplier",
                table: "OrderRequests");
        }
    }
}
