using System.ComponentModel.DataAnnotations;

namespace SistemaGestionGimnasioApi.Data.Models
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "El token es obligatorio")]
        public string tokenRecover { get; set; }

        [Required(ErrorMessage = "La nueva contraseña es obligatoria")]
        [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$", ErrorMessage = "La contraseña debe contener al menos una letra minúscula, una mayúscula, un número y al menos 8 caracteres")]
        public string newPassword { get; set; }


    }
}
