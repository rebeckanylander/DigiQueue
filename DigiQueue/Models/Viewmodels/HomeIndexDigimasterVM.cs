using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DigiQueue.Models.Viewmodels
{
    public class HomeIndexDigimasterVM
    {
        [Required(ErrorMessage = "Enter digital classroom name")]
        public string Name { get; set; }
        public bool ChatBox { get; set; }
    }
}
