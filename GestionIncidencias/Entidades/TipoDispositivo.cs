namespace GestionIncidencias.Entidades
{
    public class TipoDispositivo // Clase que definirá el modelo de registro almacenado en la tabla TiposDispositivos en BBDD
    {
        public int Id { get; set; } // Id del registro en la tabla Tipos

        [Required(ErrorMessage = "Campo Obligatorio")]
        [Display(Name = "Tipo Dispositivo")]
        [StringLength(50)]
        public string Titulo { get; set; } // Nombre del Tipo de Dispositivo

        [StringLength(200)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; } // Descripción del Tipo de Dispositivo

        [Required(ErrorMessage = "Campo Obligatorio")]
        [DefaultValue("true")]
        public bool Activo { get; set; } // Especifica el modelo se usa en la actualidad

        public List<ModeloDispositivo> ModelosDispositivos { get; set; }  // Configura la Relación entre este técnico y varios modelos de dispositivos.
    }
}
