using DigiQueue.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DigiQueue.Models.Viewmodels
{
    public class HomeIndexDigiStudentVM
    {
        [Required(ErrorMessage="Enter alias")]
        public string Alias { get; set; }

        [Required(ErrorMessage = "Choose a classroom")]
        public Classroom Classroom { get; set; }
    }
}
