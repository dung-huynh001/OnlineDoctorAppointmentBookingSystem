using DoctorAppointmentSystem.Areas.Admin.Models;
using DoctorAppointmentSystem.Areas.Admin.Models.AdminUser;
using DoctorAppointmentSystem.Areas.Admin.Models.DataTableModel;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorSchedule;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorScheduleManage;
using DoctorAppointmentSystem.Areas.Admin.Models.Validation;
using DoctorAppointmentSystem.Authorization;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Menu;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services.ServiceInterface;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Unity;

namespace DoctorAppointmentSystem.Areas.Admin.Controllers
{
    [AppAuthorize("Admin")]
    public class DoctorScheduleManageController : Controller
    {
        private readonly DBContext _dbContext;
        private readonly ISystemParamService _sysParam;
        private readonly IMapperService _mapper;
        private readonly IloggerService _logger;
        private readonly DoctorScheduleIO _doctorSchedule;

        public DoctorScheduleManageController(DBContext dbContext, ISystemParamService sysParam, IMapperService mapper, IloggerService logger, DoctorScheduleIO doctorSchedule)
        {
            _dbContext = dbContext;
            _sysParam = sysParam;
            _mapper = mapper;
            _logger = logger;
            _doctorSchedule = doctorSchedule;
        }



        // GET: Admin/DoctorSchedule
        public ActionResult Index()
        {
            AdminMenu menu = new AdminMenu();
            ViewBag.menu = menu.RenderMenu("Doctor Schedules");
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            var user = GetCurrentUser();
            ViewBag.Name = user != null ? user.USERNAME : "";

            var listDoctor = _dbContext.DOCTOR.Where(d => d.DELETEDFLAG == false).ToList();
            var ListConsultantTime = _sysParam.GetAllParam().Where(p => p.GROUPID == "ConsultantTime").ToList();
            ViewBag.doctor = new SelectList(listDoctor, "DOCTORID", "DOCTORNAME");
            ViewBag.consultanttime = new SelectList(ListConsultantTime, "ID", "NOTE");
            return View();
        }

        //load data doctor schedule to jquery datatable
        public ActionResult LoadDoctorScheduleData(JqueryDatatableParam param)
        {
            var schedule = _doctorSchedule.GetAllSchedule();
            IEnumerable<DoctorScheduleViewModel> Schedules = new List<DoctorScheduleViewModel>();
            try {
                 Schedules = schedule.Select(dt => _mapper.GetMapper().Map<SCHEDULE, DoctorScheduleViewModel>(dt)).ToList();
            }
            catch
            {
                
                string sEventCatg = "ADMIN PORTAL";
                string sEventMsg = "Exception: Failed to mapping data SCHEDULE to DoctorScheduleViewModel";
                string sEventSrc = nameof(LoadDoctorScheduleData);
                string sEventType = "C";
                string sInsBy = GetCurrentUser().USERNAME;
                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
               
            }

            if (!string.IsNullOrEmpty(param.sSearch)) //search
            {
                Schedules = Schedules.Where(x => x.DOCTORID.ToString().ToLower().Contains(param.sSearch.ToLower())
                                              || x.DOCTORNAME.ToLower().Contains(param.sSearch.ToLower())
                                              || x.WORKINGDAY.ToLower().Contains(param.sSearch.ToLower())
                                               ).ToList();
            }

            var sortColumnIndex = param.iSortCol_0;
            var sortDirection = param.sSortDir_0;

            if (sortColumnIndex == 1)
            {
                Schedules = (sortDirection == "asc" ? Schedules.OrderBy(c => c.SCHEDULEID) : Schedules.OrderByDescending(c => c.SCHEDULEID));
            }
            else if (sortColumnIndex == 2)
            {
                Schedules = (sortDirection == "asc" ? Schedules.OrderBy(c => c.DOCTORID) : Schedules.OrderByDescending(c => c.DOCTORID));
            }
            else if (sortColumnIndex == 3)
            {
                Schedules = sortDirection == "asc" ? Schedules.OrderBy(c => c.DOCTORNAME) : Schedules.OrderByDescending(c => c.DOCTORNAME);
            }
            else if (sortColumnIndex == 4)
            {
                Schedules = sortDirection == "asc" ? Schedules.OrderBy(c => c.WORKINGDAY) : Schedules.OrderByDescending(c => c.WORKINGDAY);
            }
            else if (sortColumnIndex == 5)
            {
                Schedules = sortDirection == "asc" ? Schedules.OrderBy(c => c.SHIFTTIME) : Schedules.OrderByDescending(c => c.SHIFTTIME);
            }
            else if (sortColumnIndex == 6)
            {
                Schedules = sortDirection == "asc" ? Schedules.OrderBy(c => c.BREAKTIME) : Schedules.OrderByDescending(c => c.BREAKTIME);
            }
            else if (sortColumnIndex == 7)
            {
                Schedules = sortDirection == "asc" ? Schedules.OrderBy(c => c.CONSULTANTTIME) : Schedules.OrderByDescending(c => c.CONSULTANTTIME);
            }
            else if (sortColumnIndex == 8)
            {
                Schedules = sortDirection == "asc" ? Schedules.OrderBy(c => c.APPOINTMENTNUM) : Schedules.OrderByDescending(c => c.APPOINTMENTNUM);
            }
            else if (sortColumnIndex == 9)
            {
                Schedules = sortDirection == "asc" ? Schedules.OrderBy(c => c.CREATEDBY) : Schedules.OrderByDescending(c => c.APPOINTMENTNUM);
            }
            else if(sortColumnIndex == 10)
            {
                Schedules = sortDirection == "asc" ? Schedules.OrderBy(c => c.CREATEDDATE) : Schedules.OrderByDescending(c => c.APPOINTMENTNUM);
            }
            else if(sortColumnIndex == 12)
            {
                Schedules = sortDirection == "asc" ? Schedules.OrderBy(c => c.UPDATEDBY) : Schedules.OrderByDescending(c => c.APPOINTMENTNUM);
            }
            else 
            {
                Schedules = sortDirection == "asc" ? Schedules.OrderBy(c => c.UPDATEDDATE) : Schedules.OrderByDescending(c => c.APPOINTMENTNUM);
            }

             var displayResult = Schedules.Skip(param.iDisplayStart)
                .Take(param.iDisplayLength).ToList();
            var totalRecords = Schedules.Count();

            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = displayResult
            }, JsonRequestBehavior.AllowGet);

        }

        //create doctor schedule
        public JsonResult CreateDoctorSchedule(DoctorScheduleCreateModel model)
        {

            USER CurentUser = GetCurrentUser();
            if (CurentUser == null)
            {
                return Json(new { error = 1, msg = "Can't find current user !" });
            }
            SCHEDULE schedule = null;
            try
            {
                schedule = _mapper.GetMapper().Map<DoctorScheduleCreateModel, SCHEDULE>(model);
            }
            catch
            {
                string sEventCatg = "ADMIN PORTAL";
                string sEventMsg = "Exception: Failed to mapping data DoctorScheduleCreateModel to SCHEDULEl";
                string sEventSrc = nameof(CreateDoctorSchedule);
                string sEventType = "C";
                string sInsBy = GetCurrentUser().USERNAME;
                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
               
                return Json(new { error = 1, msg = "Create doctor schedule is failed!" });
            }
           
            ValidationResult result = _doctorSchedule.CreateSchedule(schedule,CurentUser.USERNAME);
            if (!result.Success) { return Json(new { error = 1, msg = result.ErrorMessage }); }

            return Json(new {error = 0, msg = "ok"});
        }

        //load Schedule data 
        public JsonResult LoadDoctorScheduleInfo(int scheduleid)
        {
            var schedule = _doctorSchedule.GetScheduleById(scheduleid);
            DoctorScheduleViewEditModel scheduleView = null;
            try
            {
                scheduleView = _mapper.GetMapper().Map<SCHEDULE, DoctorScheduleViewEditModel>(schedule);
            }
            catch
            {
                string sEventCatg = "ADMIN PORTAL";
                string sEventMsg = "Exception: Failed to SCHEDULE to DoctorScheduleViewEditModel";
                string sEventSrc = nameof(LoadDoctorScheduleInfo);
                string sEventType = "C";
                string sInsBy = GetCurrentUser().USERNAME ;
                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                
                return Json(new { error = 1, msg = "Get schedule info is failed!" });
            }
         
            return Json(new {error = 0, msg ="ok", schedule = scheduleView });
        }

        //update doctor schedule
        public JsonResult UpdateDoctorSchedule(DoctorScheduleEditModel model)
        {

            USER CurentUser = GetCurrentUser();
            if (CurentUser == null)
            {
                return Json(new { error = 1, msg = "Can't find current user !" });
            }
            SCHEDULE schedule = null;

            try
            {
                schedule = _mapper.GetMapper().Map<DoctorScheduleEditModel,SCHEDULE>(model) ;
            }
            catch
            {
                string sEventCatg = "ADMIN PORTAL";
                string sEventMsg = "Exception: Failed to DoctorScheduleEditModel to SCHEDULE";
                string sEventSrc = nameof(UpdateDoctorSchedule);
                string sEventType = "C";
                string sInsBy = GetCurrentUser().USERNAME;
                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return Json(new { error = 1, msg = "Update doctor schedule is failed!" });
            }
            ValidationResult result = _doctorSchedule.UpdateSchedule(schedule, CurentUser.USERNAME);
            if(!result.Success) { return Json(new { error = 1, msg = result.ErrorMessage }); }
            return Json(new { error = 0, msg = "ok" });
        }

        //delete schedule by id 
        public JsonResult DeleteSchedule(int scheduleid)
        {
            USER CurentUser = GetCurrentUser();
            if (CurentUser == null)
            {
                return Json(new { error = 1, msg = "Can't find current user !" });
            }
            ValidationResult result = _doctorSchedule.DeleteSchedule(scheduleid, "");
            if (!result.Success) { return Json(new { error = 1, msg = result.ErrorMessage }); }
            return Json(new { error = 0, msg = "ok", });
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