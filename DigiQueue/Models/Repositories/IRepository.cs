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
        Task<ClassroomDigiMasterVM> FindClassroom(string alias, UserExtension classroom);
        Task<ClassroomDigiMasterVM> FindClassroom(int id);
        Task<UserExtension[]> GetAllClassrooms();
        Task<SignInResult> SignIn(string username, string password);
        string GetUserId(ClaimsPrincipal claimsPrincipal);
        Task<IdentityResult> CreateUser(string username, string password);
        Task<string> GetUserAsync(string username);
        string GetUsername(ClaimsPrincipal claimsPrincipal);
        Task<ClassroomDigiMasterVM> FindClassroom(ClaimsPrincipal user);
        void SaveChatToDigiBase(ProtocolMessage json);
        void SaveProblemToDigiBase(ProtocolMessage json);
        int GetClassroomId(string id);
        Task<int> GetClassroomIdByName(string oldClassroomName);
        string GetClassroomNameByAspNetId(string user);
        Task SignOut();
        void EndProblem(string alias, string classroomId);
        int[] GetTimeArray(string id);
        int[] GetLanguageArray(string id);
        string GetClassroomById(int form);
    }
}
