using DigiQueue.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DigiQueue.Models.Repositories
{
    public class DigiBaseRepository : IRepository
    {
        DigibaseContext context;

        public DigiBaseRepository(DigibaseContext context)
        {
            this.context = context;
        }
    }
}
