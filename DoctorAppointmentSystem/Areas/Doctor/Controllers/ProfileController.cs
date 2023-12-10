using DoctorAppointmentSystem.Authorization;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Menu;
using DoctorAppointmentSystem.Models.Account;
using DoctorAppointmentSystem.Models.Account.ChangePassword;
using DoctorAppointmentSystem.Models.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Doctor.Controllers
{
    [AppAuthorize("Doctor")]
    public class ProfileController : Controller
    {
        private readonly ProfileIO profileIO;
        public ProfileController()
        {
            profileIO = new ProfileIO();
        }
        // GET: Doctor/Profile

        public ActionResult Index()
        {
            var genders = SystemParaHelper.GenerateGender();
            ViewBag.genders = new SelectList(genders, "id", "paraval");
            ViewBag.backUrl = "/Doctor/Home";

            return View();
        }


        public ActionResult ChangePassword()
        {
            ViewBag.menu = RenderMenu.RenderDoctorMenu("");
            ViewBag.name = GetInfo.GetFullName(User.Identity.Name);
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            ViewBag.questions = new ChangePasswordIO().GetAuthQuestions(User.Identity.Name);
            return View();
        }


        [HttpPost]
        public ActionResult ChangePasswordAPI(ChangePasswordViewModel data)
        {
            try
            {
                bool success = new ChangePasswordIO().ChangePassword(data);
                if (success)
                {
                    return Json(new { success = true, message = "Change password successfully." }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    throw new Exception("User failed to change password.");
                }
            }
            catch (Exception ex)
            {
                string sEventCatg = "PATIENT PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "ChangePasswordAPI";
                string sEventType = "U";
                string sInsBy = GetInfo.Username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);

                return Json(new { success = false, message = "Password incorrect! Please check again" }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}