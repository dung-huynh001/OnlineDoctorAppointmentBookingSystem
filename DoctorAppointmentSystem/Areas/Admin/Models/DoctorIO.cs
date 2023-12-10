using DoctorAppointmentSystem.Areas.Admin.Models.DoctorManage;
using DoctorAppointmentSystem.Areas.Admin.Models.Validation;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models
{
    public class DoctorIO
    {
        private readonly DBContext _dbContext;
        private readonly ISystemParamService _sysParam;
        private readonly IMapperService _mapper;
        private readonly IloggerService _logger;

        public DoctorIO(DBContext dbContext, ISystemParamService sysParam, IMapperService mapper, IloggerService logger)
        {
            _dbContext = dbContext;
            _sysParam = sysParam;
            _mapper = mapper;
            _logger = logger;
        }

        public ValidationResult IsValid(DoctorCreateModel model )
        {
            model.DOCTORNAME = model.DOCTORNAME != null ? model.DOCTORNAME.Trim() : model.DOCTORNAME;
            model.DOCTORNATIONALID = model.DOCTORNATIONALID != null ? model.DOCTORNATIONALID.Trim() : model.DOCTORNATIONALID;
            model.DOCTORADDRESS = model.DOCTORADDRESS != null ? model.DOCTORADDRESS.Trim() : model.DOCTORADDRESS;

            // validation
            // check DOCTORNAME
            ValidationResult NameValidResult = ValidationInput.NameIsValid(model.DOCTORNAME, "Doctor name");
            if (!NameValidResult.Success)
            {
                return new ValidationResult { Success = false, ErrorMessage = NameValidResult.ErrorMessage };
            }

            // check USERNAME
            ValidationResult UNValidResult = ValidationInput.UserNameIsValid(model.USERNAME, "Username");
            if (!UNValidResult.Success)
            {
                return new ValidationResult { Success = false, ErrorMessage = UNValidResult.ErrorMessage };
            }

            
            var usermatch = _dbContext.USER.Where(u => u.USERNAME == model.USERNAME).ToList();

            if (usermatch.Count > 0)
            {
                return new ValidationResult { Success = false, ErrorMessage = "Username already exists!" };
            }

            // check DOCTORGENDER
            var sysParam = _sysParam.GetAllParam();
            ValidationResult GenderValidResult = ValidationInput.GenderIsValid(model.DOCTORGENDER, sysParam);
            if (!GenderValidResult.Success)
            {
                return new ValidationResult { Success = false, ErrorMessage = GenderValidResult.ErrorMessage };
            }

            // check PASSWORD
            ValidationResult PaswdValidResult = ValidationInput.PasswordIsValid(model.PASSWORD);
            if (!PaswdValidResult.Success)
            {
                return new ValidationResult { Success = false, ErrorMessage = PaswdValidResult.ErrorMessage };
            }
            // check DOCTORNATIONALID
            ValidationResult NationalValidResult = ValidationInput.NationalIsValid(model.DOCTORNATIONALID);
            if (!NationalValidResult.Success)
            {
                return new ValidationResult { Success = false, ErrorMessage = NationalValidResult.ErrorMessage };
            }
            var nationidmatch = _dbContext.DOCTOR.Where(u => u.DOCTORNATIONALID == model.DOCTORNATIONALID).ToList();
            if (nationidmatch.Count > 0)
            {
                return new ValidationResult { Success = false, ErrorMessage = "Nation ID already exists !" };
            }

            //check DOCTORDATEOFBIRTH
            ValidationResult DOBValidResult = ValidationInput.DateOfBirthIsValid(model.DOCTORDATEOFBIRTH);
            if (!DOBValidResult.Success)
            {
                return new ValidationResult { Success = false, ErrorMessage = DOBValidResult.ErrorMessage };
            }

            //check DOCTORMOBILENO
            ValidationResult MobileValidResult = ValidationInput.MobileIsValid(model.DOCTORMOBILENO);
            if (!MobileValidResult.Success)
            {
                return new ValidationResult { Success = false, ErrorMessage = MobileValidResult.ErrorMessage };
            }
            var mobilematch = _dbContext.DOCTOR.Where(u => u.DOCTORMOBILENO == model.DOCTORMOBILENO).ToList();
            if (mobilematch.Count > 0)
            {
                return new ValidationResult { Success = false, ErrorMessage = "Mobile number already exists !" };
            }
            // check EMAIL
            ValidationResult EmailValidResult = ValidationInput.EmailIsValid(model.EMAIL);
            if (!EmailValidResult.Success)
            {
                return new ValidationResult { Success = false, ErrorMessage = EmailValidResult.ErrorMessage };
            }
            var emailmatch = _dbContext.USER.Where(u => u.EMAIL == model.EMAIL).ToList();
            if (emailmatch.Count > 0)
            {
                return new ValidationResult { Success = false, ErrorMessage = "Email already exists!" };
            }

            // check DOCTORADDRESS
            ValidationResult AddressValidResult = ValidationInput.AddressIsValid(model.DOCTORADDRESS);
            if (!AddressValidResult.Success)
            {
                return new ValidationResult { Success = false, ErrorMessage = AddressValidResult.ErrorMessage };
            }
            // check SPECIALITY
            ValidationResult SpecialityValidResult = ValidationInput.SpecialityIsValid(model.SPECIALITY);
            if (!SpecialityValidResult.Success)
            {
                return new ValidationResult { Success = false, ErrorMessage = SpecialityValidResult.ErrorMessage };
            }

            // check WORKINGSTARTDATE , WORKINGENDDATE
            ValidationResult WorkingValidResult = ValidationInput.WorkingIsValid(model.WORKINGSTARTDATE, model.WORKINGENDDATE);
            if (!WorkingValidResult.Success)
            {
                return new ValidationResult { Success = false, ErrorMessage = WorkingValidResult.ErrorMessage };
            }

            return new ValidationResult { Success = true ,ErrorMessage ="" };
        }

        public ValidationResult CreateDoctor(DoctorCreateModel model , string username)
        {
            ValidationResult resultValid = IsValid(model);
            if (!resultValid.Success)
            {
                return resultValid;
            }
            //map model bind to doctor
            DOCTOR Doctors ;
            try
            { 
                Doctors = _mapper.GetMapper().Map<DoctorCreateModel, DOCTOR>(model);
            }
            catch
            {
                string sEventCatg = "ADMIN PORTAL";
                string sEventMsg = "Exception: Failed to mapping DoctorCreateModel to DOCTOR ";
                string sEventSrc = nameof(CreateDoctor);
                string sEventType = "C";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return new ValidationResult { Success = false, ErrorMessage = "Failed to create doctor" };
            }
           

            var department = _dbContext.DEPARTMENT.Find(Doctors.DEPARTMENTID);
            if (department == null) { return new ValidationResult { Success = false, ErrorMessage = "Error ! Can`t find Department !" }; }
            Doctors.DEPARTMENT = department;

            var date = DateTime.Now;
            Doctors.CREATEDBY = username;
            Doctors.CREATEDDATE = date;
            Doctors.UPDATEDBY = username;
            Doctors.UPDATEDDATE = date;

            //check role exists
            var role = _dbContext.ROLE.Where(r => r.ROLENAME == "Doctor").FirstOrDefault();
            if (role == null)
            {
                role = _dbContext.ROLE.Add(new ROLE()
                {
                    ROLENAME = "Doctor",
                    CREATEDBY = username,
                    UPDATEDBY = username,
                    CREATEDDATE = date,
                    UPDATEDDATE = date,
                    DELETEDFLAG = false,
                });

                try
                {
                    _dbContext.SaveChanges();
                }
                catch 
                {
                    string sEventCatg = "ADMIN PORTAL";
                    string sEventMsg = "Exception: Adding role is failed ";
                    string sEventSrc = nameof(CreateDoctor);
                    string sEventType = "I";
                    string sInsBy = username;

                    Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                    //write error log
                    return new ValidationResult { Success = false, ErrorMessage = "" };
                }
            }

            var hashcode = PasswordHelper.HashPassword(model.PASSWORD);

            USER user = new USER()
            {

                USERNAME = model.USERNAME,
                ROLEID = role.ROLEID,
                PASSWORDHASH = hashcode,
                EMAIL = model.EMAIL,
                USERTYPE = "Doctor",//Partient , Admin
                LOGINRETRYCOUNT = 0,
                LASTLOGIN = DateTime.Now,
                STATUS = true,
                CREATEDBY = username,
                UPDATEDBY = username,
                CREATEDDATE = date,
                UPDATEDDATE = date,
                DELETEDFLAG = false,
            };

            var newuser = _dbContext.USER.Add(user);
            Doctors.USERID = newuser.USERID;
            _dbContext.DOCTOR.Add(Doctors);

            try
            {
                _dbContext.SaveChanges();

            }
            catch 
            {
                string sEventCatg = "ADMIN PORTAL";
                string sEventMsg = "Exception: Failed to save new doctor ";
                string sEventSrc = nameof(CreateDoctor);
                string sEventType = "I";
                string sInsBy = username;
                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
               
                return new ValidationResult { Success = false, ErrorMessage = "Failed to create doctor" };
            }


            return new ValidationResult { Success = true, ErrorMessage = "ok" };
        }

        public ValidationResult IsValidUpdate(DoctorEditModel model,DOCTOR oldDoctor)
        {
            model.DOCTORNAME = model.DOCTORNAME != null ? model.DOCTORNAME.Trim() : model.DOCTORNAME;
            model.DOCTORNATIONALID = model.DOCTORNATIONALID != null ? model.DOCTORNATIONALID.Trim() : model.DOCTORNATIONALID;
            model.DOCTORADDRESS = model.DOCTORADDRESS != null ? model.DOCTORADDRESS.Trim() : model.DOCTORADDRESS;
            ;
            // validation
            // check DOCTORNAME
            ValidationResult NameValidResult = ValidationInput.NameIsValid(model.DOCTORNAME, "Doctor name");
            if (!NameValidResult.Success)
            {
                return new ValidationResult { Success = false, ErrorMessage = NameValidResult.ErrorMessage };
            }
            // check DOCTORGENDER
            var sysParam = _sysParam.GetAllParam();
            ValidationResult GenderValidResult = ValidationInput.GenderIsValid(model.DOCTORGENDER, sysParam);
            if (!GenderValidResult.Success)
            {
                return new ValidationResult { Success = false, ErrorMessage = GenderValidResult.ErrorMessage };
            }

            // check DOCTORNATIONALID
            ValidationResult NationalValidResult = ValidationInput.NationalIsValid(model.DOCTORNATIONALID);
            if (!NationalValidResult.Success)
            {
                return new ValidationResult { Success = false, ErrorMessage = NationalValidResult.ErrorMessage };
            }
            var nationidmatch = _dbContext.DOCTOR.Where(u => u.DOCTORNATIONALID == model.DOCTORNATIONALID &&
                                                             u.DOCTORNATIONALID != oldDoctor.DOCTORNATIONALID).ToList();
            if (nationidmatch.Count > 0)
            {
                return new ValidationResult { Success = false, ErrorMessage = "Nation ID already exists !" };
            }

            //check DOCTORDATEOFBIRTH
            ValidationResult DOBValidResult = ValidationInput.DateOfBirthIsValid(model.DOCTORDATEOFBIRTH);
            if (!DOBValidResult.Success)
            {
                return new ValidationResult { Success = false, ErrorMessage = DOBValidResult.ErrorMessage };
            }

            //check DOCTORMOBILENO
            ValidationResult MobileValidResult = ValidationInput.MobileIsValid(model.DOCTORMOBILENO);
            if (!MobileValidResult.Success)
            {
                return new ValidationResult { Success = false, ErrorMessage = MobileValidResult.ErrorMessage };
            }
            var mobilematch = _dbContext.DOCTOR.Where(u => u.DOCTORMOBILENO == model.DOCTORMOBILENO &&
                                                           u.DOCTORMOBILENO != oldDoctor.DOCTORMOBILENO).ToList();
            if (mobilematch.Count > 0)
            {
                return new ValidationResult { Success = false, ErrorMessage = "Mobile number already exists !" };
            }

            // check EMAIL
            ValidationResult EmailValidResult = ValidationInput.EmailIsValid(model.EMAIL);
            if (!EmailValidResult.Success)
            {
                return new ValidationResult { Success = false, ErrorMessage = EmailValidResult.ErrorMessage };
            }
            var emailmatch = _dbContext.USER.Where(u => u.EMAIL == model.EMAIL &&
                                                        u.EMAIL != oldDoctor.USER.EMAIL).ToList();
            if (emailmatch.Count > 0)
            {
                return new ValidationResult { Success = false, ErrorMessage = "Email already exists!" };
            }

            // check DOCTORADDRESS
            ValidationResult AddressValidResult = ValidationInput.AddressIsValid(model.DOCTORADDRESS);
            if (!AddressValidResult.Success)
            {
                return new ValidationResult { Success = false, ErrorMessage = AddressValidResult.ErrorMessage };
            }
            // check SPECIALITY
            ValidationResult SpecialityValidResult = ValidationInput.SpecialityIsValid(model.SPECIALITY);
            if (!SpecialityValidResult.Success)
            {
                return new ValidationResult { Success = false, ErrorMessage = SpecialityValidResult.ErrorMessage };
            }

            // check WORKINGSTARTDATE , WORKINGENDDATE
            ValidationResult WorkingValidResult = ValidationInput.WorkingIsValid(model.WORKINGSTARTDATE, model.WORKINGENDDATE);
            if (!WorkingValidResult.Success)
            {
                return new ValidationResult { Success = false, ErrorMessage = WorkingValidResult.ErrorMessage };
            }

            return new ValidationResult { Success = true, ErrorMessage = "" };
        }

        public ValidationResult UpdateDoctor(DoctorEditModel model , string currentUserName)
        {


            var oldDoctor = _dbContext.DOCTOR.Where(d => d.DOCTORID == model.DOCTORID && d.DELETEDFLAG == false).Include("USER").FirstOrDefault();
            USER oldUser = null;

            if (oldDoctor == null)
            {
                return new ValidationResult { Success = false, ErrorMessage = "Error ! Can`t find Doctor !" };
            }
            else
            {
                if (oldDoctor.USER == null)
                {
                    return new ValidationResult { Success = false, ErrorMessage = "Error ! Can`t find Doctor !" };
                }
                oldUser = oldDoctor.USER;
            }

            var  resultValid = IsValidUpdate(model, oldDoctor);
            if (!resultValid.Success)
            {
                return resultValid;
            }

            //map model bind to doctor
            DOCTOR newDoctors;
            try
            {
                newDoctors = _mapper.GetMapper().Map<DoctorEditModel, DOCTOR>(model);
            }
            catch
            {
                string sEventCatg = "ADMIN PORTAL";
                string sEventMsg = "Exception: Failed to mapping DoctorEditModel to DOCTOR";
                string sEventSrc = nameof(CreateDoctor);
                string sEventType = "C";
                string sInsBy = currentUserName;
                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);

                return new ValidationResult { Success = false, ErrorMessage = "Failed to update doctor" };
            }
            
            var department = _dbContext.DEPARTMENT.Find(newDoctors.DEPARTMENTID);
            if (department == null) { return new ValidationResult { Success = false, ErrorMessage = "Error ! Can`t find Department !" }; }
            newDoctors.DEPARTMENT = department;
            var date = DateTime.Now;

            //update old doctor info to new doctor  
            newDoctors.DOCTORID = oldDoctor.DOCTORID;
            newDoctors.USERID = oldUser.USERID;
            newDoctors.CREATEDBY = oldDoctor.CREATEDBY;
            newDoctors.CREATEDDATE = oldDoctor.CREATEDDATE;
            newDoctors.UPDATEDBY = currentUserName;
            newDoctors.UPDATEDDATE = date;
            newDoctors.DELETEDFLAG = false;

            //update user info
            oldUser.EMAIL = model.EMAIL;
            oldUser.UPDATEDDATE = date;
            oldUser.UPDATEDBY = currentUserName;

            _dbContext.USER.AddOrUpdate(oldUser);
            _dbContext.DOCTOR.AddOrUpdate(newDoctors);
            try
            {
                _dbContext.SaveChanges();
            }
            catch 
            {
                string sEventCatg = "ADMIN PORTAL";
                string sEventMsg = "Exception: Failed to saving update doctor ";
                string sEventSrc = nameof(CreateDoctor);
                string sEventType = "I";
                string sInsBy = currentUserName;
                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);

                return new ValidationResult { Success = false, ErrorMessage = "Failed to create doctor" };
               
            }
            return new ValidationResult { Success = true, ErrorMessage = "ok" };




           
        }
    }
}