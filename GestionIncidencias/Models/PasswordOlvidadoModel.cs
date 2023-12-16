namespace GestionIncidencias.Models
{
    public class PasswordOlvidadoModel
    {
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "La dirección de correo electrónico no es válida.")]
        [Display(Name = "Correo electrónico")]
        [StringLength(maximumLength: 100)]
        public string Email { get; set; }
    }
}
