namespace GestionIncidencias.Models
{
    // Modelo para la lista de tipos de dispositivos.
    public class TiposDispositivosListadoViewModel
    {
        // Lista de tipos de dispositivos
        public List<TipoDispositivoViewModel> TiposDispositivos { get; set; }

        // Para pasar mensajes de acciones realizadas o errores.
        public string Mensaje { get; set; }
    }
}