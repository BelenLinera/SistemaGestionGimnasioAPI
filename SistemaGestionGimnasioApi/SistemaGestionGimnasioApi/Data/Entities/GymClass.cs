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

        [ForeignKey("IdTrainerActivity")]
        public int IdTrainerActivity { get; set; }
        public TrainerActivity TrainerActivity { get; set; }

  
        public DateTime DateTimeClass { get; set; }
        public int Capacity { get; set; }


    }
}
