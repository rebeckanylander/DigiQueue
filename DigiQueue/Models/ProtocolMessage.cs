using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigiQueue.Models
{
    public class ProtocolMessage
    {
        public string Command { get; set; }
        public string Alias { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string ClassroomName { get; set; }
        public PType PType { get; set; }
        public string Location { get; set; }
    }
}
