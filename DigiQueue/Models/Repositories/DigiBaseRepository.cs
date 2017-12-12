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
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DigiQueue.Models.Repositories
{
    public class DigiBaseRepository : IRepository
    {
        IdentityDbContext identityContext;
        DigibaseContext context;
        UserManager<IdentityUser> userManager;
        SignInManager<IdentityUser> signInManager;
        RoleManager<IdentityRole> roleManager;

        public DigiBaseRepository(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            DigibaseContext context,
            IdentityDbContext identityContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.context = context;
            this.identityContext = identityContext;
        }

        public async Task<ClassroomDigiMasterVM> CreateClassroom(string name, string id)
        {
            await context.Classroom.AddAsync(new Classroom { Name = name, AspUserId = id });
            await context.SaveChangesAsync();

            return new ClassroomDigiMasterVM { Classroom = await context.Classroom.SingleOrDefaultAsync(c => c.Name == name && c.AspUserId == id) };
        }

        public async Task<IdentityResult> CreateUser(string username, string password)
        {
            return await userManager.CreateAsync(new IdentityUser(username), password);
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

        public int GetClassroomId(string id)
        {
            var classroomid = context.Classroom.Single(c => c.AspUserId == id).Id;
            return classroomid;
        }

        public async Task<int> GetClassroomIdByName(string oldClassroomName)
        {
            var classroom = await context.Classroom.SingleAsync(c => c.Name == oldClassroomName);
            return classroom.Id;
        }

        public async Task<string> GetUserAsync(string username)
        {
            var user = await identityContext.Users.SingleAsync(o => o.NormalizedUserName.Equals(username, StringComparison.InvariantCultureIgnoreCase));
            return user.Id;
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
                ClassroomId = context.Classroom.SingleOrDefault(c => c.Name == json.ClassroomId).Id,
                Date = DateTime.Now,
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
                ClassroomId = context.Classroom.SingleOrDefault(c => c.Name == json.ClassroomId).Id,
                Date = DateTime.Now,
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
