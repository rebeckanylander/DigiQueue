using DigiQueue.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DigiQueue.Models.Viewmodels
{
    public class HomeIndexFindClassroomVM
    {
        [Required(ErrorMessage="Enter alias")]
        public string Alias { get; set; }

        [Required(ErrorMessage = "Choose a classroom")]
        public Classroom[] Classrooms { get; set; }

        //public SelectList SelectList { get; set; }

    }
}
