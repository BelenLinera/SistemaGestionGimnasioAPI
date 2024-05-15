using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Org.BouncyCastle.Asn1.X509;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaGestionGimnasioApi.Data.Entities
{
    public class TrainerActivity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdTrainerActivity { get; set; }
        [ForeignKey("TrainerEmail")]
        [Required]
        public string TrainerEmail { get; set; }
        public Trainer Trainer { get;set; }
        [ForeignKey("IdActivity")]
        public int IdActivity { get; set; }
        public Activity Activity { get; set; }





    }
}
