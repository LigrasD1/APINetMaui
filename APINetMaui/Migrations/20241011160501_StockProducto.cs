using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APINetMaui.Migrations
{
    /// <inheritdoc />
    public partial class StockProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "stock",
                table: "Productos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "stock",
                table: "Productos");
        }
    }
}
