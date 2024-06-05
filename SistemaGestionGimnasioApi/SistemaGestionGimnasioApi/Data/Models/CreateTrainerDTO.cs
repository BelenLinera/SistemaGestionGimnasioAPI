using System.ComponentModel.DataAnnotations;

namespace SistemaGestionGimnasioApi.Data.Models
{
    public class CreateTrainerDTO
    {
        [Required(ErrorMessage = "Este campo no puede quedar vacio")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Este campo no puede quedar vacio")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Este campo no puede quedar vacio")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Este campo no puede quedar vacio")]
        [RegularExpression("^(?=.?[A-Z])(?=.?[a-z])(?=.*?[0-9]).{8,}$", ErrorMessage = "La contraseña debe contener al menos una letra minúscula,una mayuscula, un número y al menos 8 caracteres")]
        public string Password { get; set; }
        public List<int> Activities { get; set; } 
    }
}
