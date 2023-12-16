namespace GestionIncidencias.Models
{
    // Modelo para el listado de usuarios.
    public class UsuariosListadoViewModel
    {
        // Lista de usuarios
        public List<UsuarioViewModel> Usuarios { get; set; }

        // Para pasar mensajes de acciones realizadas o errores.
        public string Mensaje { get; set; } 
    }
}
