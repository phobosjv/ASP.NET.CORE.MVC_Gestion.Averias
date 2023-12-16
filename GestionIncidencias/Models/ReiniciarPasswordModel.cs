namespace GestionIncidencias.Models
{
    public class ReiniciarPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "La contraseña escrita no coincide en la confirmación.")]
        [Display(Name = "Repetir Contraseña")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "La dirección de correo electrónico no es válida.")]
        [Display(Name = "Correo electrónico")]
        [StringLength(maximumLength: 100)]
        public string Email { get; set; }

        public string Token { get; set; }
    }
}
