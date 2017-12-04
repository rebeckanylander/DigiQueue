using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigiQueue.Models.Entities
{
    public partial class DigibaseContext : DbContext
    {
        public DigibaseContext(DbContextOptions<DigibaseContext> options) : base(options)
        {

        }
    }
}
