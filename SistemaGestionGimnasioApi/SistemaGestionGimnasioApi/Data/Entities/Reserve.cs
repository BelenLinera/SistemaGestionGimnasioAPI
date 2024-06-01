using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaGestionGimnasioApi.Data.Entities
{
    public class Reserve
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("IdGymClass")]
        public int IdGymClass { get; set; }
        public GymClass GymClass { get; set; }

        [ForeignKey("ClientEmail")]
        public string ClientEmail { get; set; }
        public Client Client { get; set; }

        public bool ClientAttended { get; set; }
        public DateTime DateClass { get; set; }
    }
}
