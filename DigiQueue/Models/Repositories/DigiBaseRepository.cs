using DigiQueue.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiQueue.Models.Viewmodels;
using Microsoft.EntityFrameworkCore;

namespace DigiQueue.Models.Repositories
{
    public class DigiBaseRepository : IRepository
    {
        DigibaseContext context;

        public DigiBaseRepository(DigibaseContext context)
        {
            this.context = context;
        }

        public async Task<ClassroomDigiMasterVM> CreateClassroom(string name, string id)
        {
            await context.Classroom.AddAsync(new Classroom { Name = name, AspUserId = id });
            await context.SaveChangesAsync();

            return new ClassroomDigiMasterVM { Classroom = await context.Classroom.SingleOrDefaultAsync(c => c.Name == name && c.AspUserId == id) };
        }

        public async Task<ClassroomDigiMasterVM> FindClassroom(string alias, Classroom classroom)
        {
            return new ClassroomDigiMasterVM { Classroom = await context.Classroom.SingleOrDefaultAsync(c => c == classroom) };
        }

        public async Task<ClassroomDigiMasterVM> FindClassroom(int id)
        {
            return new ClassroomDigiMasterVM { Classroom = await context.Classroom.SingleOrDefaultAsync(c => c.Id == id) };
        }

        public async Task<Classroom[]> GetAllClassrooms()
        {
            return await context.Classroom.ToArrayAsync();
        }

        public async Task<bool> IsClassroomNameAvailable(string name)
        {
            Classroom classroom = await context.Classroom.FirstOrDefaultAsync(c => c.Name == name);
            return classroom == null;
        }
    }
}
