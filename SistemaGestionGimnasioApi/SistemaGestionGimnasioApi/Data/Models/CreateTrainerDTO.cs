﻿using System.ComponentModel.DataAnnotations;

namespace SistemaGestionGimnasioApi.Data.Models
{
    public class CreateTrainerDTO
    {
        [Required(ErrorMessage = "Este campo no puede quedar vacio")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Este campo no puede quedar vacio")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Este campo no puede quedar vacio")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Este campo no puede quedar vacio")]
        public string Password { get; set; }
        public List<int> Activitys { get; set; } 
    }
}
