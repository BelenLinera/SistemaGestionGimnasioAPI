using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaGestionGimnasioApi.Data.Entities
{
    public class Reserve
    {
        [Key, Column(Order = 1)]
        [ForeignKey("Activity")]
        public string ActivityName { get; set; }

        [Key, Column(Order = 2)]
        [ForeignKey("DateTime")]
        public DateTime DateTime { get; set; }

        [Key, Column(Order = 3)]
        [ForeignKey("ClientId")]
        public int ClientId { get; set; }

        public Activity Activity { get; set; }
        public Client Client { get; set; }
    }
}
