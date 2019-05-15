using SBlogA.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SBlogA.Areas.Admin.ViewModels
{
    public class UsersIndex
    {
        public IEnumerable<User> Users { get; set; }
    }

    public class UsersNew
    {
        public IList<RoleCheckBox> Roles { get; set; }
        [Required, MaxLength(128)]
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [Required, DataType(DataType.EmailAddress),MaxLength(256)]
        public string Email { get; set; }
    }

    public class UsersEdit
    {
       

        [Required, MaxLength(128)]
        public string Username { get; set; }

        [Required, DataType(DataType.EmailAddress), MaxLength(256)]
        public string Email { get; set; }

        public IList<RoleCheckBox> Roles { get; set; }
    }

    public class UsersResetPassword
    {
        
        public string Username { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }

    public class RoleCheckBox
    {
        public int Id { get; set; }
        public bool IsChecked { get; set; }
        public string Name { get; set; }
    }
}