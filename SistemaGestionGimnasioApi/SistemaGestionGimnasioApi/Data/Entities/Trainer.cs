using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaGestionGimnasioApi.Data.Entities
{
    public class Trainer : User
    {

        public List<TrainerActivity> TrainerActivities { get; set; } = new List<TrainerActivity>();
    }
}
