namespace GestionIncidencias.Models
{
    public class IncidenciaExportar
    {
        public int Id { get; set; }
        public DateTime FechaHora { get; set; }
        public string Usuario { get; set; }
        public string CodOficina { get; set; }
        public string NombreOficina { get; set; }
        public string TipoDispositivo { get; set; }
        public string ModeloDispositivo { get; set; }
        public string NumSerie { get; set; }
        public string Tecnico { get; set; }
        public string Mensaje { get; set; }
    }
}
