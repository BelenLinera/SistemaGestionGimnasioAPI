using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SistemaGestionGimnasioApi.Data.Entities
{
    public class CancelledClassDate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int GymClassId { get; set; }
        public GymClass GymClass { get; set; }

        public DateTime CancelledDate { get; set; }
    }
}
