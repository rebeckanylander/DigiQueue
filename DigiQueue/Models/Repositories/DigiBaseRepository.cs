using DigiQueue.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiQueue.Models.Viewmodels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace DigiQueue.Models.Repositories
{
    public class DigiBaseRepository : IRepository
    {
        DigibaseContext context;
        UserManager<IdentityUser> userManager;
        SignInManager<IdentityUser> signInManager;
        RoleManager<IdentityRole> roleManager;

        public DigiBaseRepository(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            DigibaseContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
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

        public string GetUserId(ClaimsPrincipal claimsPrincipal)
        {
            return userManager.GetUserId(claimsPrincipal);
        }

        public string GetUsername(ClaimsPrincipal claimsPrincipal)
        {
            return userManager.GetUserName(claimsPrincipal);
        }

        public async Task<bool> IsClassroomNameAvailable(string name)
        {
            Classroom classroom = await context.Classroom.FirstOrDefaultAsync(c => c.Name == name);
            return classroom == null;
        }

        public void SaveChatToDigiBase(ProtocolMessage json)
        {
            Message message = new Message
            {
                Alias = json.Alias,
                ClassroomId = json.ClassroomId,
                Date = json.Date,
                Content = json.Description
            };
            context.Message.Add(message);
            context.SaveChanges();
        }

        public void SaveProblemToDigiBase(ProtocolMessage json)
        {
            Problem problem = new Problem
            {
                Alias = json.Alias,
                ClassroomId = json.ClassroomId,
                Date = json.Date,
                Description = json.Description,
                Type = (int)json.PType
            };
            context.Problem.Add(problem);
            context.SaveChanges();
        }

        public async Task<SignInResult> SignIn(string username, string password)
        {
            return await signInManager.PasswordSignInAsync(
                username, password, false, false);

        }
    }
}
