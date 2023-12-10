using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Doctor.Models.Home
{
    public class EventViewModel
    {
        public int id { get; set; }

        public string title { get; set; }
        public string description { get; set; }
        public string start { get; set; }

        public string end { get; set; }

        public bool allDay { get; set; }

        public string url { get; set; }
        public string color { get; set; }
    }
}