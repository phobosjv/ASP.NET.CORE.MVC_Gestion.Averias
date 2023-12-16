namespace GestionIncidencias.Models
{
    // Modelo para el listado de Oficinas.
    public class OficinasListadoViewModel
    {
        // Lista de Oficinas
        public List<OficinaViewModel> Oficinas { get; set; }

        // Para pasar mensajes de acciones realizadas o errores.
        public string Mensaje { get; set; }
    }
}