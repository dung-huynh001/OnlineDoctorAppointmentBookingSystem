using DoctorAppointmentSystem.Areas.Admin.Models.DoctorSchedule;
using DoctorAppointmentSystem.Areas.Admin.Models.Validation;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services.ServiceInterface;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;

using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models
{
    public class DoctorScheduleIO
    {
        private readonly DBContext _dbContext;
        private readonly ISystemParamService _sysParam;
        private readonly IMapperService _mapper;
        private readonly IloggerService _logger;

        public DoctorScheduleIO(DBContext dbContext, ISystemParamService sysParam, IMapperService mapper, IloggerService logger)
        {
            _dbContext = dbContext;
            _sysParam = sysParam;
            _mapper = mapper;
            _logger = logger;
        }
        // get all chedule
        public List<SCHEDULE> GetAllSchedule()
        {
            return _dbContext.SCHEDULE.Where(d => d.DELETEDFLAG == false).ToList();
        }

        //get schedule by id 
        public SCHEDULE GetScheduleById(int scheduleId)
        {
            return _dbContext.SCHEDULE.Where(s => s.SCHEDULEID == scheduleId && s.DELETEDFLAG == false).FirstOrDefault();

        }

        //validation schedule, isUpdate = true=> check for create 
        public ValidationResult IsVald(SCHEDULE schedule , bool isUpdate) {

            var datenow = DateTime.Now;
            //check null
            if(schedule == null)
            {
                return new ValidationResult { Success = false, ErrorMessage = "Schedule info is repuired" };
            }

            // check doctor
            if (schedule.DOCTORID <= 0)
            {
                return new ValidationResult { Success = false, ErrorMessage = "Doctor is repuired" };
            }
            var doctor = _dbContext.DOCTOR.Where(d => d.DOCTORID == schedule.DOCTORID && d.DELETEDFLAG == false).FirstOrDefault();
            if (doctor == null)
            {
                return new ValidationResult { Success = false, ErrorMessage = "Can't find doctor !" };
            }

            //check working day
            if (schedule.WORKINGDAY == null)
            {
                return new ValidationResult { Success = false, ErrorMessage = "Schedule date is repuired" };
            }
            schedule.WORKINGDAY = schedule.WORKINGDAY.AddHours(schedule.SHIFTTIME.Hours).AddMinutes(schedule.SHIFTTIME.Minutes);
            if (schedule.WORKINGDAY < datenow)
            {
                return new ValidationResult { Success = false, ErrorMessage = "Schedule date greater than current date" };
            }
            //check shift time
            if (schedule.SHIFTTIME == null)
            {
                return new ValidationResult { Success = false, ErrorMessage = "Start time is repuired" };
            }
            //check break time
            if (schedule.BREAKTIME == null)
            {
                return new ValidationResult { Success = false, ErrorMessage = "End time is repuired" };
            }
            if (schedule.BREAKTIME <= schedule.SHIFTTIME)
            {
                return new ValidationResult { Success = false, ErrorMessage = "Start time smaller than end time" };
            }
            //check consutant time
            if (schedule.CONSULTANTTIME <= 0)
            {
                return new ValidationResult { Success = false, ErrorMessage = "Consutant time is required" };
            }
            //check the amount of working time
            var workingTime = schedule.BREAKTIME - schedule.SHIFTTIME;
            if (schedule.CONSULTANTTIME > workingTime.TotalMinutes)
            {
                return new ValidationResult { Success = false, ErrorMessage = "the amount of working time must be greater than the consultation time" };
            }
            //check  overlaps
            bool isCreate = true;

            List<SCHEDULE> doctorScheduled = null;
            if (isUpdate)
            {
               doctorScheduled = _dbContext.SCHEDULE.Where(s => s.DOCTORID == schedule.DOCTORID &&
                                                               s.WORKINGDAY.Day == schedule.WORKINGDAY.Day &&
                                                               s.WORKINGDAY.Month == schedule.WORKINGDAY.Month &&
                                                               s.WORKINGDAY.Year == schedule.WORKINGDAY.Year &&
                                                               s.SCHEDULEID != schedule.SCHEDULEID &&
                                                               s.DELETEDFLAG == false
                                                               ).ToList();
            }
            else
            {
                doctorScheduled = _dbContext.SCHEDULE.Where(s => s.DOCTORID == schedule.DOCTORID &&
                                                                 s.WORKINGDAY.Day == schedule.WORKINGDAY.Day &&
                                                               s.WORKINGDAY.Month == schedule.WORKINGDAY.Month &&
                                                               s.WORKINGDAY.Year == schedule.WORKINGDAY.Year &&
                                                                s.DELETEDFLAG == false
                                                                ).ToList();
            }
            doctorScheduled.ForEach(s =>
            {
                //in
                if (schedule.SHIFTTIME >= s.SHIFTTIME && schedule.BREAKTIME <= s.BREAKTIME)
                {
                    isCreate = false;
                }
                //out
                if (schedule.SHIFTTIME < s.SHIFTTIME && schedule.BREAKTIME > s.BREAKTIME)
                {
                    isCreate = false;
                }
                //left
                if (schedule.SHIFTTIME < s.SHIFTTIME && schedule.BREAKTIME >= s.SHIFTTIME && schedule.BREAKTIME <= s.BREAKTIME)
                {
                    isCreate = false;
                }
                //right
                if (schedule.SHIFTTIME >= s.SHIFTTIME && schedule.SHIFTTIME <= s.BREAKTIME && schedule.BREAKTIME > s.BREAKTIME)
                {
                    isCreate = false;
                }

            });

            if (!isCreate) { return new ValidationResult { Success = false, ErrorMessage = "Start time or end time overlaps with another schedule !" }; }

            return new ValidationResult { Success = true,ErrorMessage = string.Empty };
        }

        //create schedule 
        public ValidationResult CreateSchedule(SCHEDULE schedule, string username)
        {
            var resultValid = IsVald(schedule,false);
            if (!resultValid.Success) { return resultValid; }
            schedule.CREATEDBY = username;
            schedule.CREATEDDATE = DateTime.Now;
            schedule.UPDATEDBY = username;
            schedule.UPDATEDDATE = DateTime.Now;
            schedule.DELETEDFLAG = false;

            try
            {
                _dbContext.SCHEDULE.Add(schedule);
                _dbContext.SaveChanges();
            }
            catch
            {
                _logger.InsertLog("ADMIN PORTAL", "create schedule is failed", nameof(CreateSchedule), "I", username);
                return new ValidationResult { Success = false, ErrorMessage = "Create doctor schedule is failed!" };
            }
            return new ValidationResult { Success = true, ErrorMessage = "ok" };
        }

        //update schedule
        public ValidationResult UpdateSchedule(SCHEDULE schedule, string username)
        {

            var oldSchedule = _dbContext.SCHEDULE.Where(s => s.DELETEDFLAG == false && s.SCHEDULEID == schedule.SCHEDULEID).FirstOrDefault();
            if(oldSchedule == null)
            {
                return new ValidationResult { Success = false, ErrorMessage = "Can't find schedule!" };
            }
            var resultValid = IsVald(schedule,true);
            if (!resultValid.Success) { return resultValid; }
            schedule.CREATEDBY = oldSchedule.CREATEDBY;
            schedule.CREATEDDATE = oldSchedule.CREATEDDATE;
            schedule.UPDATEDBY = username;
            schedule.UPDATEDDATE = DateTime.Now;
            schedule.DELETEDFLAG = false;

            try
            {
                _dbContext.SCHEDULE.AddOrUpdate(schedule);
                _dbContext.SaveChanges();
            }
            catch
            {
                _logger.InsertLog("ADMIN PORTAL", "update schedule is failed", nameof(UpdateSchedule), "U", username);
                return new ValidationResult { Success = false, ErrorMessage = "update schedule is failed!" };
            }
            return new ValidationResult { Success = true, ErrorMessage = "ok" };
        }

        // delete schedule
        public ValidationResult DeleteSchedule(int scheduleid, string username)
        {

            var schedule = _dbContext.SCHEDULE.Where(s => s.DELETEDFLAG == false && s.SCHEDULEID == scheduleid).Include("APPOINTMENT").FirstOrDefault();

            if (schedule == null)
            {
                return new ValidationResult { Success = false, ErrorMessage = "Can't find schedule!" };
            }
            if (schedule.APPOINTMENT.Count > 0)
            {
                schedule.APPOINTMENT.ForEach(a => {
                    a.DELETEDFLAG = false;
                    a.UPDATEDBY = username;
                    a.UPDATEDDATE = DateTime.Now;
                });
            }

            schedule.DELETEDFLAG = true;
            schedule.UPDATEDBY=username;
            schedule.UPDATEDDATE = DateTime.Now;

            try
            {
                _dbContext.SCHEDULE.AddOrUpdate(schedule);
                _dbContext.SaveChanges();
            }
            catch
            {
                _logger.InsertLog("ADMIN PORTAL", "delete schedule is failed", nameof(DeleteSchedule), "D", username);
                return new ValidationResult { Success = false, ErrorMessage = "delete schedule is failed!" };
            }
            return new ValidationResult { Success = true, ErrorMessage = "ok" };
        }
    }
}