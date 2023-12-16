namespace GestionIncidencias.Models
{
    // Modelo para la lista de modelos de dispositivos.
    public class DispositivosListadoViewModel
    {
        // Lista de dispositivos
        public List<Dispositivo> Dispositivos { get; set; }

        // Para pasar mensajes de acciones realizadas o errores.
        public string Mensaje { get; set; }
    }
}