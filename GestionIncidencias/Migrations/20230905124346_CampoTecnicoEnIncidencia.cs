using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionIncidencias.Migrations
{
    /// <inheritdoc />
    public partial class CampoTecnicoEnIncidencia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidencias_AspNetUsers_UsuarioId",
                table: "Incidencias");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "Incidencias",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TecnicoId",
                table: "Incidencias",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Incidencias_TecnicoId",
                table: "Incidencias",
                column: "TecnicoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidencias_AspNetUsers_UsuarioId",
                table: "Incidencias",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Incidencias_Tecnicos_TecnicoId",
                table: "Incidencias",
                column: "TecnicoId",
                principalTable: "Tecnicos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Incidencias_AspNetUsers_UsuarioId",
                table: "Incidencias");

            migrationBuilder.DropForeignKey(
                name: "FK_Incidencias_Tecnicos_TecnicoId",
                table: "Incidencias");

            migrationBuilder.DropIndex(
                name: "IX_Incidencias_TecnicoId",
                table: "Incidencias");

            migrationBuilder.DropColumn(
                name: "TecnicoId",
                table: "Incidencias");

            migrationBuilder.AlterColumn<string>(
                name: "UsuarioId",
                table: "Incidencias",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AddForeignKey(
                name: "FK_Incidencias_AspNetUsers_UsuarioId",
                table: "Incidencias",
                column: "UsuarioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
