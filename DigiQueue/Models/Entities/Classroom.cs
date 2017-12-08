using System;
using System.Collections.Generic;

namespace DigiQueue.Models.Entities
{
    public partial class Classroom
    {
        public Classroom()
        {
            Message = new HashSet<Message>();
            Problem = new HashSet<Problem>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string AspUserId { get; set; }

        public ICollection<Message> Message { get; set; }
        public ICollection<Problem> Problem { get; set; }
    }
}
