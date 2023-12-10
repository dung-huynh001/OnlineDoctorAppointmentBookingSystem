using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Doctor.Models.Appointments
{
    public class CompletedApptViewModel
    {
        public int APPOINTMENTID { get; set; } //1
        public string PATIENTNAME { get; set; } //2
        public string DATEOFCONSULTANT { get; set; }//3
        public string APPOINTMENTDATE { get; set; }//4
        public string APPOIMENTSTATUS { get; set; }//5
        public string CONSULTANTTIME { get; set; } //6
        public string CLOSEDDATE { get; set; }//7
        public string CLOSEDBY { get; set; }//8
        public int PATIENID { get; set; }
      
       
    }
}
