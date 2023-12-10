using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Menu
{
    public class PatientMenu
    {
        public List<MenuItem> RenderMenu(string idActive)
        {
            List<MenuItem> menu = new List<MenuItem>();
            menu.Add(new MenuItem("Home", "fas fa-home", false, "", "Home", "Index"));
            menu.Add(new MenuItem("Schedule of Doctors", "fa-regular fa-calendar-days", false, "", "ScheduleOfDoctors", "Index"));
            menu.Add(new MenuItem("Make Appointment", "fa-solid fa-file-medical", false, "", "Appointment", "MakeAppointment"));
            menu.Add(new MenuItem("Appointment History", "fa-solid fa-list-check", false, "", "Appointment", "History"));
            menu.Add(new MenuItem("Log out", "fas fa-sign-out-alt", false, "", "Account", "Logout"));

            foreach(MenuItem item in menu)
            {
                if (item.title.Equals(idActive))
                {
                    item.active = true;
                    break;
                }
            }
            return menu;
        }
    }
}