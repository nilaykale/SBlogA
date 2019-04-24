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
    }
}

//Database.Session.Query<User>().ToList() = select * from users