using DigiQueue.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigiQueue.Models.Viewmodels
{
    public class ClassroomDigiStudentVM
    {
        public string Alias { get; set; }
        public UserExtension Classroom { get; set; }
    }
}
