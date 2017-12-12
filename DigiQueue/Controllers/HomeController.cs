using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DigiQueue.Models.Viewmodels;
using DigiQueue.Models.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace DigiQueue.Controllers
{
    public class HomeController : Controller
    {
        IRepository repository;

        public HomeController(IRepository repository)
        {
            this.repository = repository;
        }


        public async Task<IActionResult> Index()
        {
            string user = repository.GetUsername(HttpContext.User);

            //IdentityResult result = await repository.CreateUser("admin2", "P@ssword123");

            var model = new HomeIndexVM
            {
                LoggedIn = User.Identity.IsAuthenticated,
                DigiStudent = new HomeIndexFindClassroomVM { Classrooms = await repository.GetAllClassrooms() }, //classrooms = repository.FindAllClassrooms();
                CreateClassroom = new HomeIndexCreateClassroomVM(),
                DigiMaster = new HomeIndexLoginVM { Username = user }
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(HomeIndexLoginVM viewModel)
        {
            

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


            Microsoft.AspNetCore.Identity.SignInResult result = await repository.SignIn(viewModel.Username, viewModel.Password);

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

            var id = await repository.GetUserAsync(viewModel.Username);
            return RedirectToAction("DigiMaster", "Classroom", new { id = repository.GetClassroomId(id) }); 
        }

        [Authorize]
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
            string id = repository.GetUserId(HttpContext.User);

            ClassroomDigiMasterVM modell = await repository.CreateClassroom(viewModel.Name, id);

            if(modell == null)
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
            int classroomid = modell.Classroom.Id;
            return RedirectToAction("DigiMaster", "Classroom", new { id = classroomid} );
        }

        [HttpPost]
        public IActionResult FindClassroom(ClassroomDigiStudentVM viewModel)
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

            var form = int.Parse(Request.Form["DropDownListKlasser"]);

            return RedirectToAction("DigiStudent", "Classroom", new { alias = viewModel.Alias, classroomId = form });
        }

        [HttpGet]
        [Authorize]
        public IActionResult Register(string id)
        {
            var model = new AccountRegisterVM
            {
                OldClassroomName = id
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Register(AccountRegisterVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            if (!await repository.IsClassroomNameAvailable(viewModel.ClassroomName))
            {
                return View(viewModel);
            }

            var result = await repository.CreateUser(viewModel.Username, viewModel.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    if (error.Code.Contains("User"))
                    {
                        ModelState.AddModelError(nameof(AccountRegisterVM.Username), error.Description);
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(AccountRegisterVM.Password), error.Description);
                    }
                }
                return View(viewModel);
            }

            ClassroomDigiMasterVM modell = await repository.CreateClassroom(viewModel.ClassroomName, await repository.GetUserAsync(viewModel.Username));

            return RedirectToAction("DigiMaster", "Classroom", new { id = await repository.GetClassroomIdByName(viewModel.OldClassroomName) });
        }
    }
}
