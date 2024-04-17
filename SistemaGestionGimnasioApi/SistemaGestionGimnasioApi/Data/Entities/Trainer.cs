using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaGestionGimnasioApi.Data.Entities
{
    public class Trainer : User
    {
        [ForeignKey("activityName")]
        [Required]
        public string ActivityName { get;set; }

        public Activity Activity { get; set; }
    }
}
