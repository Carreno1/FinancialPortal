using Carreno_Financial_Portal.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Carreno_Financial_Portal.Extensions
{
    public static class HttpContextBaseExtension
    {
        public static async Task RefreshAuthentication(this HttpContextBase context, ApplicationUser user)
        {
            //This line performs the programmatic sign out
            context.GetOwinContext().Authentication.SignOut();

            //Now we sign back in and any new role will be recognized
            await context.GetOwinContext().Get<ApplicationSignInManager>().SignInAsync(user, isPersistent: false, rememberBrowser: false);

        }
    }
}