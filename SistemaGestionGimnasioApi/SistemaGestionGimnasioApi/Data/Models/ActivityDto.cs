using System.ComponentModel.DataAnnotations;

namespace SistemaGestionGimnasioApi.Data.Models
{
    public class ActivityDto
    {
        [Required]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "El campo debe contener solo letras.")]
        public string ActivityName { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z]+$", ErrorMessage = "El campo debe contener solo letras.")]
        public string ActivityDescription { get; set; }
    }
}
