using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Authorization
{
    public class AppAuthorizeAttribute : AuthorizeAttribute
    {

        private readonly string[] allowedroles;
        public AppAuthorizeAttribute(params string[] roles)
        {
            this.allowedroles = roles;
        }

        protected virtual string Username
        {
            get { return HttpContext.Current.User.Identity.Name; }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if(string.IsNullOrEmpty(Username)) return false;
            var userRole = GetRolesForUser(Username);

            foreach (var role in allowedroles)
            {
                if (role == userRole) return true;
            }
            return false;

        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            RedirectToRouteResult routeData = null;

            if (!HttpContext.Current.User.Identity.IsAuthenticated || string.IsNullOrEmpty(Username))
            {
                routeData = new RedirectToRouteResult
                    (new System.Web.Routing.RouteValueDictionary
                    (new
                    {
                        controller = "Account",
                        action = "Login",
                        area = ""
                    }
                    ));
            }
            else
            {
                routeData = new RedirectToRouteResult
                (new System.Web.Routing.RouteValueDictionary
                 (new
                 {
                     controller = "Error",
                     action = "AccessDenied",
                     area = ""
                 }
                 ));
            }

            filterContext.Result = routeData;

            //filterContext.Result = new JsonResult() { Data = new { error = 1, msg = $"AccessDenied!" } };

        }




        public string GetRolesForUser(string username)
        {
            using (DBContext _dbContext = new DBContext())
            {
                if (!HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    return null;
                }
                var user = _dbContext.USER.Where(u => u.USERNAME == username).Include("ROLE").FirstOrDefault();

                if (user != null)
                {
                    return (user.ROLE != null) ? user.ROLE.ROLENAME : string.Empty;
                }

                return string.Empty;

            }

        }


    }
}