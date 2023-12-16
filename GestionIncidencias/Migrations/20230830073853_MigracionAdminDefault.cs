using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionIncidencias.Migrations
{
    /// <inheritdoc />
    public partial class MigracionAdminDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Borrar los registros por defecto
            migrationBuilder.Sql("DELETE FROM AspNetUserRoles WHERE UserId='4f469d36-780f-4343-beec-3b460c3f8d17' AND RoleId='cb09f7fd-3053-41dd-a37b-a68be0b925c8';");
            migrationBuilder.Sql("DELETE FROM AspNetRoles WHERE Id='cb09f7fd-3053-41dd-a37b-a68be0b925c8';");
            migrationBuilder.Sql("DELETE FROM AspNetUsers WHERE Id='4f469d36-780f-4343-beec-3b460c3f8d17';");

            // Crear Rol Administrador
            migrationBuilder.Sql("INSERT into AspNetRoles (Id, [Name], [NormalizedName]) VALUES ('cb09f7fd-3053-41dd-a37b-a68be0b925c8', 'admin', 'ADMIN')");

            // Cread Usuario Administrador por Defecto aA123456*
            migrationBuilder.Sql(@"INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, NormalizedEmail, EmailConfirmed, PasswordHash, SecurityStamp, ConcurrencyStamp, 
                                            PhoneNumber, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnd, LockoutEnabled, AccessFailedCount)
                                VALUES('4f469d36-780f-4343-beec-3b460c3f8d17', 'admin', 'ADMIN', 'admin@default.com', 'ADMIN@DEFAULT.COM', 1, 
                                            'AQAAAAIAAYagAAAAEELuUtrbjkecDURmUtGhCT0sjViXtTOLwAoVnC4zi2QKzbALG2Vg9VFyqrMt/hY70Q==', 'F2Y3D3GCRWE5JSCNZAORFX6CF4ZI7B4A', 
                                            '360425a7-913b-41ec-9802-4851a3c47ec0', NULL, 0, 0, NULL, 1, 0);");

            // Crear Relación Administrador Usuario-Rol
            migrationBuilder.Sql("INSERT INTO AspNetUserRoles (UserId, RoleId) VALUES('4f469d36-780f-4343-beec-3b460c3f8d17', 'cb09f7fd-3053-41dd-a37b-a68be0b925c8');");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Borrar los registros por defecto
            migrationBuilder.Sql("DELETE FROM AspNetUserRoles WHERE UserId='4f469d36-780f-4343-beec-3b460c3f8d17' AND RoleId='cb09f7fd-3053-41dd-a37b-a68be0b925c8';");
            migrationBuilder.Sql("DELETE FROM AspNetRoles WHERE Id='cb09f7fd-3053-41dd-a37b-a68be0b925c8';");
            migrationBuilder.Sql("DELETE FROM AspNetUsers WHERE Id='4f469d36-780f-4343-beec-3b460c3f8d17';");
        }
    }
}
