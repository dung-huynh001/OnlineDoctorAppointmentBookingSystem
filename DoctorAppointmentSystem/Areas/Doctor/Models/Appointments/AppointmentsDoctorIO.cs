using DoctorAppointmentSystem.Areas.Admin.Models.AppointmentManage;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Doctor.Models.Appointments
{

    public class AppointmentsDoctorIO
    {
        private readonly DBContext _dbContext;
        private readonly ISystemParamService _sysParam;
        private readonly IMapperService _mapper;

        public AppointmentsDoctorIO(DBContext dbContext, ISystemParamService sysParam, IMapperService mapper)
        {
            _dbContext = dbContext;
            _sysParam = sysParam;
            _mapper = mapper;
        }

        public List<APPOINTMENT> GetAllPendingAppt(int doctorId)
        {
            return _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false && a.DOCTORID == doctorId && a.APPOIMENTSTATUS == "Pending").Include("PATIENT").Include("SCHEDULE").AsNoTracking().ToList();
        }
        public List<APPOINTMENT> GetAllCancelledAppt(int doctorId)
        {
            return _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false && a.DOCTORID == doctorId && a.APPOIMENTSTATUS == "Cancel").Include("PATIENT").AsNoTracking().ToList();
        }
        public List<APPOINTMENT> GetAllCompleteAppt(int doctorId)
        {
            return _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false && a.DOCTORID == doctorId && a.APPOIMENTSTATUS == "Completed").Include("PATIENT").Include("SCHEDULE").AsNoTracking().ToList();
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
                                                .AsNoTracking()
                                                .FirstOrDefault();

                avm = _mapper.GetMapper().Map<APPOINTMENT, CompletedApptViewDetailsModel>(apm);
            }
            catch
            {
                string sEventCatg = "DOCTOR PORTAL";
                string sEventMsg = "Exception: Failed to load detail appointment";
                string sEventSrc = nameof(getAppointmentInfo);
                string sEventType = "L";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
            }
            return avm;

        }

        public int CountApptPending(int doctorId)
        {
            return _dbContext.APPOINTMENT.Where(a=>a.DELETEDFLAG == false && a.DOCTORID == doctorId && a.APPOIMENTSTATUS == "Pending").Count();
        }
        public int CountApptToDay(int doctorId)
        {
            var startDate = DateTime.Now;
            startDate = startDate.Date.AddHours(1).AddMinutes(0).AddSeconds(0);
           var endDate = startDate.AddDays(1);

            return _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false && 
                                                    a.APPOIMENTSTATUS == "Confirm" &&
                                                    a.DOCTORID == doctorId &&
                                                    a.DATEOFCONSULTATION >=  startDate && a.DATEOFCONSULTATION < endDate).Count();
        }

        public bool ConfirmAppointment(int id)
        {
            bool success = false;
            try
            {
                APPOINTMENT appointment = _dbContext.APPOINTMENT.Find(id);
                if (appointment == null)
                {
                    throw new Exception();
                }
                else
                {
                    appointment.APPOIMENTSTATUS = "Confirm";
                    appointment.UPDATEDBY = GetInfo.Username;
                    appointment.UPDATEDDATE = DateTime.Now;

                    _dbContext.SaveChanges();

                    success = true;
                }
                return success;
            }
            catch (Exception ex)
            {
                string username = GetInfo.Username;
                string sEventCatg = GetInfo.GetUserType(username).ToUpper() + "PORTAL";
                string sEventMsg = ex.Message;
                string sEventSrc = nameof(ConfirmAppointment);
                string sEventType = "U";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return success;
            }
        }

        public bool CancelAppointment(int id)
        {
            bool success = false;

            try
            {
                APPOINTMENT appointment = _dbContext.APPOINTMENT.Find(id);
                if (appointment == null)
                {
                    throw new Exception();
                }
                else
                {
                    appointment.APPOIMENTSTATUS = "Cancel";
                    appointment.UPDATEDBY = GetInfo.Username;
                    appointment.UPDATEDDATE = DateTime.Now;

                    _dbContext.SaveChanges();

                    success = true;
                }
                return success;
            }
            catch (Exception ex)
            {
                string username = GetInfo.Username;
                string sEventCatg = GetInfo.GetUserType(username).ToUpper() + "PORTAL";
                string sEventMsg = ex.Message;
                string sEventSrc = nameof(CancelAppointment);
                string sEventType = "U";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);

                return success;
            }
        }

        public int CountApptComfirmed(int doctorId)
        {
            return _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false && a.DOCTORID == doctorId && a.APPOIMENTSTATUS == "Confirm").Count();
        }
        public int CountApptCompleted(int doctorId)
        {
            return _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false && a.DOCTORID == doctorId && a.APPOIMENTSTATUS == "Completed").Count();
        }



        public async Task<int> CountApptPendingAsync(int doctorId)
        {
            return await _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false && a.DOCTORID == doctorId && a.APPOIMENTSTATUS == "Pending").CountAsync();
        }
        public async Task<int> CountApptToDayAsync(int doctorId)
        {
            var startDate = DateTime.Now;
            startDate = startDate.Date.AddHours(1).AddMinutes(0).AddSeconds(0);
            var endDate = startDate.AddDays(1);

            return await _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false &&
                                                    a.APPOIMENTSTATUS == "Confirm" &&
                                                    a.DOCTORID == doctorId &&
                                                    a.DATEOFCONSULTATION >= startDate && a.DATEOFCONSULTATION < endDate).CountAsync();
        }
        public async Task<int> CountApptComfirmedAsync(int doctorId)
        {
            return await _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false && a.DOCTORID == doctorId && a.APPOIMENTSTATUS == "Confirm").CountAsync();
        }
        public async Task<int> CountApptCompletedAsync(int doctorId)
        {
            return await _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false && a.DOCTORID == doctorId && a.APPOIMENTSTATUS == "Completed").CountAsync();
        }
    }
}