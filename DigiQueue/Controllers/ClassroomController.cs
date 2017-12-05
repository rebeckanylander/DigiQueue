using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DigiQueue.Models.Viewmodels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using DigiQueue.Models.Repositories;

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

        [HttpPost]
        public IActionResult DigiMaster(HomeIndexCreateClassroomVM vm)
        {
            //if modelstate isvalid
            //if klassrumnamn valid

            string id = userManager.GetUserId(HttpContext.User);

            //lägga till klassrum i db          id, namn, aspnetid
            //ClassroomDigiMasterVM model = repostiory.CreateClassroom(namn, aspnetid);
            //if(model != null)
            //return view(mdeol);


            return View();
        }

        public IActionResult DigiStudent(HomeIndexFindClassroomVM vm) //ClassroomDigiStudentVM = alias, classroomId
        {
            //if modelstate isvalid

            //var classroom = repository.GetClassroomById(vm.ClassroomId);
            //var model = ny vymovel som innehåller alias och classroom

            //Logik
            //Validera
            return View();
        }
    }
}
