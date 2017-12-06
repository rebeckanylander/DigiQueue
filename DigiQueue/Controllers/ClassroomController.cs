using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DigiQueue.Models.Viewmodels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DigiQueue.Models.Repositories;
using DigiQueue.Models.Entities;

namespace DigiQueue.Controllers
{
    public class ClassroomController : Controller
    {

        UserManager<IdentityUser> userManager;
        SignInManager<IdentityUser> signInManager;
        RoleManager<IdentityRole> roleManager;
        IRepository repository;

        public ClassroomController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IRepository repository)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> DigiMaster(int id)
        {
            ClassroomDigiMasterVM model = await repository.FindClassroom(id);
            return View(model);
        }

        [HttpGet]
        [Route("Classroom/DigiStudent/{alias}/{classroomId}")]
        public async Task<IActionResult> DigiStudent(string alias, int classroomId) //ClassroomDigiStudentVM = alias, classroomId
        {
            //if modelstate isvalid

            //var classroom = repository.GetClassroomById(vm.ClassroomId);
            //var model = ny vymovel som innehåller alias och classroom

            //Logik
            //Validera
            var classroom = await repository.FindClassroom(classroomId);

            ClassroomDigiStudentVM model = new ClassroomDigiStudentVM { Alias = alias, Classroom = classroom.Classroom };
            return View(model);
        }
    }
}
