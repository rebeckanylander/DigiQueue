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

        public IActionResult DigiStudent(ClassroomDigiStudentVM id) //ClassroomDigiStudentVM = alias, classroomId
        {
            //if modelstate isvalid

            //var classroom = repository.GetClassroomById(vm.ClassroomId);
            //var model = ny vymovel som innehåller alias och classroom

            //Logik
            //Validera
            ClassroomDigiStudentVM model = new ClassroomDigiStudentVM { Alias = id.Alias, Classroom = id.Classroom };
            return View(model);
        }
    }
}
