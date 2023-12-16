﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionIncidencias.Migrations
{
    /// <inheritdoc />
    public partial class CrearCampoActivoIncidencias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activo",
                table: "Incidencias",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activo",
                table: "Incidencias");
        }
    }
}
