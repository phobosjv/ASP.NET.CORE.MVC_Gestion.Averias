namespace GestionIncidencias.Models
{
    public class DetalleIncidenciaCalendario
    {
        public int Id { get; set; }
        public string Hora { get; set; }
        public string Usuario { get; set; }
        public string CodOficina { get; set; }
        public string TipoDispositivo { get; set; }
        public string ModeloDispositivo { get; set; }
        public string Tecnico { get; set; }
        public string Descripcion { get; set; }
    }
}
