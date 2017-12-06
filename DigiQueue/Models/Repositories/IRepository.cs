using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiQueue.Models.Entities;
using DigiQueue.Models.Viewmodels;

namespace DigiQueue.Models.Repositories
{
    public interface IRepository
    {
        Task<ClassroomDigiMasterVM> CreateClassroom(string name, string id);
        Task<bool> IsClassroomNameAvailable(string name);
        Task<ClassroomDigiMasterVM> FindClassroom(string alias, Classroom classroom);
    }
}
