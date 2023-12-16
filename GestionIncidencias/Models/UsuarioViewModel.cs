namespace GestionIncidencias.Models
{
    public class UsuarioViewModel
    {
        public string NombreUsuario { get; set; }

        public string Email { get; set; }

        public bool Activado { get; set; }

        public string Permisos { get; set; }
    }
}
