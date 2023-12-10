using DoctorAppointmentSystem.Areas.Admin.Models.AppointmentManage;
using DoctorAppointmentSystem.Areas.Admin.Models.Validation;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Models.Appointment;
using DoctorAppointmentSystem.Models.Appointment.MakeAppointment;
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
    public class AppointmentIO
    {
        private readonly DBContext _dbContext;
        private readonly ISystemParamService _sysParam;
        private readonly IMapperService _mapper;
        private readonly IloggerService _logger;

        public AppointmentIO(DBContext dbContext, ISystemParamService sysParam, IMapperService mapper, IloggerService logger)
        {
            _dbContext = dbContext;
            _sysParam = sysParam;
            _mapper = mapper;
            _logger = logger;
        }

        public List<APPOINTMENT> GetAllAppointment()
        {
            return _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false).Include("PATIENT").Include("SCHEDULE").Include("SCHEDULE.DOCTOR").ToList();
        }

        public ValidationResult DeleteAppointment(int ApmId ,string username)
        {

            APPOINTMENT appointment;
            appointment = _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false && a.APPOINTMENTID == ApmId).Include("APPOINTMENT_PRESCRIPTION").Include("APPOINTMENT_PRESCRIPTION.PRESCRIPTION").FirstOrDefault();
            try
            {
                 appointment = _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false && a.APPOINTMENTID == ApmId).Include("APPOINTMENT_PRESCRIPTION").Include("APPOINTMENT_PRESCRIPTION.PRESCRIPTION").FirstOrDefault();
            }
            catch
            {
                string sEventCatg = "ADMIN PORTAL";
                string sEventMsg = "Exception: Failed to delete appointment";
                string sEventSrc = nameof(DeleteAppointment);
                string sEventType = "D";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);

                return new ValidationResult { Success = false, ErrorMessage = "Failed to delete appointment" };
            }
           
           if( appointment == null)
            {
                return new ValidationResult { Success = false, ErrorMessage = "Can't find appointment" };
            }
            if (appointment.APPOINTMENT_PRESCRIPTION.Count > 0)
            {
                appointment.APPOINTMENT_PRESCRIPTION.ForEach(item => { 
                    item.PRESCRIPTION.DELETEDFLAG = true;
                    item.PRESCRIPTION.UPDATEDDATE = DateTime.Now;
                    item.PRESCRIPTION.UPDATEDBY = username;
                    _dbContext.PRESCRIPTION.AddOrUpdate(item.PRESCRIPTION);
                });
            }

            appointment.DELETEDFLAG = true;
            appointment.UPDATEDDATE= DateTime.Now;
            appointment.UPDATEDBY = username;

            _dbContext.APPOINTMENT.AddOrUpdate(appointment);

            try
            {
                _dbContext.SaveChanges();
            }
            catch
            {
                string sEventCatg = "ADMIN PORTAL";
                string sEventMsg = "Exception: Failed to delete appointment";
                string sEventSrc = nameof(DeleteAppointment);
                string sEventType = "D";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);

                return new ValidationResult { Success = false, ErrorMessage = "Failed to delete appointment" };
            }

            return new ValidationResult { Success = true, ErrorMessage = "ok" };
        }

        public CompletedApptViewDetailsModel getAppointmentInfo(int appointmentID,string username)
        {
            CompletedApptViewDetailsModel avm = new CompletedApptViewDetailsModel();
            try {  
            var apm =_dbContext.APPOINTMENT.Where(a => a.APPOINTMENTID == appointmentID)
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

    }
}