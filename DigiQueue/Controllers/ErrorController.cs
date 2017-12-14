using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DigiQueue.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult HttpError(int id)
        {
            return View(id);
        }

        public IActionResult ServerError()
        {
            return View();
        }

        public IActionResult InputError()
        {
            return View();
        }
    }
}
