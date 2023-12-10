using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Doctor.Models.Appointments
{
    public class AppointmentViewModel
    {
        public int APPOINTMENTID { get; set; } //1
        public string PATIENTNAME { get; set; } //2
        public string DATEOFCONSULTANT { get; set; }//3
        public string DATEOFCONSULTANTTIME { get; set; }//4
        public string DATEOFCONSULTANTDAY { get; set; }//5
        public string APPOIMENTSTATUS { get; set; }//6
        public string CONSULTANTTIME { get; set; } //7
        public int PATIENID { get; set; }
        public bool LATE { get; set; }


    }
}