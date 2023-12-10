using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.DoctorSchedule
{
    public class DoctorScheduleViewModel
    {
        public int SCHEDULEID { get; set; }//1
        public int DOCTORID { get; set; }//2
        public string DOCTORNAME { get; set; }//3
        public string WORKINGDAY { get; set; }//4
        public string SHIFTTIME { get; set; }//5
        public string BREAKTIME { get; set; }//6
        public string CONSULTANTTIME { get; set; }//7
        public int APPOINTMENTNUM { get; set; }//8
        public string CREATEDBY { get; set; }//9
        public string CREATEDDATE { get; set; }//10
        public string UPDATEDBY { get; set; }//11
        public string UPDATEDDATE { get; set; }//12
        
    }
}