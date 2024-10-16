using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APINetMaui.Migrations
{
    /// <inheritdoc />
    public partial class AgregarRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Rol_IdRol",
                table: "Usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rol",
                table: "Rol");

            migrationBuilder.RenameTable(
                name: "Rol",
                newName: "Rols");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rols",
                table: "Rols",
                column: "IdRol");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Rols_IdRol",
                table: "Usuarios",
                column: "IdRol",
                principalTable: "Rols",
                principalColumn: "IdRol");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuarios_Rols_IdRol",
                table: "Usuarios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rols",
                table: "Rols");

            migrationBuilder.RenameTable(
                name: "Rols",
                newName: "Rol");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rol",
                table: "Rol",
                column: "IdRol");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuarios_Rol_IdRol",
                table: "Usuarios",
                column: "IdRol",
                principalTable: "Rol",
                principalColumn: "IdRol");
        }
    }
}
