using System.ComponentModel.DataAnnotations;

namespace SistemaGestionGimnasioApi.Data.Models
{
    public class ReserveDto
    {
        [Required]
        [RegularExpression("^[0-9]+$", ErrorMessage = "El campo debe contener solo números.")]

        public int IdGymClass { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Por favor, ingrese una dirección de correo electrónico válida.")]
        public string ClientEmail { get; set; }

        public bool ClientAttended { get; set; }

        [Required]
        //[RegularExpression(@"^(0[1-9]|[12][0-9]|3[01])$", ErrorMessage = "El día debe estar en formato 'dd' y ser un número válido entre 01 y 31.")]
        public DateTime DateClass { get; set; }

    }
}
