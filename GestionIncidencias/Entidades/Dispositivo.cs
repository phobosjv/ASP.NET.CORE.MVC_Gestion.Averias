namespace GestionIncidencias.Entidades
{
    public class Dispositivo // Clase que difinirá el modelo de registro almacenado en la tabla Dispositivos en BBDD
    {
        public int Id { get; set; } // Id del registro en la tabla Dispositivos

        [Required]
        [StringLength(50)]
        [Display(Name = "Número Serie")]
        public string NumeroSerie { get; set; } // Número de serie del dispositivo en particular. Si de algún modelo no se almacena el número de serie
                                                // crearemos un dispositivo genérico de ese modelo, con número de serie 0 y oficina 0, y éste será utilizado
                                                // para abrir incidencias en todos los centros.
        [Display(Name = "Modelo Dispositivo")]
        public int ModeloDispositivoId { get; set; } // Modelo al que pertenece el dispositivo.

        public ModeloDispositivo ModeloDispositivo { get; set; } // Para configurar la relación entre Dispositivo y 1 Modelo

        [Display(Name = "Oficina o Centro")]
        public int OficinaId { get; set; } // Oficina a la que pertenece el dispositivo. Si el número de Serie es 0, la oficina debe ser 0 - 0000 - Sin Centro

        public Oficina Oficina { get; set; } // Configura la relación entre Dispositivo y 1 Oficina

        [Required]
        [DefaultValue("true")]
        public bool Activo { get; set; } // Indica si el dispositivo está activo o no.

        public List<Incidencia> Incidencias { get; set; } // Configura la Relación entre este Dispositivo y varias Incidencias.
    }
}
