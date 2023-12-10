using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.DoctorScheduleManage
{
    public class DoctorScheduleViewEditModel
    {
        public int SCHEDULEID { get; set; }
        public int DOCTORID { get; set; }
        public string WORKINGDAY { get; set; }
        public string SHIFTTIME { get; set; }
        public string BREAKTIME { get; set; }
        public int CONSULTANTTIME { get; set; }
    }
}