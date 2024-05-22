using SistemaGestionGimnasioApi.Data.Enum;
using System.ComponentModel.DataAnnotations;


namespace SistemaGestionGimnasioApi.Data.Models
{
    public class GymClassDto
    {
        public int IdTrainerActivity { get; set; }

        [Required]
        [RegularExpression("^([0-9]|[01]\\d|2[0-3]):([00]\\d)$", ErrorMessage = "La hora debe estar en formato 'hh:mm'")]

        public string TimeClass { get; set; }
        
        public DaysEnum  Days { get; set; }

        [Required]
        [RegularExpression("^[0-9]+$", ErrorMessage = "El campo debe contener solo números.")]
        [Range(1,20, ErrorMessage = "La capacidad debe ser mayor a 1 y menor a 20")]
        public int Capacity { get; set; }
    }
}
