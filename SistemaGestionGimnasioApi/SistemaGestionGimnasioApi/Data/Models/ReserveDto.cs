using System.ComponentModel.DataAnnotations;

namespace SistemaGestionGimnasioApi.Data.Models
{
    public class ReserveDto
    {
        [Required]
        public int IdGymClass { get; set; }        

        [Required]
        //[RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "El día debe estar en formato 'dd' y ser un número válido entre 01 y 31.")]
        public DateTime DateClass { get; set; }

    }
}
