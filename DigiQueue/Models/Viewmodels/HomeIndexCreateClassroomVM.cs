using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DigiQueue.Models.Viewmodels
{
    public class HomeIndexCreateClassroomVM
    {
        [Required(ErrorMessage = "Enter Classroom name")]
        public string Name { get; set; }

    }
}
