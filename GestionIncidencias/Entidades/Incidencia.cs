namespace GestionIncidencias.Entidades
{
    public class Incidencia // Clase que definirá el modelo de registro de la tabla Incidencias en la BBDD
    {
        public int Id { get; set; } // Id del registro en la tabla Incidencias

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "Fecha y Hora")]
        public DateTime FechaHora { get; set; } // Fecha y hora de la incidencia

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "Usuario Solicitante")]
        public string Usuario { get; set; } // Usuario que pone la avería

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "Oficina o Centro")]
        public int OficinaId { get; set; } // Id de la Oficina en la que se abre la avería

        public Oficina Oficina { get; set; } // Para configurar la relación entre esta Avería y 1 Oficina

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "Dispositivo")]
        public int DispositivoId { get; set; } // Id del Dispositivo del que se abre la avería

        public Dispositivo Dispositivo { get; set; } // Configura la relación entre estra Avería y 1 Dispositivo

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "Ténico Relacionado")]
        public int TecnicoId {  get; set; } // Id del Técnico que se encarga de la avería

        public Tecnico Tecnico { get; set; } // Configura la relación entre esta avería y 1 técnico.

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; } // Mensaje y detalles que se pasa al técnico para abrir la avería.

        [Required(ErrorMessage = "Campo Obligatorio")]
        [DefaultValue("true")]
        public bool Activo { get; set; } // Indica si la incidencia está comunicada o cancelada.
    }
}
