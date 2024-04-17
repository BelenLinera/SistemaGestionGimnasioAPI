using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaGestionGimnasioApi.Data.Entities
{
    public class Reserve
    {


        [Key, Column(Order = 1)]
        [ForeignKey("DateTime")]
        public DateTime DateTimeClass { get; set; }
        [Key, Column(Order = 2)]
        [ForeignKey("UserEmail")]
        public int ClientEmail { get; set; }
        public Class Class { get; set; }
        public Client Client { get; set; }
    }
}
