using System.ComponentModel.DataAnnotations;

namespace SistemaGestionGimnasioApi.Data.Models
{
    public class EditTrainerDto
    {
        [Required(ErrorMessage = "Este campo no puede quedar vacio")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Este campo no puede quedar vacio")]
        public string LastName { get; set; }
        public List<int> Activities { get; set; }  
    }
}
