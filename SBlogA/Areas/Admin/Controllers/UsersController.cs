using NHibernate.Linq;
using SBlogA.Areas.Admin.ViewModels;
using SBlogA.Infrastructure;
using SBlogA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBlogA.Areas.Admin.Controllers
{
    [Authorize(Roles ="admin")]
    public class UsersController : Controller
    {
        [SelectedTabAttribute("c")]
        public ActionResult Index()
        {
            return View(new UsersIndex() {
                Users=Database.Session.Query<User>().ToList()
            });
        }

        public ActionResult New()
        {
            return View(new UsersNew());
        }

        [HttpPost]
        public ActionResult New(UsersNew formData)
        {
            
            if (Database.Session.Query<User>().Any(u => u.Username == formData.Username))
            {
                ModelState.AddModelError("Username", "Username must be unique");
            }

            if (!ModelState.IsValid)
            {
                return View(formData);
            }

            var user = new User()
            {
                Username = formData.Username,
                PasswordHash = formData.Password,
                Email = formData.Email
            };

            user.SetPassword(formData.Password);
            Database.Session.Save(user); //insert into Users (USername,password_hash,email) values ....

            return RedirectToAction("Index");
            

        }


    }
}

//Database.Session.Query<User>().ToList() = select * from users