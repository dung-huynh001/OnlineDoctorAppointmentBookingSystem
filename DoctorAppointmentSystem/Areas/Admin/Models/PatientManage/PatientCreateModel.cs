using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.PatientManage
{
    public class PatientCreateModel
    {
     
        public string PATIENTNAME { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string EMAIL { get; set; }
        public string PATIENTNATIONALID { get; set; }
        public int PATIENTGENDER { get; set; }
        public string PATIENTMOBILENO { get; set; }
        public System.DateTime PATIENTDATEOFBIRTH { get; set; }
        public string PATIENTADDRESS { get; set; }

    }
}