
using DoctorAppointmentSystem.Areas.Admin.Models.DataTableModel;
using DoctorAppointmentSystem.Areas.Doctor.Models.Appointments;
using DoctorAppointmentSystem.Areas.Doctor.Models.ComfirmedAppt;
using DoctorAppointmentSystem.Areas.Doctor.Models.ViewModels;
using DoctorAppointmentSystem.Authorization;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Menu;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppointmentViewModel = DoctorAppointmentSystem.Areas.Doctor.Models.Appointments.AppointmentViewModel;

namespace DoctorAppointmentSystem.Areas.Doctor.Controllers
{
    [AppAuthorize("Doctor")]
    public class AppointmentsController : Controller
    {
        private readonly DBContext _dbContext;
        private readonly AppointmentsDoctorIO _appointments;
        private readonly IMapperService _mapper;

        public AppointmentsController(DBContext dbContext, AppointmentsDoctorIO appointments, IMapperService mapper)
        {
            _dbContext = dbContext;
            _appointments = appointments;
            _mapper = mapper;
        }



        // GET: Doctor/Appointments
        public ActionResult Index()
        {
            ViewBag.menu = RenderMenu.RenderDoctorMenu("Appointments");
            ViewBag.name = GetInfo.GetFullName(User.Identity.Name);
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            return View();
        }

        public JsonResult LoadAppointmentData(JqueryDatatableParam param)
        {
            var currentUser = GetCurrentUser();
            int doctorId = 0;
            if (currentUser != null)
            {
                doctorId = currentUser.DOCTOR.Count > 0 ? currentUser.DOCTOR.FirstOrDefault().DOCTORID : 0;
            }
           
            var apmts = _appointments.GetAllPendingAppt(doctorId);
            IEnumerable<AppointmentViewModel> Apointments;
            try
            {
                Apointments = apmts.Select(dt => _mapper.GetMapper().Map<APPOINTMENT, AppointmentViewModel>(dt)).ToList();
            }
            catch
            {
                string sEventCatg = "DOCTOR PORTAL";
                string sEventMsg = "Exception: Failed to mapping APPOINTMENT to AppointmentViewModel";
                string sEventSrc = nameof(LoadAppointmentData);
                string sEventType = "C";
                string sInsBy = GetCurrentUser().USERNAME;
                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                Apointments = new List<AppointmentViewModel>();
            }

            if (!string.IsNullOrEmpty(param.sSearch)) //search
            {
                Apointments = Apointments.Where(x => x.PATIENID.ToString().ToLower().Contains(param.sSearch.ToLower())
                                              || x.PATIENTNAME.ToLower().Contains(param.sSearch.ToLower())
                                              || x.APPOINTMENTID.ToString().ToLower().Contains(param.sSearch.ToLower())
                                              || x.DATEOFCONSULTANT.ToString().ToLower().Contains(param.sSearch.ToLower())
                                               ).ToList();
            }
            var sortColumnIndex = param.iSortCol_0;
            var sortDirection = param.sSortDir_0;

            if (sortColumnIndex == 1)
            {
                Apointments = (sortDirection == "asc" ? Apointments.OrderBy(c => c.APPOINTMENTID) : Apointments.OrderByDescending(c => c.APPOINTMENTID));
            }
            else
            {
                Func<AppointmentViewModel, string> orderingFunction = e =>
                                                           sortColumnIndex == 2 ? e.PATIENTNAME :
                                                           sortColumnIndex == 3 ? e.DATEOFCONSULTANT :
                                                           sortColumnIndex == 4 ? e.DATEOFCONSULTANTTIME :
                                                           sortColumnIndex == 5 ? e.DATEOFCONSULTANTDAY :
                                                           sortColumnIndex == 6 ? e.APPOIMENTSTATUS : e.CONSULTANTTIME 
                                                           ;

                Apointments = (sortDirection == "asc" ? Apointments.OrderBy(orderingFunction) : Apointments.OrderByDescending(orderingFunction));

            }

            var displayResult = Apointments.Skip(param.iDisplayStart)
                .Take(param.iDisplayLength).ToList();
            var totalRecords = Apointments.Count();

            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = displayResult
            }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult MemberDetails(int id)
        {
            MemberDetailViewModel data = new ConfirmedApptIO().GetMemberDetails(id);

            ViewBag.consultantType = SystemParaHelper.GenerateByGroup("consultantType");
            ViewBag.modeOfConsultant = SystemParaHelper.GenerateByGroup("modeOfConsultant");
            ViewBag.menu = RenderMenu.RenderDoctorMenu("Appointments");
            ViewBag.name = GetInfo.GetFullName(User.Identity.Name);
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            return View(data);
        }

        [HttpPost]
        public ActionResult ConfirmAppointment(int id)
        {
            bool success = false;
            try
            {
                success = _appointments.ConfirmAppointment(id);
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

        [HttpPost]
        public ActionResult CancelAppointment(int id)
        {
            bool success = false;
            try
            {
                success = _appointments.CancelAppointment(id);
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
            return null;
        }

    }
}