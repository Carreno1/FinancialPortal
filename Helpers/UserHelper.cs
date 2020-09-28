using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Carreno_Financial_Portal.Models;
using Microsoft.AspNet.Identity;

namespace Carreno_Financial_Portal.Helpers
{
    public class UserHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string UserId { get; set; }

        public UserHelper()
        {
            UserId = HttpContext.Current.User.Identity.GetUserId();
        }

        public string GetUserDisplayName()
        {
            return db.Users.Find(UserId).FirstName;
        }

        public string GetUserAvatar()
        {
            return db.Users.Find(UserId).AvatarPath;
        }

    }
}