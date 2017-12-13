using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigiQueue.Models
{
    public class ProblemVM
    {
        public string Alias { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string ClassroomName { get; set; }
        public string Language { get; set; }
        public DateTime Time { get; set; }
    }
}
