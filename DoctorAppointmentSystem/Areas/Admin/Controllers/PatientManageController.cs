using DoctorAppointmentSystem.Areas.Admin.Models.DataTableModel;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorManage;
using DoctorAppointmentSystem.Areas.Admin.Models.PatientManage;
using DoctorAppointmentSystem.Areas.Admin.Models.Validation;
using DoctorAppointmentSystem.Authorization;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Menu;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services;
using DoctorAppointmentSystem.Services.ServiceInterface;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Admin.Controllers
{
    [AppAuthorize("Admin")]
    public class PatientManageController : Controller
    {
        private readonly DBContext _dbContext;
        private readonly IMapperService _mapper;
        private readonly ISystemParamService _sysParam;

        public PatientManageController(DBContext dbContext, IMapperService mapper, ISystemParamService sysParam)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _sysParam = sysParam;
        }

        // GET: Admin/Partient
        public ActionResult Index()
        {
            AdminMenu menu = new AdminMenu();
            ViewBag.menu = menu.RenderMenu("Patient Management");
            ViewBag.avatar = GetInfo.GetImgPath(User.Identity.Name);
            var user = GetCurrentUser();
            ViewBag.Name = user != null ? user.USERNAME : "";
            var sysParam = _sysParam.GetAllParam();
            ViewBag.genders = ViewBag.genders = new SelectList(sysParam.Where(c => c.GROUPID.Equals("Gender")).ToList(), "ID", "PARAVAL");
            

            return View();
        }

        public async Task<ActionResult> LoadPatientData(JqueryDatatableParam param)
        {
            
            var patients = await _dbContext.PATIENT.Where(d => d.DELETEDFLAG == false).Include("USER").ToListAsync();
            IEnumerable<PatientViewModel> Patients;
            try
            {
                Patients = patients.Select(dt => _mapper.GetMapper().Map<PATIENT, PatientViewModel>(dt)).ToList();
            }
            catch
            {
                string sEventCatg = "ADMIN PORTAL";
                string sEventMsg = "Exception: Failed to mapping PATIENT to PatientViewModel";
                string sEventSrc = nameof(LoadPatientData);
                string sEventType = "C";
                string sInsBy = GetCurrentUser().USERNAME;
                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                Patients = new List<PatientViewModel>();
            }

            if (!string.IsNullOrEmpty(param.sSearch)) //search
            {
                Patients = Patients.Where(x => x.PATIENTNAME.ToLower().Contains(param.sSearch.ToLower())
                                              || x.PATIENTID.ToString().Contains(param.sSearch.ToLower())
                                              || x.PATIENTGENDER.ToLower().Contains(param.sSearch.ToLower())
                                              || x.PATIENTDATEOFBIRTH.ToLower().Contains(param.sSearch.ToLower())
                                              || x.PATIENTMOBILENO.ToString().Contains(param.sSearch.ToLower())
                                              || x.PATIENTADDRESS.ToLower().Contains(param.sSearch.ToLower())
                                               ).ToList();
            }

            var sortColumnIndex = param.iSortCol_0;// Convert.ToInt32(HttpContext.Request.QueryString["iSortCol_0"]);
            var sortDirection = param.sSortDir_0; // HttpContext.Request.QueryString["sSortDir_0"];



            if (sortColumnIndex == 1)
            {
                Patients = sortDirection == "asc" ? Patients.OrderBy(c => c.PATIENTID) : Patients.OrderByDescending(c => c.PATIENTID);
            }
            else if (sortColumnIndex == 5)
            {
                Patients = sortDirection == "asc" ? Patients.OrderBy(c => c.PATIENTDATEOFBIRTH) : Patients.OrderByDescending(c => c.PATIENTDATEOFBIRTH);
            }
            else if (sortColumnIndex == 11)
            {
                Patients = sortDirection == "asc" ? Patients.OrderBy(c => c.LOGINRETRYCOUNT) : Patients.OrderByDescending(c => c.LOGINRETRYCOUNT);
            }
            else if (sortColumnIndex == 10)
            {
                Patients = sortDirection == "asc" ? Patients.OrderBy(c => c.LOGINLOCKDATE) : Patients.OrderByDescending(c => c.LOGINLOCKDATE);
            }
            else if (sortColumnIndex == 13)
            {
                Patients = sortDirection == "asc" ? Patients.OrderBy(c => c.CREATEDDATE) : Patients.OrderByDescending(c => c.CREATEDDATE);
            }
            else if (sortColumnIndex == 15)
            {
                Patients = sortDirection == "asc" ? Patients.OrderBy(c => c.UPDATEDDATE) : Patients.OrderByDescending(c => c.CREATEDDATE);
            }
            else
            {
                Func<PatientViewModel, string> orderingFunction = e =>
                                                           sortColumnIndex == 2 ? e.PATIENTNAME :
                                                           sortColumnIndex == 3 ? e.USERNAME :
                                                           sortColumnIndex == 4 ? e.PATIENTGENDER :
                                                           sortColumnIndex == 6 ? e.PATIENTNATIONALID :
                                                           sortColumnIndex == 7 ? e.PATIENTMOBILENO :
                                                           sortColumnIndex == 8 ? e.EMAIL :
                                                           sortColumnIndex == 9 ? e.PATIENTADDRESS :
                                                           sortColumnIndex == 12 ? e.CREATEDBY :
                                                           e.UPDATEDBY;//11

                Patients = sortDirection == "asc" ? Patients.OrderBy(orderingFunction) : Patients.OrderByDescending(orderingFunction);
                 
            }


            var displayResult = Patients.Skip(param.iDisplayStart)
                .Take(param.iDisplayLength).ToList();
            var totalRecords = Patients.Count();


            return Json(new
            {
                param.sEcho,
                iTotalRecords = totalRecords,
                iTotalDisplayRecords = totalRecords,
                aaData = displayResult
            }, JsonRequestBehavior.AllowGet);

        }

        // Create patient
        [HttpPost]
        public JsonResult CreatePatient(PatientCreateModel model)
        {
            model.PATIENTNAME = model.PATIENTNAME != null ? model.PATIENTNAME.Trim() : model.PATIENTNAME;
            model.PATIENTNATIONALID = model.PATIENTNATIONALID != null ? model.PATIENTNATIONALID.Trim() : model.PATIENTNATIONALID;
            model.PATIENTADDRESS = model.PATIENTADDRESS != null ? model.PATIENTADDRESS.Trim() : model.PATIENTADDRESS;

            USER CurentUser = GetCurrentUser();
            if (CurentUser == null)
            {
                return Json(new { error = 1, msg = "Can't find current user !" });
            }

            // validation
            // check PATIENTNAME
            ValidationResult NameValidResult = ValidationInput.NameIsValid(model.PATIENTNAME, "Patient name");
            if (!NameValidResult.Success)
            {
                return Json(new { error = 1, msg = NameValidResult.ErrorMessage });
            }

            // check USERNAME
            ValidationResult UNValidResult = ValidationInput.UserNameIsValid(model.USERNAME, "Username");
            if (!UNValidResult.Success)
            {
                return Json(new { error = 1, msg = UNValidResult.ErrorMessage });
            }
            var usermatch = _dbContext.USER.Where(u => u.USERNAME == model.USERNAME).ToList();
            if (usermatch.Count > 0)
            {
                return Json(new { error = 1, msg = "Username already exists!" });
            }

            // check PASSWORD
            ValidationResult PaswdValidResult = ValidationInput.PasswordIsValid(model.PASSWORD);
            if (!PaswdValidResult.Success)
            {
                return Json(new { error = 1, msg = PaswdValidResult.ErrorMessage });
            }

            // check PATIENTGENDER
            var sysParam = _sysParam.GetAllParam();
            ValidationResult GenderValidResult = ValidationInput.GenderIsValid(model.PATIENTGENDER, sysParam);
            if (!GenderValidResult.Success)
            {
                return Json(new { error = 1, msg = GenderValidResult.ErrorMessage });
            }

            // check NATIONALID
            ValidationResult NationalValidResult = ValidationInput.NationalIsValid(model.PATIENTNATIONALID);
            if (!NationalValidResult.Success)
            {
                return Json(new { error = 1, msg = NationalValidResult.ErrorMessage });
            }
            var nationidmatch = _dbContext.DOCTOR.Where(u => u.DOCTORNATIONALID == model.PATIENTNATIONALID).ToList();
            if (nationidmatch.Count > 0)
            {
                return Json(new { error = 1, msg = "Nation ID already exists!" });
            }

            //check DATEOFBIRTH
            ValidationResult DOBValidResult = ValidationInput.DateOfBirthIsValid(model.PATIENTDATEOFBIRTH);
            if (!DOBValidResult.Success)
            {
                return Json(new { error = 1, msg = DOBValidResult.ErrorMessage });
            }

            //check MOBILENO
            ValidationResult MobileValidResult = ValidationInput.MobileIsValid(model.PATIENTMOBILENO);
            if (!MobileValidResult.Success)
            {
                return Json(new { error = 1, msg = MobileValidResult.ErrorMessage });
            }
            var mobilematch = _dbContext.PATIENT.Where(u => u.PATIENTMOBILENO == model.PATIENTMOBILENO).ToList();
            if (mobilematch.Count > 0)
            {
                return Json(new { error = 1, msg = "Mobile number already exists !" });
            }

            // check EMAIL
            ValidationResult EmailValidResult = ValidationInput.EmailIsValid(model.EMAIL);
            if (!EmailValidResult.Success)
            {
                return Json(new { error = 1, msg = EmailValidResult.ErrorMessage });
            }
            var emailmatch = _dbContext.USER.Where(u => u.EMAIL == model.EMAIL).ToList();
            if (emailmatch.Count > 0)
            {
                return Json(new { error = 1, msg = "Email already exists!" });
            }

            // check PATIENTADDRESS
            ValidationResult AddressValidResult = ValidationInput.AddressIsValid(model.PATIENTADDRESS);
            if (!AddressValidResult.Success)
            {
                return Json(new { error = 1, msg = AddressValidResult.ErrorMessage });
            }

            //map model bind to doctor
            PATIENT Patient;
            try
            {
                Patient = _mapper.GetMapper().Map<PatientCreateModel, PATIENT>(model);

            }
            catch
            {
                string sEventCatg = "ADMIN PORTAL";
                string sEventMsg = "Exception: Failed to mapping PatientCreateModel to PATIENT ";
                string sEventSrc = nameof(CreatePatient);
                string sEventType = "C";
                string sInsBy = GetCurrentUser().USERNAME;
                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return Json(new { error = 1, msg = "Faile to create patient" });
            }
           
            var date = DateTime.Now;
            Patient.CREATEDBY = CurentUser.USERNAME;
            Patient.CREATEDDATE = date;
            Patient.UPDATEDBY = CurentUser.USERNAME;
            Patient.UPDATEDDATE = date;

            //check role exists
            var role = _dbContext.ROLE.Where(r => r.ROLENAME == "Patient").FirstOrDefault();
            if (role == null)
            {
                role = _dbContext.ROLE.Add(new ROLE()
                {
                    ROLENAME = "Patient",
                    CREATEDBY = CurentUser.USERNAME,
                    UPDATEDBY = CurentUser.USERNAME,
                    CREATEDDATE = date,
                    UPDATEDDATE = date,
                    DELETEDFLAG = false,
                });
                try { 
                    _dbContext.SaveChanges();
                }
                catch 
                {
                    string sEventCatg = "ADMIN PORTAL";
                    string sEventMsg = "Exception: Failed to create new role ";
                    string sEventSrc = nameof(CreatePatient);
                    string sEventType = "I";
                    string sInsBy = GetCurrentUser().USERNAME;
                    Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                    return Json(new { error = 1, msg = "Faile to create patient" });
                }


            }

            var hashcode = PasswordHelper.HashPassword(model.PASSWORD);

            USER user = new USER()
            {

                USERNAME = model.USERNAME,
                ROLEID = role.ROLEID,
                PASSWORDHASH = hashcode,
                EMAIL = model.EMAIL,
                USERTYPE = "Patient",//Patient, Doctor , Admin 
                LOGINRETRYCOUNT = 0,
                LASTLOGIN= DateTime.Now,
                STATUS = true,
                CREATEDBY = CurentUser.USERNAME,
                UPDATEDBY = CurentUser.USERNAME,
                CREATEDDATE = date,
                UPDATEDDATE = date,
                DELETEDFLAG = false,
            };

            var newuser = _dbContext.USER.Add(user);
           

            Patient.USERID = newuser.USERID;

            _dbContext.PATIENT.Add(Patient);
            try { 
                _dbContext.SaveChanges(); 
            }
            catch 
            {
                string sEventCatg = "ADMIN PORTAL";
                string sEventMsg = "Exception: Failed to create new patient ";
                string sEventSrc = nameof(CreatePatient);
                string sEventType = "I";
                string sInsBy = GetCurrentUser().USERNAME;
                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return Json(new { error = 1, msg = "Faile to create patient" });
            }



            return Json(new { error = 0, msg = "ok" });
        }

        [HttpPost]
        //load patient data for update 
        public JsonResult LoadPatientInfo(int PatientId)
        {
            if (PatientId == 0)
            {
                return Json(new { error = 1, msg = "Error! do not find patient !" });
            }
            var patient = _dbContext.PATIENT.Where(p=>p.PATIENTID == PatientId && p.DELETEDFLAG == false).Include("USER").FirstOrDefault();
            if (patient == null)
            {
                return Json(new { error = 1, msg = "Error! do not find patient !" });
            }
            PatientViewEditModel patientInfo;
            try
            {
                patientInfo = _mapper.GetMapper().Map<PATIENT, PatientViewEditModel>(patient);
            }
            catch
            {
                string sEventCatg = "ADMIN PORTAL";
                string sEventMsg = "Exception: Failed to mapping PATIENT to PatientViewEditModel";
                string sEventSrc = nameof(LoadPatientInfo);
                string sEventType = "C";
                string sInsBy = GetCurrentUser().USERNAME;
                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return Json(new { error = 1, msg = "Faile to load patient info" });
            }
           

            return Json(new { error = 0, msg = "ok", patient = patientInfo });
        }

        // Create patient 
        [HttpPost]
        public JsonResult UpdatePatient(PatientEditModel model)
        {
            USER CurentUser = GetCurrentUser();
            if (CurentUser == null)
            {
                return Json(new { error = 1, msg = "Can't find current user !" });
            }
            var oldPatient = _dbContext.PATIENT.Where(p=>p.PATIENTID == model.PATIENTID && p.DELETEDFLAG == false).Include("USER").FirstOrDefault();
            USER oldUser = null;

            if (oldPatient == null)
            {
                return Json(new { error = 1, msg = "Error ! Can`t find Doctor !" });
            }
            else
            {
                if (oldPatient.USER == null)
                {
                    return Json(new { error = 1, msg = "Error ! Can`t find Doctor !" });
                }
                oldUser = oldPatient.USER;
            }

            model.PATIENTNAME = model.PATIENTNAME != null ? model.PATIENTNAME.Trim() : model.PATIENTNAME;
            model.PATIENTNATIONALID = model.PATIENTNATIONALID != null ? model.PATIENTNATIONALID.Trim() : model.PATIENTNATIONALID;
            model.PATIENTADDRESS = model.PATIENTADDRESS != null ? model.PATIENTADDRESS.Trim() : model.PATIENTADDRESS;

            // validation
            // check PATIENTNAME
            ValidationResult NameValidResult = ValidationInput.NameIsValid(model.PATIENTNAME, "Patient name");
            if (!NameValidResult.Success)
            {
                return Json(new { error = 1, msg = NameValidResult.ErrorMessage });
            }

            // check PATIENTGENDER
            var sysParam = _sysParam.GetAllParam();
            ValidationResult GenderValidResult = ValidationInput.GenderIsValid(model.PATIENTGENDER, sysParam);
            if (!GenderValidResult.Success)
            {
                return Json(new { error = 1, msg = GenderValidResult.ErrorMessage });
            }

            // check NATIONALID
            ValidationResult NationalValidResult = ValidationInput.NationalIsValid(model.PATIENTNATIONALID);
            if (!NationalValidResult.Success)
            {
                return Json(new { error = 1, msg = NationalValidResult.ErrorMessage });
            }
            var nationidmatch = _dbContext.PATIENT.Where(u => u.PATIENTNATIONALID == model.PATIENTNATIONALID && u.PATIENTID != oldPatient.PATIENTID ).ToList();
            if (nationidmatch.Count > 0)
            {
                return Json(new { error = 1, msg = "Nation ID already exists!" });
            }
            //check DATEOFBIRTH
            ValidationResult DOBValidResult = ValidationInput.DateOfBirthIsValid(model.PATIENTDATEOFBIRTH);
            if (!DOBValidResult.Success)
            {
                return Json(new { error = 1, msg = DOBValidResult.ErrorMessage });
            }

            //check MOBILENO
            ValidationResult MobileValidResult = ValidationInput.MobileIsValid(model.PATIENTMOBILENO);
            if (!MobileValidResult.Success)
            {
                return Json(new { error = 1, msg = MobileValidResult.ErrorMessage });
            }
            var mobilematch = _dbContext.PATIENT.Where(u => u.PATIENTMOBILENO == model.PATIENTMOBILENO && u.PATIENTMOBILENO != oldPatient.PATIENTMOBILENO ).ToList();
            if (mobilematch.Count > 0)
            {
                return Json(new { error = 1, msg = "Mobile number already exists !" });
            }

            // check EMAIL
            ValidationResult EmailValidResult = ValidationInput.EmailIsValid(model.EMAIL);
            if (!EmailValidResult.Success)
            {
                return Json(new { error = 1, msg = EmailValidResult.ErrorMessage });
            }
            var emailmatch = _dbContext.USER.Where(u => u.EMAIL == model.EMAIL && u.USERID != oldUser.USERID).ToList();
            if (emailmatch.Count > 0)
            {
                return Json(new { error = 1, msg = "Email already exists!" });
            }

            // check ADDRESS

            ValidationResult AddressValidResult = ValidationInput.AddressIsValid(model.PATIENTADDRESS);

            if (!AddressValidResult.Success)
            {
                return Json(new { error = 1, msg = AddressValidResult.ErrorMessage });
            }

            //map model bind to doctor
            PATIENT newPatient;
            try
            {
                newPatient = _mapper.GetMapper().Map<PatientEditModel, PATIENT>(model);
            }
            catch
            {
                string sEventCatg = "ADMIN PORTAL";
                string sEventMsg = "Exception: Failed to mapping PatientEditModel to PATIENT ";
                string sEventSrc = nameof(UpdatePatient);
                string sEventType = "C";
                string sInsBy = GetCurrentUser().USERNAME;
                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return Json(new { error = 1, msg = "Faile to update patient" });
            }
           

            //update old doctor info to new doctor  
            var date = DateTime.Now;
            newPatient.USERID = oldPatient.USERID;
            newPatient.CREATEDBY = oldPatient.CREATEDBY;
            newPatient.CREATEDDATE =oldPatient.CREATEDDATE;
            newPatient.UPDATEDBY = CurentUser.USERNAME;
            newPatient.UPDATEDDATE = date;
            newPatient.DELETEDFLAG = oldPatient.DELETEDFLAG;

            //update user info
            oldUser.EMAIL = model.EMAIL;
            oldUser.UPDATEDBY = CurentUser.USERNAME;
            oldUser.UPDATEDDATE = date;
            try
            {
                _dbContext.USER.AddOrUpdate(oldUser);
                _dbContext.PATIENT.AddOrUpdate(newPatient);
                _dbContext.SaveChanges();
            }
            catch
            {
                string sEventCatg = "ADMIN PORTAL";
                string sEventMsg = "Exception: Failed to update patient";
                string sEventSrc = nameof(UpdatePatient);
                string sEventType = "U";
                string sInsBy = GetCurrentUser().USERNAME;
                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return Json(new { error = 1, msg = "Faile to update patient" });
            }

            


            return Json(new { error = 0, msg = "ok" });
        }

        //delete patient
        [HttpPost]
        public JsonResult DeletePatient(int PatientId)
        {
            USER CurentUser = GetCurrentUser();
            if (CurentUser == null)
            {
                return Json(new { error = 1, msg = "Can't find current user !" });
            }
            if (PatientId == 0)
            {
                return Json(new { error = 1, msg = "Error! do not delete patient !" });
            }
            var patient = _dbContext.PATIENT.Where(p=>p.PATIENTID == PatientId && p.DELETEDFLAG == false).Include("USER").Include("APPOINTMENT").Include("APPOINTMENT.APPOINTMENT_PRESCRIPTION.PRESCRIPTION").FirstOrDefault();
            if (patient == null)
            {
                return Json(new { error = 1, msg = "Error! do not find patienr !" });
            }

            if (patient.APPOINTMENT.Count > 0)
            {
                patient.APPOINTMENT.ForEach(a =>
                {
                    if (a.APPOINTMENT_PRESCRIPTION.Count > 0)
                    {
                        a.APPOINTMENT_PRESCRIPTION.ForEach(p => { 
                            p.PRESCRIPTION.DELETEDFLAG = true;
                            _dbContext.APPOINTMENT_PRESCRIPTION.AddOrUpdate(p);
                        });
                       
                    }
                   
                    a.DELETEDFLAG = true; 
                    _dbContext.APPOINTMENT.AddOrUpdate(a);
                });
            }



            patient.UPDATEDBY = CurentUser.USERNAME;
            patient.UPDATEDDATE = DateTime.Now;
            patient.DELETEDFLAG = true;
            patient.UPDATEDDATE = DateTime.Now;
            patient.USER.DELETEDFLAG = true;
            patient.USER.UPDATEDBY = CurentUser.USERNAME;



            _dbContext.PATIENT.AddOrUpdate(patient);
            _dbContext.USER.AddOrUpdate(patient.USER);
            try { 
                _dbContext.SaveChanges(); 
            }
            catch 
            {
                string sEventCatg = "ADMIN PORTAL";
                string sEventMsg = "Exception: Failed to delete patient ";
                string sEventSrc = nameof(DeletePatient);
                string sEventType = "D";
                string sInsBy = GetCurrentUser().USERNAME;
                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return Json(new { error = 1, msg = "Faile to delete patient" });
            }


            return Json(new { error = 0, msg = "ok" });
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