using DoctorAppointmentSystem.Areas.Doctor.Models.Appointments;
using DoctorAppointmentSystem.Areas.Doctor.Models.Home;
using DoctorAppointmentSystem.Authorization;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Menu;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Doctor.Controllers
{
    [AppAuthorize("Doctor")]
    public class HomeController : Controller
    {
        private readonly DBContext _dbContext;
        private readonly ISystemParamService _sysParam;
        private readonly calendarIO _calendarIO;
        private readonly AppointmentsDoctorIO _appointments;

        public HomeController(DBContext dbContext, ISystemParamService sysParam, calendarIO calendarIO, AppointmentsDoctorIO appointments)
        {
            _dbContext = dbContext;
            _sysParam = sysParam;
            _calendarIO = calendarIO;
            _appointments = appointments;
        }



        // GET: Doctor/Home

        public async Task<ActionResult> Index()
        {

            var currentUser = GetCurrentUser();

            ViewBag.menu = RenderMenu.RenderDoctorMenu("Dashboard");
            ViewBag.name = currentUser.DOCTOR.FirstOrDefault().DOCTORNAME?? "";
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            ViewBag.daynow = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            var CurrentDoctorId = GetCurrentDoctorID(currentUser);
         
            ViewBag.newBooking = await _appointments.CountApptPendingAsync(CurrentDoctorId);
            ViewBag.todayAppt = await _appointments.CountApptToDayAsync(CurrentDoctorId);
            ViewBag.apptConfirm = await _appointments.CountApptComfirmedAsync(CurrentDoctorId);
            ViewBag.apptCompleted = await _appointments.CountApptCompletedAsync(CurrentDoctorId);

            return View();

        }


        [HttpPost]
        public JsonResult GetDoctorSchedule(DateTime start, DateTime end ,string status) //action for calendar
        {
            var currentUser = GetCurrentUser();
            int doctorId = currentUser.DOCTOR !=null? currentUser.DOCTOR.FirstOrDefault().DOCTORID : 0;

            var events = _calendarIO.GetDoctorSchedule(start, end, doctorId,status);

            return Json(events, JsonRequestBehavior.AllowGet);
        }




        [NonAction]
        private USER GetCurrentUser()
        {
            if (User.Identity.IsAuthenticated == true)
            {
                var userName = User.Identity.Name;
                if (userName != null)
                {
                    var currentUser = _dbContext.USER.Where(u => u.USERNAME == userName && u.DELETEDFLAG == false).Include("DOCTOR").FirstOrDefault();
                    return currentUser;
                }
            }
            return new USER();
        }
        [NonAction]
        private int GetCurrentDoctorID(USER currentUser )
        {

             
            int doctorId = currentUser.DOCTOR != null ? currentUser.DOCTOR.FirstOrDefault().DOCTORID : 0;

            return doctorId;
        }
    }
}