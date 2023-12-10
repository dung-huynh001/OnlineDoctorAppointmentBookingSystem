using DoctorAppointmentSystem.Authorization;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Menu;
using DoctorAppointmentSystem.Models.Appointment;
using DoctorAppointmentSystem.Models.Appointment.MakeAppointment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Controllers
{
    [AppAuthorize("Patient")]
    public class ScheduleOfDoctorsController : Controller
    {
        private readonly AppointmentIO appointmentIO;

        public ScheduleOfDoctorsController()
        {
            appointmentIO = new AppointmentIO();
        }

        // GET: ScheduleOfDoctors
        public ActionResult Index()
        {
            ViewBag.consultantType = SystemParaHelper.GenerateByGroup("consultantType");
            ViewBag.modeOfConsultant = SystemParaHelper.GenerateByGroup("modeOfConsultant");
            ViewBag.menu = RenderMenu.RenderPatientMenu("Schedule of Doctors");
            ViewBag.name = GetInfo.GetFullName(User.Identity.Name);
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            return View();
        }

        public ActionResult LoadAllSchedule()
        {
            var schedules = appointmentIO.LoadScheduleList();
            return Json(new { data = schedules }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadDoctorInfo(string selectedDoctorID, string selectedScheduleID)
        {
            ScheduleViewModel data = appointmentIO.LoadDoctorInfo(Convert.ToInt32(selectedDoctorID), User.Identity.Name, Convert.ToInt32(selectedScheduleID));
            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }
    }
}