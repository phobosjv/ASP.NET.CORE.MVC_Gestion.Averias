namespace GestionIncidencias.Models
{
    // Modelo para la lista de modelos de dispositivos.
    public class IncidenciasListadoViewModel
    {
        // Lista de dispositivos
        public List<Incidencia> Incidencias { get; set; }

        // Para pasar mensajes de acciones realizadas o errores.
        public string Mensaje { get; set; }
    }
}