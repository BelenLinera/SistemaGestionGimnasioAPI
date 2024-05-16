using System.ComponentModel.DataAnnotations;

namespace SistemaGestionGimnasioApi.Data.Models
{
    public class ActivityDto
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "El campo no debe contener símbolos ni caracteres especiales.")]
        public string ActivityName { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z0-9 ]+$", ErrorMessage = "El campo no debe contener símbolos ni caracteres especiales.")]
        public string ActivityDescription { get; set; }
    }
}
