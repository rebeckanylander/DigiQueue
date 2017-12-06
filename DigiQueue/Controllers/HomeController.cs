using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DigiQueue.Models.Viewmodels;
using DigiQueue.Models.Repositories;
using Microsoft.AspNetCore.Identity;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DigiQueue.Controllers
{
    public class HomeController : Controller
    {
        IRepository repository;
        //IdentityDbContext identityContext;
        UserManager<IdentityUser> userManager;
        SignInManager<IdentityUser> signInManager;
        RoleManager<IdentityRole> roleManager;

        public HomeController(
            IRepository repository,
            //IdentityDbContext identityContext,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.repository = repository;
            //this.identityContext = identityContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;

            //identityContext.Database.EnsureCreated();

        }


        public IActionResult Index()
        {
            var model = new HomeIndexVM
            {
                LoggedIn = User.Identity.IsAuthenticated,
                DigiStudent = new HomeIndexFindClassroomVM(), //classrooms = repository.FindAllClassrooms();
                CreateClassroom = new HomeIndexCreateClassroomVM(),
                DigiMaster = new HomeIndexLoginVM()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(HomeIndexLoginVM viewModel)
        {
            //var result =
            //    await userManager.CreateAsync(new IdentityUser("admin"), "P@ssword123");

            if (!ModelState.IsValid)
            {
                var model = new HomeIndexVM
                {
                    LoggedIn = User.Identity.IsAuthenticated,
                    DigiStudent = new HomeIndexFindClassroomVM(), //classrooms = repository.FindAllClassrooms();
                    CreateClassroom = new HomeIndexCreateClassroomVM(),
                    DigiMaster = viewModel
                };

                return RedirectToAction(nameof(Index), model);
            }

            var result = await signInManager.PasswordSignInAsync(
                viewModel.Username, viewModel.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(nameof(HomeIndexLoginVM.Username), "Invalid username/password");
                var model = new HomeIndexVM
                {
                    LoggedIn = User.Identity.IsAuthenticated,
                    DigiStudent = new HomeIndexFindClassroomVM(), 
                    CreateClassroom = new HomeIndexCreateClassroomVM(),
                    DigiMaster = viewModel
                };

                return RedirectToAction(nameof(Index), model);
            }

            return RedirectToAction(nameof(Index)); 
        }

        [HttpPost]
        public async Task<IActionResult> CreateClassroom(HomeIndexCreateClassroomVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                var model = new HomeIndexVM
                {
                    LoggedIn = User.Identity.IsAuthenticated,
                    DigiStudent = new HomeIndexFindClassroomVM(),
                    CreateClassroom = viewModel,
                    DigiMaster = new HomeIndexLoginVM()
                };

                return RedirectToAction(nameof(Index), model);
            }

            if (!await repository.IsClassroomNameAvailable(viewModel.Name))
            {
                var model = new HomeIndexVM
                {
                    LoggedIn = User.Identity.IsAuthenticated,
                    DigiStudent = new HomeIndexFindClassroomVM(),
                    CreateClassroom = viewModel,
                    DigiMaster = new HomeIndexLoginVM()
                };

                return RedirectToAction(nameof(Index), model);
            }

            string id = userManager.GetUserId(HttpContext.User);

            ClassroomDigiMasterVM modell = await repository.CreateClassroom(viewModel.Name, id);

            if(modell != null)
            {
                var model = new HomeIndexVM
                {
                    LoggedIn = User.Identity.IsAuthenticated,
                    DigiStudent = new HomeIndexFindClassroomVM(),
                    CreateClassroom = viewModel,
                    DigiMaster = new HomeIndexLoginVM()
                };

                return RedirectToAction(nameof(Index), model);
            }
            return RedirectToAction("DigiMaster", "Classroom");
        }

        [HttpPost]
        public async Task<IActionResult> FindClassroom(ClassroomDigiStudent viewModel)
        {
            if (!ModelState.IsValid)
            {
                var model = new HomeIndexVM
                {
                    LoggedIn = User.Identity.IsAuthenticated,
                    DigiStudent = new HomeIndexFindClassroomVM { Alias=viewModel.Alias },
                    CreateClassroom = new HomeIndexCreateClassroomVM(),
                    DigiMaster = new HomeIndexLoginVM()
                };

                return RedirectToAction(nameof(Index), model);
            }

            ClassroomDigiMasterVM modell = await repository.FindClassroom(viewModel.Alias, viewModel.Classroom);

            if (modell != null)
            {
                var model = new HomeIndexVM
                {
                    LoggedIn = User.Identity.IsAuthenticated,
                    DigiStudent = new HomeIndexFindClassroomVM { Alias = viewModel.Alias },
                    CreateClassroom = new HomeIndexCreateClassroomVM(),
                    DigiMaster = new HomeIndexLoginVM()
                };

                return RedirectToAction(nameof(Index), model);
            }
            return RedirectToAction("DigiStudent", "Classroom");
        }
    }
}
