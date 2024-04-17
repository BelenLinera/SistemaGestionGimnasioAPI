using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaGestionGimnasioApi.Data.Entities
{
    public class Activity
    {
        [Key]
        [Required]
        public string ActivityName { get; set; }

        public string ActivityDescription { get; set; }

        
    }
}
