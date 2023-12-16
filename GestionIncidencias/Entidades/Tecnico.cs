namespace GestionIncidencias.Entidades
{
    public class Tecnico  // Clase que definirá el modelo de registro almacenado en la tabla Tecnicos en BBDD
    {
        public int Id { get; set; } // Id del registro en la tabla Tecnicos

        [Required(ErrorMessage ="Campo Obligatorio")]
        [StringLength(200, ErrorMessage = "No debe exceder los {1} caracteres")]
        [Display(Name = "Nombre Empresa")]
        public string NombreEmpresa { get; set; } // Nombre de la Empresa de los técnicos

        [Required(ErrorMessage = "Campo Obligatorio")]
        [StringLength(50, ErrorMessage = "No debe exceder los {1} caracteres")]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; } // Teléfono de contacto

        [Required(ErrorMessage = "Campo Obligatorio")]
        [StringLength(200, ErrorMessage = "No debe exceder los {1} caracteres")]
        [EmailAddress(ErrorMessage = "Dirección email no válida")]
        [Display(Name = "Correo Electrónico")]
        public string CorreoPara { get; set; } // Correo Electrónico Incidencia Para

        [RegularExpression(@"(([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)(\s*,\s*|\s*$))*"
                            , ErrorMessage = "Debe contener un correo, o lista de correos separados por ','")]
        [StringLength(200, ErrorMessage = "No debe exceder los {1} caracteres")]
        [Display(Name = "Correo Copia")]
        public string CorreoCopia { get; set; } // Correo Electrónico Incidencia Copia

        [Required(ErrorMessage = "Campo Obligatorio")]
        [StringLength(200, ErrorMessage = "No debe exceder los {1} caracteres")]
        [Display(Name = "Asunto Avisos")]
        public string Asunto { get; set; } // Correo Electrónico Incidencia Asunto

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "Plantilla Avisos")]
        public string Texto { get; set; } // Texto que se usará como plantilla para poner la incidencia

        [Required]
        [DefaultValue("true")]
        public bool Activo { get; set; } // Especifica si el técnico da servicio a la empresa en la actualidad

        public List<ModeloDispositivo> ModelosDispositivos { get; set; } // Configura la Relación entre este técnico y varios modelos de dispositivos.
    }
}
