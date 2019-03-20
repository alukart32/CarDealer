using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CarDealer.Models.Users.Infrastructure;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using CarDealer.Models.Users.Models;
using System.Threading.Tasks;

namespace CarDealer.Models.Users.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            InitUsers();
            return View(UserManager.Users);
        }

        public ActionResult Create()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<ActionResult> Create(CreateModel model)
        private void Create(String name, String email, String psswrd)
        {
          AppUser user = new AppUser { UserName = name, Email = email };
          UserManager.Create(user, psswrd);  
        }

        private void InitUsers()
        {
            // создание admin
            Create("admin", "admin@gmail.com", "groot");
            // пользователь
            //Create("user", "user@gmail.com", "user_groot");
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private AppUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
            }
        }
    }
}