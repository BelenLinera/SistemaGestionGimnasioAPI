using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaGestionGimnasioApi.Data.Entities
{
    public class GymClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdGymClass { get; set; }
        [ForeignKey("IdActivity")]
        public int IdActivity { get; set; }
        [Required]
        public Activity Activity { get; set; }

        [ForeignKey("TrainerEmail")]
        public string TrainerEmail { get; set; }
        public Trainer Trainer { get; set; }
        public DateTime DateTimeClass { get; set; }
        public int Capacity { get; set; }


    }
}
