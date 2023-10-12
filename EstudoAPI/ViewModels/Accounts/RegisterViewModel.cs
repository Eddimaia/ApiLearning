using System.ComponentModel.DataAnnotations;

namespace EstudoAPI.ViewModels.Accounts
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "O e-mail é inválido")]
        public string Email { get; set; }
    }
}
