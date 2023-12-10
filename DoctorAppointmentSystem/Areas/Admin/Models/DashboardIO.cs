using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models
{
    public class DashboardIO
    {
        private readonly DBContext _dbContext;
        private readonly IloggerService _logger;

        public DashboardIO(DBContext dbContext, IloggerService logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public int CountAllDoctor()
        {
            return _dbContext.DOCTOR.Count(d => d.DELETEDFLAG == false);
        }
        public int CountAllPatient()
        {
            return _dbContext.PATIENT.Count(d => d.DELETEDFLAG == false);
        }

        public int[] GetArrayAppointmentOnWeek()
        {
            var today = DateTime.Now;
            today = today.Date.AddHours(1).AddMinutes(0).AddSeconds(0);
            var thisWeekStart = today.AddDays(-(int)today.DayOfWeek + 1);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);


            var listApmOnWeek = _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false &&
                                                     a.DATEOFCONSULTATION >= thisWeekStart &&
                                                     a.DATEOFCONSULTATION <= thisWeekEnd).ToList();
            List<int> result = new List<int>(); 
            for (int i=0; i<7 ;i++)
            {
                result.Add( listApmOnWeek.Where(a => a.DATEOFCONSULTATION > thisWeekStart && a.DATEOFCONSULTATION <= thisWeekStart.AddDays(1)).Count());
                thisWeekStart = thisWeekStart.AddDays(1);
            }
            
            return result.ToArray();
        }
        public int[] GetArrayApptForStatusOnWeek(string status)
        {
            var today = DateTime.Now;
            today = today.Date.AddHours(1).AddMinutes(0).AddSeconds(0);
            var thisWeekStart = today.AddDays(-(int)today.DayOfWeek + 1);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);


            var listApmOnWeek = _dbContext.APPOINTMENT.Where(a => a.DELETEDFLAG == false &&
                                                     a.DATEOFCONSULTATION >= thisWeekStart &&
                                                     a.DATEOFCONSULTATION <= thisWeekEnd &&
                                                     a.APPOIMENTSTATUS == status
                                                     ).ToList();
            List<int> result = new List<int>();
            for (int i = 0; i < 7; i++)
            {
                result.Add(listApmOnWeek.Where(a => a.DATEOFCONSULTATION > thisWeekStart && a.DATEOFCONSULTATION <= thisWeekStart.AddDays(1)).Count());
                thisWeekStart = thisWeekStart.AddDays(1);
            }

            return result.ToArray();
        }


        public string[] GetArrayLabelOnWeek()
        {
            var today = DateTime.Now;
            today = today.Date.AddHours(1).AddMinutes(0).AddSeconds(0);
            var thisWeekStart = today.AddDays(-(int)today.DayOfWeek + 1);
            var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);

            List<string> result = new List<string>();
            for (int i = 0; i < 7; i++)
            {
                result.Add(thisWeekStart.DayOfWeek.ToString().Substring(0,3) +" "+ thisWeekStart.Day +"/"+thisWeekStart.Month);
                thisWeekStart = thisWeekStart.AddDays(1);
            }

            return result.ToArray();
        }


        public int CountAllAppointment()
        {
            return _dbContext.APPOINTMENT.Count(a => a.DELETEDFLAG == false);
        }
        public int CountAppointmentToDay()
        {
            var today = DateTime.Now;
            today = today.Date.AddHours(1).AddMinutes(0).AddSeconds(0);
            var nextday = today.AddDays(1);
            return _dbContext.APPOINTMENT.Count(a => a.DELETEDFLAG == false && a.APPOINTMENTDATE > today && a.APPOINTMENTDATE < nextday);
        }
    }
}