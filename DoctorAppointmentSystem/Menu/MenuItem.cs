using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Menu
{
    public class MenuItem
    {
        public MenuItem(string title, string icon, bool active, string area, string controller, string action)
        {
            this.title = title;
            this.icon = icon;
            this.active = active;
            this.area = area;
            this.controller = controller;
            this.action = action;
        }
        public string title { get; set; }
        public string icon { get; set; }
        public bool active { get; set; }
        public string area { get; set; }
        public string controller { get; set; }
        public string action { get; set; }


    }
}