using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.DoctorManage
{
    public class DoctorCreateModel
    {
       
        public int DEPARTMENTID { get; set; }
        public string DOCTORNAME { get; set; }
        public string PASSWORD { get; set; }
        public string USERNAME { get; set; }
        public string DOCTORNATIONALID { get; set; }
        public int DOCTORGENDER { get; set; }
        public DateTime DOCTORDATEOFBIRTH { get; set; }
        public string DOCTORMOBILENO { get; set; }
        public string EMAIL { get; set; }
        public string DOCTORADDRESS { get; set; }
        public string SPECIALITY { get; set; }
        public DateTime LOGINLOCKDATE { get; set; }
        public DateTime WORKINGSTARTDATE { get; set; }
        public DateTime WORKINGENDDATE { get; set; }
       
    }
}