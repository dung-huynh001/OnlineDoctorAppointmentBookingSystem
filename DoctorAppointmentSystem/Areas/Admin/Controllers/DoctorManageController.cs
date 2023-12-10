using Antlr.Runtime;
using DoctorAppointmentSystem.Areas.Admin.Models;
using DoctorAppointmentSystem.Areas.Admin.Models.DataTableModel;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorManage;
using DoctorAppointmentSystem.Areas.Admin.Models.Validation;
using DoctorAppointmentSystem.Authorization;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Menu;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Admin.Controllers
{
    
    [AppAuthorize("Admin")]
    public class DoctorManageController : Controller
    {
        
        private readonly DBContext _dbContext;
        private readonly ISystemParamService _sysParam;
        private readonly IMapperService _mapper;
        private readonly DoctorIO _doctorIO;

        public DoctorManageController(DBContext dbContext, ISystemParamService sysParam, IMapperService mapper, DoctorIO doctorIO)
        {
            _dbContext = dbContext;
            _sysParam = sysParam;
            _mapper = mapper;
            _doctorIO = doctorIO;
        }



        // GET: Admin/DoctorManage

        public ActionResult Index()
        {
            AdminMenu menu = new AdminMenu();
            ViewBag.menu = menu.RenderMenu("Doctor Management");
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            var user = GetCurrentUser();
            ViewBag.Name = user != null ? user.USERNAME : "";

            var sysParam = _sysParam.GetAllParam();

            ViewBag.genders = ViewBag.genders = new SelectList(sysParam.Where(c => c.GROUPID.Equals("Gender") && c.DELETEDFLAG == false).ToList(), "ID", "PARAVAL");
            ViewBag.department = new SelectList( _dbContext.DEPARTMENT.ToList(), "DEPARTMENTID", "DEPARTMENTNAME");
            return View();
        }

        //load data doctors to jquery datatable
        public async  Task<ActionResult> LoadDoctorData( JqueryDatatableParam param)
        {
            var doctor = await _dbContext.DOCTOR.Where(d => d.DELETEDFLAG == false).Include("DEPARTMENT").Include("USER").ToListAsync();
            IEnumerable<DoctorViewModel> Doctors = doctor.Select(dt => _mapper.GetMapper().Map<DOCTOR, DoctorViewModel>(dt)).ToList();
            
            if (!string.IsNullOrEmpty(param.sSearch)) //search
            {
                Doctors = Doctors.Where(x => x.DOCTORNAME.ToLower().Contains(param.sSearch.ToLower())
                                              || x.DOCTORID.ToString().Contains(param.sSearch.ToLower())
                                              || x.DOCTORGENDER.ToLower().Contains(param.sSearch.ToLower())
                                              || x.DOCTORDATEOFBIRTH.ToLower().Contains(param.sSearch.ToLower())
                                              || x.DOCTORMOBILENO.ToString().Contains(param.sSearch.ToLower())
                                              || x.DOCTORADDRESS.ToLower().Contains(param.sSearch.ToLower())
                                               ).ToList();
            }

            var sortColumnIndex = param.iSortCol_0;// Convert.ToInt32(HttpContext.Request.QueryString["iSortCol_0"]);
            var sortDirection = param.sSortDir_0; // HttpContext.Request.QueryString["sSortDir_0"];

           
            if (sortColumnIndex == 1)
            {
                Doctors = sortDirection == "asc" ? Doctors.OrderBy(c => c.DOCTORID) : Doctors.OrderByDescending(c => c.DOCTORID);
            }
            else if (sortColumnIndex == 6)
            {
                Doctors = sortDirection == "asc" ? Doctors.OrderBy(c => c.DOCTORDATEOFBIRTH) : Doctors.OrderByDescending(c => c.DOCTORDATEOFBIRTH);
            }
            else if (sortColumnIndex == 15)
            {
                Doctors = sortDirection == "asc" ? Doctors.OrderBy(c => c.LOGINRETRYCOUNT) : Doctors.OrderByDescending(c => c.LOGINRETRYCOUNT);
            }
            else if (sortColumnIndex == 12)
            {
                Doctors = sortDirection == "asc" ? Doctors.OrderBy(c => c.WORKINGSTARTDATE) : Doctors.OrderByDescending(c => c.WORKINGSTARTDATE);
            }
            else if (sortColumnIndex == 13)
            {
                Doctors = sortDirection == "asc" ? Doctors.OrderBy(c => c.WORKINGENDDATE) : Doctors.OrderByDescending(c => c.WORKINGENDDATE);
            }
            else if (sortColumnIndex == 17)
            {
                Doctors = sortDirection == "asc" ? Doctors.OrderBy(c => c.CREATEDDATE) : Doctors.OrderByDescending(c => c.CREATEDDATE);
            }
            else if (sortColumnIndex == 19)
            {
                Doctors = sortDirection == "asc" ? Doctors.OrderBy(c => c.UPDATEDDATE) : Doctors.OrderByDescending(c => c.CREATEDDATE);
            }
            else
            {
                Func<DoctorViewModel, string> orderingFunction = e =>
                                                           sortColumnIndex == 2 ? e.DOCTORNAME :
                                                           sortColumnIndex == 3 ? e.USERNAME :
                                                           sortColumnIndex == 4 ? e.DEPARTMENT:
                                                           sortColumnIndex == 5 ? e.DOCTORGENDER :
                                                           sortColumnIndex == 8 ? e.DOCTORMOBILENO :
                                                           sortColumnIndex == 10 ? e.DOCTORADDRESS :
                                                           sortColumnIndex == 9 ? e.EMAIL :
                                                           sortColumnIndex == 11 ? e.SPECIALITY :
                                                           sortColumnIndex == 16 ? e.CREATEDBY :
                                                           e.UPDATEDBY;//20

                Doctors = sortDirection == "asc" ? Doctors.OrderBy(orderingFunction) : Doctors.OrderByDescending(orderingFunction);
                 
            }


            var displayResult = Doctors.Skip(param.iDisplayStart)
                .Take(param.iDisplayLength).ToList();
            var totalRecords = Doctors.Count();


            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = displayResult
            }, JsonRequestBehavior.AllowGet);

        }

        // Create doctor
        [HttpPost]
        public JsonResult CreateDoctor(DoctorCreateModel model)
        {
            USER CurentUser = GetCurrentUser();
            if (CurentUser == null)
            {
                return Json(new { error = 1, msg = "Can't find current user !" });
            }

            var result = _doctorIO.CreateDoctor(model, CurentUser.USERNAME);
            if (result.Success)
            {
                return Json(new { error = 0, msg = "ok" });
            }
            return Json(new { error = 1, msg = result.ErrorMessage });
        }


        [HttpPost]
        //load doctor data for update 
        public JsonResult LoadDoctorInfo(int DoctorId)
        {
            if (DoctorId == 0)
            {
                return Json(new { error = 1, msg = "Error! do not find doctor !" });
            }
            var doctor = _dbContext.DOCTOR.Where(d=>d.DOCTORID == DoctorId && d.DELETEDFLAG == false).Include("USER").FirstOrDefault();
            if (doctor == null)
            {
                return Json(new { error = 1, msg = "Error! do not find doctor!" });
            }

            DoctorViewEditModel dt = _mapper.GetMapper().Map<DOCTOR, DoctorViewEditModel>(doctor);

            return Json(new { error = 0, msg = "ok", doctor = dt });
        }

        [HttpPost]
        //update doctor
        public JsonResult UpdateDoctor(DoctorEditModel model)
        {
            USER CurentUser = GetCurrentUser();
            if (CurentUser == null)
            {
                return Json(new { error = 1, msg = "Can't find current user !" });
            }
            var result = _doctorIO.UpdateDoctor(model, CurentUser.USERNAME);
            if (result.Success)
            {
                return Json(new { error = 0, msg = "ok" });
            }
            return Json(new { error = 1, msg = result.ErrorMessage });

        }

        //delete doctor
        [HttpPost]
        public JsonResult DeleteDoctor(int DoctorId)
        {
            USER CurentUser = GetCurrentUser();
            if (CurentUser == null)
            {
                return Json(new { error = 1, msg = "Can't find current user !" });
            }
            if (DoctorId == 0)
            {
                return Json(new { error = 1, msg = "Error! do not delete doctor !" });
            }
            var doctor = _dbContext.DOCTOR.Where(d => d.DOCTORID == DoctorId && d.DELETEDFLAG == false).FirstOrDefault();
            if (doctor == null)
            {
                return Json(new { error = 1, msg = "Error! do not find doctor !" });
            }

            doctor.UPDATEDDATE = DateTime.Now;
            doctor.UPDATEDBY = CurentUser.USERNAME;
            doctor.DELETEDFLAG = true;
            doctor.USER.UPDATEDDATE = DateTime.Now;
            doctor.USER.DELETEDFLAG = true;
            doctor.USER.UPDATEDBY = CurentUser.USERNAME;
            
            _dbContext.DOCTOR.AddOrUpdate(doctor);
            _dbContext.USER.AddOrUpdate(doctor.USER);
            try {
                _dbContext.SaveChanges(); 
            }
            catch (Exception ex)
            {
                //write error log
                return Json(new { error = 1, msg = ex.ToString() });
            }
            return Json(new { error = 0, msg = "ok" });
        }


        [NonAction]
        private USER GetCurrentUser()
        {
            if(User.Identity.IsAuthenticated == true)
            {
                var userName = User.Identity.Name;
                if(userName != null)
                {
                    var currentUser = _dbContext.USER.Where(u => u.USERNAME == userName && u.DELETEDFLAG == false).FirstOrDefault();
                    return currentUser;
                }
            }
            return null;
        }

    }
}