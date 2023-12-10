using DoctorAppointmentSystem.Areas.Doctor.Models.ComfirmedAppt;
using DoctorAppointmentSystem.Areas.Doctor.Models.ViewModels;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Menu;
using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Doctor.Controllers
{
    [Authorize]
    public class ConfirmedApptController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.consultantType = SystemParaHelper.GenerateByGroup("consultantType");
            ViewBag.modeOfConsultant = SystemParaHelper.GenerateByGroup("modeOfConsultant");
            ViewBag.menu = RenderMenu.RenderDoctorMenu("Appointments Confirmed");
            ViewBag.name = GetInfo.GetFullName(User.Identity.Name);
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            return View();
        }

        public ActionResult LoadData(string status)
        {
            string username = User.Identity.Name;
            try
            {
                int doctorID = GetInfo.GetDoctorID(username);

                if (doctorID == 0)
                {
                    throw new Exception("Doctor not found.");
                }

                List<AppointmentViewModel> data = new ConfirmedApptIO().GetConfirmedAppts(doctorID, status);

                return Json(new { success = true, data = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string sEventCatg = GetInfo.GetUserType(username).ToUpper();
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "LoadData";
                string sEventType = "S";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult AppointmentDetails(int id)
        {
            MemberDetailViewModel data = new ConfirmedApptIO().GetMemberDetails(id);

            ViewBag.consultantType = SystemParaHelper.GenerateByGroup("consultantType");
            ViewBag.modeOfConsultant = SystemParaHelper.GenerateByGroup("modeOfConsultant");
            ViewBag.menu = RenderMenu.RenderDoctorMenu("Appointments Confirmed");
            ViewBag.name = GetInfo.GetFullName(User.Identity.Name);
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            return View(data);
        }

        [HttpPost]
        public ActionResult ChangeAppointmentDate(int id, DateTime datetime)
        {
            bool success = false;
            try
            {
                if (id == 0 || datetime == null)
                {
                    throw new Exception();
                }
                string message;
                success = new ConfirmedApptIO().ChangeAppointmentDate(id, datetime, out message);
                return Json(new { success = success, message = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string username = GetInfo.Username;
                string sEventCatg = GetInfo.GetUserType(username);
                string sEventMsg = ex.Message;
                string sEventSrc = nameof(CancelAppointment);
                string sEventType = "U";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);

                return Json(new { success = success }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetFreqAndUnit()
        {
            var frequency = SystemParaHelper.GenerateByGroup("Frequency");
            var unit = SystemParaHelper.GenerateByGroup("Unit");
            return Json(new { frequency = frequency, unit = unit }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult LoadDoctorNotes(int id)
        {
            try
            {
                ConfirmedApptIO confirmedApptIO = new ConfirmedApptIO();
                var diagnosis = confirmedApptIO.GetDiagnosis(id);
                var medications = confirmedApptIO.GetMedication(id);


                return Json(new { success = true, diagnosis = diagnosis, medications = medications }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                string username = GetInfo.Username;
                string sEventCatg = GetInfo.GetUserType(username);
                string sEventMsg = ex.Message;
                string sEventSrc = nameof(LoadDoctorNotes);
                string sEventType = "S";
                string sInsBy = username;
                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);

                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult MarkAsCompleted(int id, List<MedicationViewModel> data, DiagnosisViewModel notes)
        {
            bool success = false;
            try
            {
                if(data == null || id == 0 || notes == null)
                {
                    throw new Exception();
                }
                else
                {
                    ConfirmedApptIO confirmedApptIO = new ConfirmedApptIO();
                    success = confirmedApptIO.MarkAsCompleted(id, data, notes);
                }
            }
            catch (Exception ex)
            {
                string username = GetInfo.Username;
                string sEventCatg = GetInfo.GetUserType(username).ToUpper() + "PORTAL";
                string sEventMsg = ex.Message;
                string sEventSrc = nameof(MarkAsCompleted);
                string sEventType = "U";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                
            }
            return Json(new { success = success }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult CancelAppointment(int id)
        {
            bool success = false;
            try
            {
                success = new ConfirmedApptIO().CancelAppointment(id);
                if (!success)
                {
                    throw new Exception();
                }
                return Json(new { success = success });
            }
            catch (Exception ex)
            {
                string username = GetInfo.Username;
                string sEventCatg = GetInfo.GetUserType(username);
                string sEventMsg = ex.Message;
                string sEventSrc = nameof(CancelAppointment);
                string sEventType = "U";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return Json(new { success = success });
            }
        }
    }
}