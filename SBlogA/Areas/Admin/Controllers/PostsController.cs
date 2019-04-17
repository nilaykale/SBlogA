﻿using SBlogA.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SBlogA.Areas.Admin.Controllers
{
    [Authorize(Roles="admin")]
    public class PostsController : Controller
    {
        [SelectedTabAttribute("a")]
        public ActionResult Index()
        {
            return View();
        }

        [SelectedTabAttribute("b")]
        public ActionResult List()
        {
            return View();
        }

    }
}