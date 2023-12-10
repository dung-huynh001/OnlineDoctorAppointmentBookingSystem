using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.PatientManage
{
    public class PatientViewModel
    {
        public int PATIENTID { get; set; } //1
        public string PATIENTNAME { get; set; } //2
        public string USERNAME { get; set; } //3
        public string PATIENTGENDER { get; set; }//4
        public string PATIENTDATEOFBIRTH { get; set; }//5
        public string PATIENTNATIONALID { get; set; }//6
        public string PATIENTMOBILENO { get; set; }//7
        public string EMAIL { get; set; }//8
        public string PATIENTADDRESS { get; set; }//9
        public string LOGINLOCKDATE { get; set; } //10
        public int LOGINRETRYCOUNT { get; set; } //11
        public string CREATEDBY { get; set; }//12
        public string CREATEDDATE { get; set; }//13
        public string UPDATEDBY { get; set; }//14
        public string UPDATEDDATE { get; set; }//15
       

    }
}