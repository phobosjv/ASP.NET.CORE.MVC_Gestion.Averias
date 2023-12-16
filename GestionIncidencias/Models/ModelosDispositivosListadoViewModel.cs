namespace GestionIncidencias.Models
{
    // Modelo para la lista de modelos de dispositivos.
    public class ModelosDispositivosListadoViewModel
    {
        // Lista de tipos de dispositivos
        public List<ModeloDispositivo> ModelosDispositivos { get; set; }

        // Para pasar mensajes de acciones realizadas o errores.
        public string Mensaje { get; set; }
    }
}