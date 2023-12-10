using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.DoctorManage
{
    public class DoctorViewModel
    {
        public int DOCTORID { get; set; } //1
        public string DOCTORNAME { get; set; } //2
        public string USERNAME { get; set; } //3
        public string DEPARTMENT { get; set; } //4
        public string DOCTORGENDER { get; set; }//5
        public string DOCTORDATEOFBIRTH { get; set; }//6
        public string DOCTORNATIONALID { get; set; }//7
        public string DOCTORMOBILENO { get; set; }//8
        public string EMAIL { get; set; }//9
        public string DOCTORADDRESS { get; set; }//10
        public string SPECIALITY { get; set; }//11
        public string WORKINGSTARTDATE { get; set; }//12
        public string WORKINGENDDATE { get; set; }//13
        public string LOGINLOCKDATE { get; set; } //14
        public int LOGINRETRYCOUNT { get; set; } //15
        public string CREATEDBY { get; set; }//16
        public string CREATEDDATE { get; set; }//17
        public string UPDATEDBY { get; set; }//18
        public string UPDATEDDATE { get; set; }//19
        public string AVATARURL { get; set; }//20
        public int DEPARTMENTID { get; set; }//21


    }
}