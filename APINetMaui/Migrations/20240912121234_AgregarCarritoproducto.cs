using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APINetMaui.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCarritoproducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductoCarrito_Carritos_CarritoId",
                table: "ProductoCarrito");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductoCarrito_Productos_ProductoId",
                table: "ProductoCarrito");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductoCarrito",
                table: "ProductoCarrito");

            migrationBuilder.RenameTable(
                name: "ProductoCarrito",
                newName: "ProductosCarritos");

            migrationBuilder.RenameIndex(
                name: "IX_ProductoCarrito_ProductoId",
                table: "ProductosCarritos",
                newName: "IX_ProductosCarritos_ProductoId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductoCarrito_CarritoId",
                table: "ProductosCarritos",
                newName: "IX_ProductosCarritos_CarritoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductosCarritos",
                table: "ProductosCarritos",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosCarritos_Carritos_CarritoId",
                table: "ProductosCarritos",
                column: "CarritoId",
                principalTable: "Carritos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosCarritos_Productos_ProductoId",
                table: "ProductosCarritos",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductosCarritos_Carritos_CarritoId",
                table: "ProductosCarritos");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductosCarritos_Productos_ProductoId",
                table: "ProductosCarritos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductosCarritos",
                table: "ProductosCarritos");

            migrationBuilder.RenameTable(
                name: "ProductosCarritos",
                newName: "ProductoCarrito");

            migrationBuilder.RenameIndex(
                name: "IX_ProductosCarritos_ProductoId",
                table: "ProductoCarrito",
                newName: "IX_ProductoCarrito_ProductoId");

            migrationBuilder.RenameIndex(
                name: "IX_ProductosCarritos_CarritoId",
                table: "ProductoCarrito",
                newName: "IX_ProductoCarrito_CarritoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductoCarrito",
                table: "ProductoCarrito",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductoCarrito_Carritos_CarritoId",
                table: "ProductoCarrito",
                column: "CarritoId",
                principalTable: "Carritos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductoCarrito_Productos_ProductoId",
                table: "ProductoCarrito",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
