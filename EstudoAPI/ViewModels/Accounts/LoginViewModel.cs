using System.ComponentModel.DataAnnotations;

namespace EstudoAPI.ViewModels.Accounts
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "E-mail inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Senha é obrigatória")]
        public string Password { get; set; }
    }
}
