namespace GestionIncidencias.Entidades
{
    public class Oficina  // Clase que definirá el modelo de registro almacenado en la tabla Oficinas en BBDD
    {
        public int Id { get; set; } // Id del registro en la tabla Oficinas

        [Required(ErrorMessage = "Campo Obligatorio")]
        [StringLength(4)]
        [Display(Name = "Código Centro")]
        public string Codigo { get; set; } // Código de la Oficina ####

        [Required(ErrorMessage = "Campo Obligatorio")]
        [StringLength(100)]
        public string Nombre { get; set; } // Nombre de la Oficina

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "Dirección Postal")]
        public string Direccion { get; set; } // Direccion de la oficina

        [Required(ErrorMessage = "Campo Obligatorio")]
        [StringLength(50)]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } // Teléfono de contacto de la oficina.

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "Correo Electrónico")]
        [EmailAddress(ErrorMessage = "Dirección email no válida")]
        public string Email { get; set; } // Dirección de Correo Electrónico de la Oficina

        [Required(ErrorMessage = "Campo Obligatorio")]
        [DefaultValue("true")]
        public bool Activo { get; set; } // Indica si la oficina está activa o no.

        public string Nota { get; set; } // Anotaciones acerca del centro, como horarios especiales.

        public List<Dispositivo> Dispositivos { get; set; } // Configura la Relación entre esta Oficina y varios Dispositivos.

        public List<Incidencia> Incidencias { get; set;} // Configura la Relación entre esta Oficina y varias Incidencias.
    }
}
