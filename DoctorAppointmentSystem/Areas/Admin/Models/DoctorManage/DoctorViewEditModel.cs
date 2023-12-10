using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.DoctorManage
{
    public class DoctorViewEditModel
    {
        public int DOCTORID { get; set; } 
        public string DOCTORNAME { get; set; } 
        public string USERNAME { get; set; }
        //public string DEPARTMENT { get; set; } 
        public int DOCTORGENDER { get; set; }
        public string DOCTORDATEOFBIRTH { get; set; }
        public string DOCTORNATIONALID { get; set; }
        public string DOCTORMOBILENO { get; set; }
        public string EMAIL { get; set; }
        public string DOCTORADDRESS { get; set; }
        public string SPECIALITY { get; set; }
        //public string QUALIFICATION { get; set; } 
        public string WORKINGSTARTDATE { get; set; }
        public string WORKINGENDDATE { get; set; }
        public string LOGINLOCKDATE { get; set; } 
        public string AVATARURL { get; set; }
        public int DEPARTMENTID { get; set; }


    }
}