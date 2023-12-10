using DoctorAppointmentSystem.Areas.Admin.Models.AdminProfile;
using DoctorAppointmentSystem.Authorization;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Models.Profile;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Admin.Controllers
{
    [AppAuthorize("Admin")]
    public class AdminProfileController : Controller
    {
        private readonly DBContext _dbContext;

        public AdminProfileController(DBContext dbContext)
        {
            _dbContext = dbContext;
        }


        // GET: Admin/AdminProfile
        [OutputCache(Duration = 20, VaryByParam = "none")]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult ViewProfile()
        {
            USER user = GetCurrentUser();
            if (user == null) { return Json(new { data = new AdminProfileViewModel() }); }
           
            return Json(new { data = new AdminProfileViewModel {
                                                                    fullName = user.USERNAME , 
                                                                    email = user.EMAIL,
                                                                    profilePicture = user.AVATARURL
                                                                }}, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        private USER GetCurrentUser()
        {
            if (User.Identity.IsAuthenticated == true)
            {
                var userName = User.Identity.Name;
                if (userName != null)
                {
                    var currentUser = _dbContext.USER.Where(u => u.USERNAME == userName && u.DELETEDFLAG == false).FirstOrDefault();
                    return currentUser;
                }
            }

            return null;
        }
    }
}