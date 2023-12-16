namespace GestionIncidencias.Models
{
    // Modelo para la pantalla Login del Usuario
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El usuario es obligatorio.")]
        [Display(Name = "Usuario")]
        [StringLength(maximumLength: 100)]
        public string Usuario { get; set; } // Se hace login con el Usuario

        [Required(ErrorMessage = "La contraseña es requerida.")]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Recordar Credenciales")]
        public bool Recuerdame { get; set; } // Check para indicar al sistema si se desea recordar el login para la próxima vez.
    }
}
