using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaGestionGimnasioApi.Data.Entities
{
    public class Class
    {

        [Key, Column(Order = 1)]

        [ForeignKey("ActivityName")]
        public string ActivityName { get; set; }

        [Key, Column(Order = 2)]
        [ForeignKey("UserId")]
        public int TrainerId { get; set; }

        public Activity Activity { get; set; }
        public Trainer Trainer { get; set; }
        public int Capacity { get; set; }
    }
}
