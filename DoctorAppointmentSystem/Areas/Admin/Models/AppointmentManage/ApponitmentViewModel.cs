using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.AppointmentManage
{
    public class ApponitmentViewModel
    {
        public int APPOINTMENTID { get; set; } //1
      
        public string PATIENTNAME { get; set; } //2
       
        public string DOCTORNAME { get; set; }//3
        public string APPOINTMENTDATE { get; set; }//4
        public string DATEOFCONSUITATION { get; set; }//5
       //6
        public string APPOIMENTSTATUS { get; set; }//7
         //8
        public string CLOSEDDATE { get; set; }//9
        public string CLOSEDBY { get; set; }//10
        public string CREATEDBY { get; set; }//11
        public string CREATEDDATE { get; set; }//12
        public string UPDATEDBY { get; set; }//13
        public string UPDATEDDATE { get; set; }//14
     
    }
}