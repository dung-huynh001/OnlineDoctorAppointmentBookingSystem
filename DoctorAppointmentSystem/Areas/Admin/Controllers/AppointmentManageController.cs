using DoctorAppointmentSystem.Areas.Admin.Models;
using DoctorAppointmentSystem.Areas.Admin.Models.AdminUser;
using DoctorAppointmentSystem.Areas.Admin.Models.AppointmentManage;
using DoctorAppointmentSystem.Areas.Admin.Models.DataTableModel;
using DoctorAppointmentSystem.Areas.Admin.Models.Validation;
using DoctorAppointmentSystem.Authorization;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Menu;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Linq.SqlClient;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Admin.Controllers
{
    [AppAuthorize("Admin")]
    public class AppointmentManageController : Controller
    {

        private readonly DBContext _dbContext;
        private readonly ISystemParamService _sysParam;
        private readonly IMapperService _mapper;
        private readonly IloggerService _logger;
        private readonly AppointmentIO _appointment;

        public AppointmentManageController(DBContext dbContext, ISystemParamService sysParam, IMapperService mapper, IloggerService logger, AppointmentIO appointment)
        {
            _dbContext = dbContext;
            _sysParam = sysParam;
            _mapper = mapper;
            _logger = logger;
            _appointment = appointment;
        }


        // GET: Admin/Appointment
       
        public ActionResult Index()
        {
            AdminMenu menu = new AdminMenu();
            ViewBag.menu = menu.RenderMenu("Appointments");
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            var user = GetCurrentUser();
            ViewBag.Name = user != null ? user.USERNAME : "";

            ViewBag.consultantType = GenerateByGroup("ConsultantType");
            ViewBag.modeOfConsultant = GenerateByGroup("ModeOfConsultant");

            return View();
        }

        [NonAction]
        private List<SYSTEM_PARA> GenerateByGroup(string groupID)
        {
             return _sysParam.GetAllParam().Where(l => l.GROUPID == groupID).ToList();
          
        }
       

        //performance improvement experiment ((:
        public JsonResult LoadAppointmentData(JqueryDatatableParam param) 
        {

            var apmts = _appointment.GetAllAppointment();
            IEnumerable<ApponitmentViewModel> Apointments;
            try
            {
                Apointments = apmts.Select(dt => _mapper.GetMapper().Map<APPOINTMENT, ApponitmentViewModel>(dt)).ToList();
            }
            catch
            {
                string sEventCatg = "ADMIN PORTAL";
                string sEventMsg = "Exception: Failed to mapping APPOINTMENT to ApponitmentViewModel";
                string sEventSrc = nameof(LoadAppointmentData);
                string sEventType = "C";
                string sInsBy = GetCurrentUser().USERNAME;
                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);

                Apointments = new List<ApponitmentViewModel>();
            }

            if (!string.IsNullOrEmpty(param.sSearch)) //search
            {
                Apointments = Apointments.Where(x =>  x.PATIENTNAME.ToLower().Contains(param.sSearch.ToLower())
                                                      || x.DOCTORNAME.ToLower().Contains(param.sSearch.ToLower())
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
                Func<ApponitmentViewModel, string> orderingFunction = e =>
                                                           sortColumnIndex == 2 ? e.PATIENTNAME :
                                                           sortColumnIndex == 3 ? e.DOCTORNAME :
                                                           sortColumnIndex == 4 ? e.APPOINTMENTDATE :
                                                           sortColumnIndex == 5 ? e.DATEOFCONSUITATION :
                                                            sortColumnIndex == 12 ? e.APPOIMENTSTATUS :
                                                           e.UPDATEDBY
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


        public JsonResult DeleteAppointment(int AppointmentId)
        {
            var user = GetCurrentUser();
            if (user == null)
            {
                return Json(new {error =1, msg =" " });
            }
            ValidationResult result = _appointment.DeleteAppointment(AppointmentId,user.USERNAME);
            if (result.Success)
            {
                return Json(new { error = 0, msg = "ok" });
            }
            else
            {
                return Json(new {error =1 , msg = result.ErrorMessage});
            }
        }

        public JsonResult AppointViewDetail(int appointmentID)
        {
            var apm = _appointment.getAppointmentInfo(appointmentID, GetCurrentUser().USERNAME);

            return Json(new { error = 1, msg = "ok", data= apm }, JsonRequestBehavior.AllowGet);
        }

        public CompletedApptViewDetailsModel getAppointmentInfo(int appointmentID, string username)
        {
            CompletedApptViewDetailsModel avm = new CompletedApptViewDetailsModel();
            try
            {
                var apm = _dbContext.APPOINTMENT.Where(a => a.APPOINTMENTID == appointmentID)
                                                .Include("PATIENT")
                                                .Include("SCHEDULE")
                                                .Include("SCHEDULE.DOCTOR")
                                                .FirstOrDefault();

                avm = _mapper.GetMapper().Map<APPOINTMENT, CompletedApptViewDetailsModel>(apm);
            }
            catch
            {
                string sEventCatg = "ADMIN PORTAL";
                string sEventMsg = "Exception: Failed to load detail appointment";
                string sEventSrc = nameof(getAppointmentInfo);
                string sEventType = "L";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
            }
            return avm;

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