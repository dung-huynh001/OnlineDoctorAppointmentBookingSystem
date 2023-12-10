using DoctorAppointmentSystem.Authorization;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Menu;
using DoctorAppointmentSystem.Models.Account;
using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Controllers
{
    [AppAuthorize("Patient")]
    public class HomeController : Controller
    {
        [Obsolete]
        public ActionResult Index()
        {
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    string username = User.Identity.Name;
                    int userID = dbContext.USER.Where(u => u.USERNAME.Equals(username)).Select(u => u.USERID).FirstOrDefault();

                    if (userID != 0)
                    {
                        int patientID = dbContext.PATIENT.Where(p => p.USERID.Equals(userID)).Select(p => p.PATIENTID).FirstOrDefault();

                        if (patientID != 0)
                        {
                            DateTime today = DateTime.Now.Date;
                            var appointments = dbContext.APPOINTMENT.Where(a => a.PATIENTID.Equals(patientID));

                            ViewBag.allDoctors = dbContext.DOCTOR.Count();
                            ViewBag.allBooking = appointments.Count();
                            ViewBag.newBooking = appointments.Count(a => a.APPOIMENTSTATUS.Equals("Pending") || a.APPOIMENTSTATUS.Equals("Confirm"));
                            ViewBag.todaySessions = appointments.Count(a => EntityFunctions.TruncateTime(a.APPOINTMENTDATE).Value.Equals(today));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string sEventCatg = GetInfo.GetUserType(GetInfo.Username) + " PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "Home Page";
                string sEventType = "S";
                string sInsBy = GetInfo.Username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
            }

            ViewBag.consultantType = SystemParaHelper.GenerateByGroup("consultantType");
            ViewBag.modeOfConsultant = SystemParaHelper.GenerateByGroup("modeOfConsultant");
            ViewBag.menu = RenderMenu.RenderPatientMenu("Home");
            ViewBag.name = GetInfo.GetFullName(User.Identity.Name);
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);

            return View();
        }


        [AllowAnonymous]
        public ActionResult Intro()
        {
            RegisterIO registerIO = new RegisterIO();
            
            if (User.Identity.IsAuthenticated)
            {   
                var user = registerIO.GetUserByUserName(User.Identity.Name);
                if (user != null)
                {
                    if(user.USERTYPE == "Patient")
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (user.USERTYPE.Equals("Doctor"))
                    {
                        return RedirectToAction("Index", "Home", new { area = "Doctor" });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Manage", new { area = "Admin" });

                    }
                }
            }
            
                return View();
        }
    }
}