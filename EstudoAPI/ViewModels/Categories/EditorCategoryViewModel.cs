using System.ComponentModel.DataAnnotations;

namespace EstudoAPI.ViewModels.Categories
{
    public class EditorCategoryViewModel
    {
        [Required(ErrorMessage = "Name é obrigatório")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Name deve conter entre 3 e 40 caracteres")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Slug é obrigatório")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Slug deve conter entre 3 e 40 caracteres")]
        public string Slug { get; set; }
    }
}
