using System.ComponentModel.DataAnnotations;

namespace SistemaGestionGimnasioApi.Data.Models
{
    public class ActivityDto
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9 áéíóúÁÉÍÓÚüÜñÑ]+$", ErrorMessage = "El campo no debe contener símbolos ni caracteres especiales.")]
        public string ActivityName { get; set; }

        [Required]
        [StringLength(120, MinimumLength = 5, ErrorMessage = "La descripción de la actividad debe tener entre 5 y 120 caracteres.")]
        public string ActivityDescription { get; set; }
    }
}
