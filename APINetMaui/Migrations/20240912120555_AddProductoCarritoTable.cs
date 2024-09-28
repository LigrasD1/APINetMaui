using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APINetMaui.Migrations
{
    /// <inheritdoc />
    public partial class AddProductoCarritoTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Productos_Carritos_CarritoId",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_Productos_CarritoId",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "CarritoId",
                table: "Productos");

            migrationBuilder.CreateTable(
                name: "ProductoCarrito",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    CarritoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductoCarrito", x => x.id);
                    table.ForeignKey(
                        name: "FK_ProductoCarrito_Carritos_CarritoId",
                        column: x => x.CarritoId,
                        principalTable: "Carritos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductoCarrito_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductoCarrito_CarritoId",
                table: "ProductoCarrito",
                column: "CarritoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductoCarrito_ProductoId",
                table: "ProductoCarrito",
                column: "ProductoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductoCarrito");

            migrationBuilder.AddColumn<int>(
                name: "CarritoId",
                table: "Productos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CarritoId",
                table: "Productos",
                column: "CarritoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Productos_Carritos_CarritoId",
                table: "Productos",
                column: "CarritoId",
                principalTable: "Carritos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
