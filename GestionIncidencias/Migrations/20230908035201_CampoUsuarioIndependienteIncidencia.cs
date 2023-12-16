using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionIncidencias.Migrations
{
    /// <inheritdoc />
    public partial class CampoUsuarioIndependienteIncidencia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidencias_AspNetUsers_UsuarioId",
                table: "Incidencias");

            migrationBuilder.DropIndex(
                name: "IX_Incidencias_UsuarioId",
                table: "Incidencias");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Incidencias",
                newName: "Usuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Usuario",
                table: "Incidencias",
                newName: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_UsuarioId",
                table: "Incidencias",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidencias_AspNetUsers_UsuarioId",
                table: "Incidencias",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
