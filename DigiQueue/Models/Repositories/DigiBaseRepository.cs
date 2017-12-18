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
            await context.UserExtension.AddAsync(new UserExtension { Name = name, AspUserId = id });
            await context.SaveChangesAsync();

            return new ClassroomDigiMasterVM { Classroom = await context.UserExtension.SingleOrDefaultAsync(c => c.Name == name && c.AspUserId == id) };
        }

        public async Task<IdentityResult> CreateUser(string username, string password)
        {
            return await userManager.CreateAsync(new IdentityUser(username), password);
        }

        public void EndProblem(string alias, string classroomId)
        {
            var problem = context.Problem.Last(p => p.Alias == alias && p.ClassroomId == context.UserExtension.Single(c => c.Name == classroomId).Id);
            problem.EndDate = DateTime.Now;
            context.SaveChanges();
        }

        public async Task<ClassroomDigiMasterVM> FindClassroom(string alias, UserExtension classroom)
        {
            return new ClassroomDigiMasterVM { Classroom = await context.UserExtension.SingleOrDefaultAsync(c => c == classroom) };
        }

        public async Task<ClassroomDigiMasterVM> FindClassroom(int id)
        {
            return new ClassroomDigiMasterVM { Classroom = await context.UserExtension.SingleOrDefaultAsync(c => c.Id == id) };
        }

        public async Task<ClassroomDigiMasterVM> FindClassroom(ClaimsPrincipal user)
        {
            var use = userManager.GetUserId(user);
            return new ClassroomDigiMasterVM { Classroom = await context.UserExtension.SingleOrDefaultAsync(c => c.AspUserId == use) };
        }

        public async Task<UserExtension[]> GetAllClassrooms()
        {
            return await context.UserExtension.ToArrayAsync();
        }

        public string GetClassroomById(int form)
        {
            return context.UserExtension.SingleOrDefault(x => x.Id == form)?.Name;
        }

        public int GetClassroomId(string id)
        {
            var classroomid = context.UserExtension.Single(c => c.AspUserId == id).Id;
            return classroomid;
        }

        public async Task<int> GetClassroomIdByName(string oldClassroomName)
        {
            var classroom = await context.UserExtension.SingleAsync(c => c.Name == oldClassroomName);
            return classroom.Id;
        }

        public string GetClassroomNameByAspNetId(string user)
        {
            var classroomName = context.UserExtension.Single(c => c.AspUserId == user).Name;
            return classroomName;
        }

        public int[] GetLanguageArray(string id)
        {
            var classroomid = GetClassroomId(id);
            List<int> list = new List<int>();
            var lister = context.Problem.Where(x => x.ClassroomId == classroomid).Select(x => x.Type).ToList();
            for (int i = 0; i < 7; i++)
            {
                list.Add(lister.Count(x => x == i));
            }
            return list.ToArray();
        }

        public int[] GetTimeArray(string id)
        {
            var classroomid = GetClassroomId(id);
            var listers = context.Problem.Where(x => x.ClassroomId == classroomid).ToList();
            var lister = listers.Where(x => x.EndDate != null).ToList();
            var list = lister.Select(x => (TimeSpan)(x.EndDate - x.StartDate));
            var lis = list.Select(x => new {Hours = x.Hours, Minutes = x.Minutes } );

            return new int[] {
                lis.Count(x => x.Minutes < 5 && x.Hours == 0),
                lis.Count(x => x.Minutes >= 5 && x.Minutes < 15 && x.Hours == 0),
                lis.Count(x => x.Minutes >= 15 && x.Minutes < 30 && x.Hours == 0),
                lis.Count(x => x.Minutes >= 30 && x.Minutes < 60 && x.Hours == 0),
                lis.Count(x => x.Hours == 1),
                lis.Count(x => x.Hours > 1),
                context.Problem.Where(x => x.ClassroomId == classroomid).Count(x => x.EndDate == null)
            };
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
            UserExtension classroom = await context.UserExtension.FirstOrDefaultAsync(c => c.Name == name);
            return classroom == null;
        }

        public void SaveChatToDigiBase(ProtocolMessage json)
        {
            Message message = new Message
            {
                Alias = json.Alias,
                ClassroomId = context.UserExtension.SingleOrDefault(c => c.Name == json.ClassroomName).Id,
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
                ClassroomId = context.UserExtension.SingleOrDefault(c => c.Name == json.ClassroomName).Id,
                StartDate = DateTime.Now,
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

        public async Task SignOut()
        {
            await signInManager.SignOutAsync();
        }
    }
}
