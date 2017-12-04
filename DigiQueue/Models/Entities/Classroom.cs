using System;
using System.Collections.Generic;

namespace DigiQueue.Models.Entities
{
    public partial class Classroom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AspUserId { get; set; }
    }
}
