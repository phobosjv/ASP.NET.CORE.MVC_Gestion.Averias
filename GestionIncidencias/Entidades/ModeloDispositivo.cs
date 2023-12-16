namespace GestionIncidencias.Entidades
{
    public class ModeloDispositivo  // Clase que definirá el modelo de registro almacenado en la tabla ModelosDispositivos en BBDD
    {
        public int Id { get; set; } // Id del registro en tabla ModelosDispositivos

        [Required]
        [StringLength(50)]
        public string Marca { get; set; } // Marca del dispositivo

        [Required]
        [StringLength(50)]
        public string Modelo { get; set; } // Modelo dentro de la marca

        [Display(Name = "Tipo Dispositivo")]
        public int TipoDispositivoId { get; set; } // Tipo de Dispositivo, se relaciona a través del Id

        public TipoDispositivo TipoDispositivo { get; set; } // Para configurar la relación entre este ModeloDispositivo y 1 TipoDispositivo

        [Display(Name = "Técnico")]
        public int TecnicoId { get; set; } // Técnico que se encarga en la actualidad de las averías de este modelo de dispositivo

        
        public Tecnico Tecnico { get; set; } // Para configurar la relación entre este ModeloDispositivo y 1 Tecnico.

        [Display(Name = "Descripción")]
        [StringLength(200)]
        public string Descripcion { get; set; } // Breve descripción acerca del modelo de dispositivo, como carácterísticas.

        [Required]
        [DefaultValue("true")]
        public bool Activo { get; set; } // Indica si el modelo está activo o no.

        public List<Dispositivo> Dispositivos { get; set;} // Configura la Relación entre este modelo y varios dispositivos.
    }
}
