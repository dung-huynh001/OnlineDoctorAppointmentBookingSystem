using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Doctor.Models.Home
{
    public class calendarIO
    {
        private readonly DBContext _dbContext;
        private readonly ISystemParamService _sysParam;
        private readonly IMapperService _mapper;

        public calendarIO(DBContext dbContext, ISystemParamService sysParam, IMapperService mapper)
        {
            _dbContext = dbContext;
            _sysParam = sysParam;
            _mapper = mapper;
        }

       public List<EventViewModel> GetDoctorSchedule(DateTime start, DateTime end, int doctorId, string status) // param string filter 
        {
            List<EventViewModel> schedule;
            List<APPOINTMENT> apptments;
            
            try {
                if (string.IsNullOrEmpty(status))
                {
                     apptments = _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false &&
                                                                        a.DATEOFCONSULTATION > start && 
                                                                        a.DATEOFCONSULTATION < end &&
                                                                        a.DOCTORID == doctorId //&& 
                                                                        //a.APPOIMENTSTATUS == "Confirm"
                                                                        )
                                                                        .Include("PATIENT").ToList();
                }else 
                {
                    apptments = _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false &&
                                                                       a.DATEOFCONSULTATION > start &&
                                                                       a.DATEOFCONSULTATION < end &&
                                                                       a.DOCTORID == doctorId && 
                                                                       a.APPOIMENTSTATUS == status
                                                                       )
                                                                       .Include("PATIENT").ToList();
                }
               

                schedule = apptments.Select(s => new EventViewModel
                {
                    id = s.APPOINTMENTID,
                    title = "Patient: " + s.PATIENT.PATIENTNAME,
                    start = s.DATEOFCONSULTATION.ToString("yyyy-MM-ddTHH:mm:ss"),
                    description = "Status: " + s.APPOIMENTSTATUS + ((s.APPOIMENTSTATUS == "Pending" || s.APPOIMENTSTATUS == "Confirm") && s.DATEOFCONSULTATION < DateTime.Now ? "<br/><span class=\"text-danger\"> Appointment time exceeded </span>" : "" ),
                    url = GetUrlRedirect(s.APPOIMENTSTATUS),
                    allDay = false,
                    color = s.APPOIMENTSTATUS == "Pending" ? (s.DATEOFCONSULTATION< DateTime.Now)? "#747399" : "#ffc107" :
                            s.APPOIMENTSTATUS == "Completed" ? "#00ff21" :
                            s.APPOIMENTSTATUS == "Cancel" ? "#dc3545" : (s.DATEOFCONSULTATION < DateTime.Now) ? "#747399":"#007bff"
                             ,
                }).ToList();
            }
            catch
            {
                schedule = new List<EventViewModel>();
            }
            return schedule;
        }

        private string GetUrlRedirect(string status)
        {
            string Url = status == "Pending" ? "/Doctor/Appointments" :
                            status == "Completed" ? "/Doctor/CompletedAppt" :
                            status == "Confirm" ? "/Doctor/ConfirmedAppt" : "/Doctor/CancelledAppt";

            return Url;
        }

    }
}