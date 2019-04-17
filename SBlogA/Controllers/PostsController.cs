using SBlogA.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBlogA.Controllers
{
    public class PostsController : Controller
    {
        [SelectedTabAttribute("post list")]
        public ActionResult Index()
        {
            return View();
        }
    }
}