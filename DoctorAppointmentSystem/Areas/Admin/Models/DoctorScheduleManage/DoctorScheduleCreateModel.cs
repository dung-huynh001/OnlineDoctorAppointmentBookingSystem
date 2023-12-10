using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.DoctorScheduleManage
{
    public class DoctorScheduleCreateModel
    {
        public int DOCTORID { get; set; }
        public DateTime WORKINGDAY { get; set; }
        public TimeSpan SHIFTTIME { get; set; }
        public TimeSpan BREAKTIME { get; set; }
        public int CONSULTANTTIME { get; set; }

    }
}