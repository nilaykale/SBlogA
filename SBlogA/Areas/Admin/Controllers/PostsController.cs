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
    [Authorize(Roles="admin")]
    public class PostsController : Controller
    {
        private const int PostsPerPage = 5;

        [SelectedTabAttribute("a")]
        public ActionResult Index(int page=1)
        {

            var totalPostCount = Database.Session.Query<Post>().Count();
            var currentItems = Database.Session.Query<Post>()
                .OrderByDescending(p => p.CreatedAt)
                .Skip((page - 1) * PostsPerPage)
                .Take(PostsPerPage)
                .ToList();


            return View( new PostsIndex() {
               Posts=new PagedData<Post>(currentItems,totalPostCount,page,PostsPerPage)
            });
        }

        [SelectedTabAttribute("b")]
        public ActionResult List()
        {
            return View();
        }

        public ActionResult New()
        {
            return View("Form", new PostsForm() {IsNew=true });
        }

        public ActionResult Edit(int Id)
        {

            var post = Database.Session.Load<Post>(Id);

            if (post == null)
                return HttpNotFound();

            return View("Form", new PostsForm() { IsNew = false,PostId=Id,Title=post.Title,Slug=post.Slug,Content=post.Content });
        }


        public ActionResult Trash(int id)
        {

            var post = Database.Session.Load<Post>(id);

            if (post == null)
                return HttpNotFound();


            post.DeletedAt = DateTime.Now;
            Database.Session.SaveOrUpdate(post);
            Database.Session.Flush();

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {

            var post = Database.Session.Load<Post>(id);

            if (post == null)
                return HttpNotFound();


            Database.Session.Delete(post);
            Database.Session.Flush();

            return RedirectToAction("Index");
        }




        public ActionResult Restore(int id)
        {

            var post = Database.Session.Load<Post>(id);

            if (post == null)
                return HttpNotFound();


            post.DeletedAt = null;
            Database.Session.SaveOrUpdate(post);
            Database.Session.Flush();

            return RedirectToAction("Index");
        }

        [HttpPost,ValidateInput(false)]
        public ActionResult Form(PostsForm formData)
        {
            formData.IsNew = formData.PostId == null;

            if (!ModelState.IsValid)
                return View(formData);

            Post post;
            if (formData.IsNew)
            {
                post = new Post()
                {
                    CreatedAt = DateTime.Now,
                    User = Auth.User
                };
            }
            else
            {
                post = Database.Session.Load<Post>(formData.PostId);
                if (post == null)
                    return HttpNotFound();

                post.UpdatedAt = DateTime.Now;
            }

            post.Title = formData.Title;
            post.Slug = formData.Slug;
            post.Content = formData.Content;

            Database.Session.SaveOrUpdate(post);
            Database.Session.Flush();

            return RedirectToAction("Index");
        }

    }
}