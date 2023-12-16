namespace GestionIncidencias.Models
{

    // Modelo que se usará para mostrar en la vista del formulario de registro de usuario. 

    public class RegistroViewModel
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "La dirección de correo electrónico no es válida.")]
        [Display(Name = "Correo electrónico")]
        [StringLength(maximumLength: 100)]
        public string Email { get; set; } // Dirección de correo electrónico del usuario a registrar.

        [Required(ErrorMessage = "El usuario es obligatorio.")]
        [Display(Name = "Usuario")]
        [StringLength(maximumLength: 100)]
        public string Usuario { get; set; } // Usuario a registrar

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
