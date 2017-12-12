using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DigiQueue.Models.Viewmodels
{
    public class AccountRegisterVM
    {
        [Display(Name = "Username*")]
        [Required(ErrorMessage = "Enter a username")]
        public string Username { get; set; }

        [Display(Name = "Password*")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Enter a password")]
        public string Password { get; set; }

        [Display(Name = "Classroom name*")]
        [Required(ErrorMessage = "Enter a classroom name")]
        public string ClassroomName { get; set; }

        public string OldClassroomName { get; set; }
    }
}
