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
using Newtonsoft.Json;

namespace DigiQueue.Controllers
{
    public class ClassroomController : Controller
    {
        IRepository repository;

        public ClassroomController(IRepository repository)
        {
            this.repository = repository;
        }

        [Authorize]
        public IActionResult Stats()
        {
            return View();
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> DigiMaster(int id)
        {
            ClassroomDigiMasterVM model = await repository.FindClassroom(id);
            return View(model);
        }

        [HttpGet]
        [Route("Classroom/DigiStudent/{alias}/{classroomId}")]
        public async Task<IActionResult> DigiStudent(string alias, int classroomId)
        {
            var classroom = await repository.FindClassroom(classroomId);

            ClassroomDigiStudentVM model = new ClassroomDigiStudentVM { Alias = alias, Classroom = classroom.Classroom };
            return View(model);
        }

        public string GetTime()
        {
            return JsonConvert.SerializeObject(repository.GetTimeArray(repository.GetUserId(HttpContext.User)));
        }

        public string GetLanguage()
        {
            return JsonConvert.SerializeObject(repository.GetLanguageArray(repository.GetUserId(HttpContext.User)));
        }
    }
}
