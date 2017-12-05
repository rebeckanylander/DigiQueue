using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DigiQueue.Models.Viewmodels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DigiQueue.Controllers
{
    public class AccountController : Controller
    {
        //IdentityDbContext identityContext;
        UserManager<IdentityUser> userManager;
        SignInManager<IdentityUser> signInManager;
        RoleManager<IdentityRole> roleManager;

        public AccountController(
            //IdentityDbContext identityContext,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager)
        {
            //this.identityContext = identityContext;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;

            //identityContext.Database.EnsureCreated();

        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            //var result =
            //    await userManager.CreateAsync(new IdentityUser("admin"), "P@ssword123");

            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginVM viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var result = await signInManager.PasswordSignInAsync(
                viewModel.Username, viewModel.Password, false, false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(nameof(AccountLoginVM.Username), "Invalid username/password");
                return View(viewModel);
            }

            return RedirectToAction(nameof(Index), "Home"); //Varför inte nameof(ContactController)?
        }
    }
}
