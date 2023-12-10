using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Models.Appointment.MakeAppointment
{
    public class DoctorViewModel
    {
        public int doctorID { get; set; }
        public int scheduleID { get; set; }
        public string doctorName { get; set; }
    }
}