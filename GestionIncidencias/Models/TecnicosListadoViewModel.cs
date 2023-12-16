namespace GestionIncidencias.Models
{
    // Modelo para el listado de Técnicos.
    public class TecnicosListadoViewModel
    {
        // Lista de tecnicos
        public List<TecnicoViewModel> Tecnicos { get; set; }

        // Para pasar mensajes de acciones realizadas o errores.
        public string Mensaje { get; set; }
    }
}