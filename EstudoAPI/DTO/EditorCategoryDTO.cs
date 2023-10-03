using System.ComponentModel.DataAnnotations;

namespace EstudoAPI.DTO
{
    public class EditorCategoryDTO
    {
        [Required(ErrorMessage = "Name é obrigatório")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Este campo deve conter entre 3 e 40 caracteres")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Slug é obrigatório")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "Este campo deve conter entre 3 e 40 caracteres")]
        public string Slug { get; set; }
    }
}
