using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APINetMaui.Migrations
{
    /// <inheritdoc />
    public partial class ActualizacionProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "image",
                table: "Productos",
                newName: "imageDirectory");

            migrationBuilder.AddColumn<string>(
                name: "ImagenLink",
                table: "Productos",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagenLink",
                table: "Productos");

            migrationBuilder.RenameColumn(
                name: "imageDirectory",
                table: "Productos",
                newName: "image");
        }
    }
}
