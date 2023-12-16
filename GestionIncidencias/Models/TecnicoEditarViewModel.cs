namespace GestionIncidencias.Models
{
    public class TecnicoEditarViewModel
    {
        public int Id { get; set; } // Id del registro en la tabla Tecnicos

        [Required]
        [StringLength(200)]
        public string NombreEmpresa { get; set; } // Nombre de la Empresa de los técnicos

        [Required]
        [StringLength(50)]
        public string Telefono { get; set; } // Teléfono de contacto

        [Required]
        [StringLength(200)]
        [EmailAddress]
        public string CorreoPara { get; set; } // Correo Electrónico Incidencia Para

        [StringLength(200)]
        [EmailAddress]
        public string CorreoCopia { get; set; } // Correo Electrónico Incidencia Copia

        [Required]
        [StringLength(200)]
        public string Asunto { get; set; } // Correo Electrónico Incidencia Asunto

        [Required]
        public string Texto { get; set; } // Texto que se usará como plantilla para poner la incidencia

    }
}
