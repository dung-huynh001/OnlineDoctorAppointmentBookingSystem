using DoctorAppointmentSystem.Areas.Admin.Models;
using DoctorAppointmentSystem.Authorization;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Menu;
using DoctorAppointmentSystem.Models.DB;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Admin.Controllers
{
    [AppAuthorize("Admin")]
    public class ManageController : Controller
    {
        private readonly DBContext _dbContext;
        private readonly DashboardIO _dashboardIO;

        public ManageController(DBContext dbContext, DashboardIO dashboardIO)
        {
            _dbContext = dbContext;
            _dashboardIO = dashboardIO;
        }


        // GET: Admin/Manage
        public ActionResult Index()
        {
            AdminMenu menu = new AdminMenu();
            ViewBag.menu = menu.RenderMenu("Dashboard");
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            var user = GetCurrentUser();
            ViewBag.Name = user != null ? user.USERNAME : "";

            ViewBag.DoctorCount = _dashboardIO.CountAllDoctor();
            ViewBag.PatientCount = _dashboardIO.CountAllPatient();
            ViewBag.TodayAppointment = _dashboardIO.CountAppointmentToDay();
            ViewBag.AppointmentTillDate = _dashboardIO.CountAllAppointment();


            return View();
        }
        [HttpGet]
        public JsonResult GetChartApm()
        {
            int[] apmData = _dashboardIO.GetArrayAppointmentOnWeek();
            int[] apmCompleted = _dashboardIO.GetArrayApptForStatusOnWeek("Completed");
            int[] apmConfirmed = _dashboardIO.GetArrayApptForStatusOnWeek("Confirm");
            int[] apmPending = _dashboardIO.GetArrayApptForStatusOnWeek("Pending");
            int[] apmCancelled = _dashboardIO.GetArrayApptForStatusOnWeek("Cancel");

            string[] dataLabel = _dashboardIO.GetArrayLabelOnWeek();

            return Json(new { 
                error = 0, 
                datachart = apmData, 
                datalabel = dataLabel , 
                completed = apmCompleted,
                confirmed = apmConfirmed,
                pending = apmPending,
                cancelled = apmCancelled,
            }, 
            JsonRequestBehavior.AllowGet);
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

            return new USER();
        }

    }
}