using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DigiQueue.Models.Viewmodels;
using DigiQueue.Models.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DigiQueue.Controllers
{
    public class HomeController : Controller
    {
        IRepository repository;

        public HomeController(IRepository repository)
        {
            this.repository = repository;
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
    }
}
