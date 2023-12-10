using DoctorAppointmentSystem.Authorization;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Controllers
{
    [AppAuthorize("Patient","Doctor")]
    public class ProfileController : Controller
    {
        private readonly ProfileIO profileIO;

        public ProfileController()
        {
            profileIO = new ProfileIO();
        }
        // GET: Profile
        public ActionResult Index()
        {
            var genders = SystemParaHelper.GenerateGender();
            ViewBag.genders = new SelectList(genders, "id", "paraval");
            ViewBag.backUrl = "/Home/Index";
            return View();
        }

        public ActionResult ViewProfile()
        {
            ProfileViewModel profile = profileIO.GetProfile(User.Identity.Name);
            return Json(new { data = profile }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Edit(ProfileViewModel profileData)
        {
            string username = User.Identity.Name;
            string message = "";
            if (profileIO.VerifiedData(profileData, username, out message))
            {
                profileIO.UpdateProfile(profileData, username);
            }
            return Json(new { success = true, message});
        }
    }
}