using System;
using System.Collections.Generic;

namespace DigiQueue.Models.Entities
{
    public partial class Message
    {
        public int Id { get; set; }
        public string Alias { get; set; }
        public int ClassroomId { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }

        public Classroom Classroom { get; set; }
    }
}
