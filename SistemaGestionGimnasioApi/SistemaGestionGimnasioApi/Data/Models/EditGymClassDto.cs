using System.ComponentModel.DataAnnotations;

namespace SistemaGestionGimnasioApi.Data.Models
{
    public class EditGymClassDto
    {
        [Required]
        [RegularExpression("^[0-9]+$", ErrorMessage = "El campo debe contener solo números.")]
        public int IdTrainerActivity { get; set; }

        [Required]
        public DateTime DateTimeClass { get; set; }

        [Required]
        [RegularExpression("^[0-9]+$", ErrorMessage = "El campo debe contener solo números.")]

        public int Capacity { get; set; }
    }
}
