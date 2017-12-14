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
using DigiQueue.Models;
using DigiQueue.Models.Hubs;

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
                DigiStudent = new HomeIndexFindClassroomVM { Classrooms = await repository.GetAllClassrooms(), Alias = TempData["alias"]?.ToString(), Message = TempData["findMessage"]?.ToString()}, //classrooms = repository.FindAllClassrooms();
                CreateClassroom = new HomeIndexCreateClassroomVM(),
                DigiMaster = new HomeIndexLoginVM { Username = user },
                Register = new AccountRegisterVM()
            };

            string id = "";
            if (User.Identity.IsAuthenticated)
            {
                id = repository.GetUserId(HttpContext.User);
                model.Register = new AccountRegisterVM { OldClassroomName = repository.GetClassroomNameByAspNetId(id), OldClassroomId = repository.GetClassroomId(id), Message = TempData["message"]?.ToString(), RegPassword = TempData["password"]?.ToString(), ClassroomName = TempData["classroomname"]?.ToString(), RegUsername = TempData["username"]?.ToString() };
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(HomeIndexLoginVM viewModel)
        {


            if (!ModelState.IsValid)
            {
                var model = new HomeIndexVM
                {
                    LoggedIn = User.Identity.IsAuthenticated,
                    DigiStudent = new HomeIndexFindClassroomVM { Classrooms = await repository.GetAllClassrooms() },  //classrooms = repository.FindAllClassrooms();
                    CreateClassroom = new HomeIndexCreateClassroomVM(),
                    DigiMaster = viewModel,
                    Register = new AccountRegisterVM()
                };

                return View(model);
            }


            Microsoft.AspNetCore.Identity.SignInResult result = await repository.SignIn(viewModel.Username, viewModel.Password);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(nameof(HomeIndexLoginVM.Username), "Invalid username/password");
                var model = new HomeIndexVM
                {
                    LoggedIn = User.Identity.IsAuthenticated,
                    DigiStudent = new HomeIndexFindClassroomVM { Classrooms = await repository.GetAllClassrooms() },
                    CreateClassroom = new HomeIndexCreateClassroomVM(),
                    DigiMaster = viewModel,
                    Register = new AccountRegisterVM()
                };

                return View(model);
            }

            var id = await repository.GetUserAsync(viewModel.Username);
            return RedirectToAction("DigiMaster", "Classroom");
        }

        //[Authorize]
        //[HttpPost]
        //public async Task<IActionResult> CreateClassroom(HomeIndexCreateClassroomVM viewModel)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        var model = new HomeIndexVM
        //        {
        //            LoggedIn = User.Identity.IsAuthenticated,
        //            DigiStudent = new HomeIndexFindClassroomVM(),
        //            CreateClassroom = viewModel,
        //            DigiMaster = new HomeIndexLoginVM()
        //        };

        //        return RedirectToAction(nameof(Index), model);
        //    }

        //    if (!await repository.IsClassroomNameAvailable(viewModel.Name))
        //    {
        //        var model = new HomeIndexVM
        //        {
        //            LoggedIn = User.Identity.IsAuthenticated,
        //            DigiStudent = new HomeIndexFindClassroomVM(),
        //            CreateClassroom = viewModel,
        //            DigiMaster = new HomeIndexLoginVM()
        //        };

        //        return RedirectToAction(nameof(Index), model);
        //    }
        //    string id = repository.GetUserId(HttpContext.User);

        //    ClassroomDigiMasterVM modell = await repository.CreateClassroom(viewModel.Name, id);

        //    if(modell == null)
        //    {
        //        var model = new HomeIndexVM
        //        {
        //            LoggedIn = User.Identity.IsAuthenticated,
        //            DigiStudent = new HomeIndexFindClassroomVM(),
        //            CreateClassroom = viewModel,
        //            DigiMaster = new HomeIndexLoginVM()
        //        };

        //        return RedirectToAction(nameof(Index), model);
        //    }
        //    int classroomid = modell.Classroom.Id;
        //    return RedirectToAction("DigiMaster", "Classroom", new { id = classroomid} );
        //}

        [HttpPost]
        public IActionResult FindClassroom(ClassroomDigiStudentVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                var model = new HomeIndexVM
                {
                    LoggedIn = User.Identity.IsAuthenticated,
                    DigiStudent = new HomeIndexFindClassroomVM { Alias = viewModel.Alias },
                    CreateClassroom = new HomeIndexCreateClassroomVM(),
                    DigiMaster = new HomeIndexLoginVM(),
                    Register = new AccountRegisterVM()
                };
                TempData["alias"] = viewModel.Alias;
                TempData["findMessage"] = "Fyll i alla fält";
                return RedirectToAction(nameof(Index), model);
            }

            if (viewModel.Alias.Length > 32)
            {
                var model = new HomeIndexVM
                {
                    LoggedIn = User.Identity.IsAuthenticated,
                    DigiStudent = new HomeIndexFindClassroomVM { Alias = viewModel.Alias },
                    CreateClassroom = new HomeIndexCreateClassroomVM(),
                    DigiMaster = new HomeIndexLoginVM(),
                    Register = new AccountRegisterVM()
                };
                TempData["alias"] = viewModel.Alias;
                TempData["findMessage"] = "Ditt alias är för långt, vänligen ange ett kortare alias";
                return RedirectToAction(nameof(Index), model);
            }

            var form = int.Parse(Request.Form["DropDownListKlasser"]);
            if ((DigiHub.loggedInList.Values.SingleOrDefault(p => p.Alias == viewModel.Alias && p.ClassroomName == repository.GetClassroomById(form))) != null)
            {
                var model = new HomeIndexVM
                {
                    LoggedIn = User.Identity.IsAuthenticated,
                    DigiStudent = new HomeIndexFindClassroomVM { Alias = viewModel.Alias },
                    CreateClassroom = new HomeIndexCreateClassroomVM(),
                    DigiMaster = new HomeIndexLoginVM(),
                    Register = new AccountRegisterVM()
                };
                TempData["alias"] = viewModel.Alias;
                TempData["findMessage"] = "Alias i detta klassrum är upptaget";
                return RedirectToAction(nameof(Index), model);
            }

            return RedirectToAction("DigiStudent", "Classroom", new { alias = UtilClass.ParseHtml(viewModel.Alias), classroomId = form });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Register(AccountRegisterVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                string user = repository.GetUsername(HttpContext.User);
                string id = repository.GetUserId(HttpContext.User);

                var model = new HomeIndexVM
                {
                    LoggedIn = User.Identity.IsAuthenticated,
                    DigiStudent = new HomeIndexFindClassroomVM { Classrooms = await repository.GetAllClassrooms() }, //classrooms = repository.FindAllClassrooms();
                    CreateClassroom = new HomeIndexCreateClassroomVM(),
                    DigiMaster = new HomeIndexLoginVM { Username = user },
                    Register = new AccountRegisterVM { OldClassroomName = repository.GetClassroomNameByAspNetId(id), OldClassroomId = repository.GetClassroomId(id) }
                };
                TempData["username"] = viewModel.RegUsername;
                TempData["password"] = viewModel.RegPassword;
                TempData["classroomname"] = viewModel.ClassroomName;
                TempData["message"] = "Fyll i alla fält.";
                return RedirectToAction(nameof(Index), model);
            }

            if (!await repository.IsClassroomNameAvailable(viewModel.ClassroomName))
            {
                string user = repository.GetUsername(HttpContext.User);
                string id = repository.GetUserId(HttpContext.User);

                var model = new HomeIndexVM
                {
                    LoggedIn = User.Identity.IsAuthenticated,
                    DigiStudent = new HomeIndexFindClassroomVM { Classrooms = await repository.GetAllClassrooms() }, //classrooms = repository.FindAllClassrooms();
                    CreateClassroom = new HomeIndexCreateClassroomVM(),
                    DigiMaster = new HomeIndexLoginVM { Username = user },
                    Register = new AccountRegisterVM { OldClassroomName = repository.GetClassroomNameByAspNetId(id), OldClassroomId = repository.GetClassroomId(id) }
                };
                TempData["username"] = viewModel.RegUsername;
                TempData["password"] = viewModel.RegPassword;
                TempData["classroomname"] = viewModel.ClassroomName;
                TempData["message"] = "Klassrumsnamn är upptaget.";
                return RedirectToAction(nameof(Index), model);
            }

            var result = await repository.CreateUser(viewModel.RegUsername, viewModel.RegPassword);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    if (error.Code.Contains("User"))
                    {
                        ModelState.AddModelError(nameof(AccountRegisterVM.RegUsername), error.Description);
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(AccountRegisterVM.RegPassword), error.Description);
                    }
                }
                string user = repository.GetUsername(HttpContext.User);
                string id = repository.GetUserId(HttpContext.User);

                var model = new HomeIndexVM
                {
                    LoggedIn = User.Identity.IsAuthenticated,
                    DigiStudent = new HomeIndexFindClassroomVM { Classrooms = await repository.GetAllClassrooms() }, //classrooms = repository.FindAllClassrooms();
                    CreateClassroom = new HomeIndexCreateClassroomVM(),
                    DigiMaster = new HomeIndexLoginVM { Username = user },
                    Register = new AccountRegisterVM { OldClassroomName = repository.GetClassroomNameByAspNetId(id), OldClassroomId = repository.GetClassroomId(id) }
                };
                TempData["username"] = viewModel.RegUsername;
                TempData["password"] = viewModel.RegPassword;
                TempData["classroomname"] = viewModel.ClassroomName;
                TempData["message"] = "Användarnamn eller lösenord är inte tillgänglig.";
                return RedirectToAction(nameof(Index), model);
            }

            ClassroomDigiMasterVM modell = await repository.CreateClassroom(UtilClass.ParseHtml(viewModel.ClassroomName), await repository.GetUserAsync(viewModel.RegUsername));

            return RedirectToAction("DigiMaster", "Classroom", new { id = await repository.GetClassroomIdByName(viewModel.OldClassroomName) });
        }

        public async Task<IActionResult> SignOut()
        {
            await repository.SignOut();

            string user = repository.GetUsername(HttpContext.User);

            var model = new HomeIndexVM
            {
                LoggedIn = User.Identity.IsAuthenticated,
                DigiStudent = new HomeIndexFindClassroomVM { Classrooms = await repository.GetAllClassrooms() }, //classrooms = repository.FindAllClassrooms();
                CreateClassroom = new HomeIndexCreateClassroomVM(),
                DigiMaster = new HomeIndexLoginVM { Username = user },
                Register = new AccountRegisterVM()
            };

            return RedirectToAction(nameof(Index), model);

        }
    }
}
