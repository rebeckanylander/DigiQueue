﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiQueue.Models.Entities;
using DigiQueue.Models.Viewmodels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DigiQueue.Models.Repositories
{
    public interface IRepository
    {
        Task<ClassroomDigiMasterVM> CreateClassroom(string name, string id);
        Task<bool> IsClassroomNameAvailable(string name);
        Task<ClassroomDigiMasterVM> FindClassroom(string alias, Classroom classroom);
        Task<ClassroomDigiMasterVM> FindClassroom(int id);
        Task<Classroom[]> GetAllClassrooms();
        Task<SignInResult> SignIn(string username, string password);
        string GetUserId(ClaimsPrincipal claimsPrincipal);
        string GetUsername(ClaimsPrincipal claimsPrincipal);
        void SaveChatToDigiBase(ProtocolMessage json);
        void SaveProblemToDigiBase(ProtocolMessage json);
    }
}
