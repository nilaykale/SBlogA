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
            return View(new UsersNew() {
                Roles =Database.Session.Query<Role>().Select(role=> 
                new RoleCheckBox() {
                    Id=role.Id,
                    IsChecked=false,
                    Name =role.Name
                    }
                ).ToList()
            });
        }

        private void SyncRoles(IList<RoleCheckBox> checkBoxes, IList<Role> roles)
        {
            var selectedRoles = new List<Role>();

            foreach(var role in Database.Session.Query<Role>())
            {
                var checkbox = checkBoxes.Single(c => c.Id == role.Id);
                checkbox.Name = role.Name;

                if (checkbox.IsChecked)
                {
                    selectedRoles.Add(role);
                }
            }

            foreach(var toAdd in selectedRoles.Where(t => !roles.Contains(t)))
            {
                roles.Add(toAdd);
            }

            foreach (var toRemove in roles.Where(t => !selectedRoles.Contains(t)).ToList())
            {
                roles.Remove(toRemove);
            }

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

            SyncRoles(formData.Roles, user.Roles);
            

            user.SetPassword(formData.Password);
            Database.Session.Save(user); //insert into Users (USername,password_hash,email) values ....
            Database.Session.Flush();
            return RedirectToAction("Index");
            

        }

        public ActionResult Edit(int id)
        {
            var user = Database.Session.Load<User>(id);
           
            if (user == null)
            {
                return HttpNotFound();
            }


            return View(
                new UsersEdit() {
                    Username=user.Username,
                    Email=user.Email,
                    Roles=Database.Session.Query<Role>().Select(role=> new RoleCheckBox() {
                        Id=role.Id,
                        Name=role.Name,
                        IsChecked=user.Roles.Contains(role)
                    }).ToList()
                }
            );
        }

        [HttpPost]
        public ActionResult Edit(int id,UsersEdit formData)
        {

            var user = Database.Session.Load<User>(id);
            if (user == null)
            {
                return HttpNotFound();
            }


            if (Database.Session.Query<User>().Any(u => u.Username == formData.Username && u.Id!=id))
            {
                ModelState.AddModelError("Username", "Username must be unique");
            }

            if (!ModelState.IsValid)
            {
                return View(formData);
            }

            user.Username = formData.Username;
            user.Email = formData.Email;
            SyncRoles(formData.Roles, user.Roles);
            Database.Session.Update(user); //insert into Users (USername,password_hash,email) values ....
            Database.Session.Flush();
            return RedirectToAction("Index");


        }


        public ActionResult ResetPassword(int id)
        {
            var user = Database.Session.Load<User>(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(new UsersResetPassword() {
                Username=user.Username
            });
        }

        [HttpPost]
        public ActionResult ResetPassword(int id, UsersResetPassword formData)
        {

            var user = Database.Session.Load<User>(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            formData.Username = user.Username;

            if (Database.Session.Query<User>().Any(u => u.Username == formData.Username && u.Id != id))
            {
                ModelState.AddModelError("Username", "Username must be unique");
            }

            if (!ModelState.IsValid)
            {
                return View(formData);
            }

            user.SetPassword(formData.Password);
            Database.Session.Update(user); //insert into Users (USername,password_hash,email) values ....
            Database.Session.Flush();
            return RedirectToAction("Index");


        }



        [HttpPost]
        public ActionResult Delete(int id)
        {
            var user = Database.Session.Load<User>(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            Database.Session.Delete(user);
            Database.Session.Flush();

            return RedirectToAction("Index");
                
        }
    }
}

//Database.Session.Query<User>().ToList() = select * from users