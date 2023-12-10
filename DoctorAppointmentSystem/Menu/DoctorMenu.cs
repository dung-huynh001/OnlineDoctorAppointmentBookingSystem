using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Menu
{
    public class DoctorMenu
    {
        public List<MenuItem> RenderMenu(string idActive) {
            List<MenuItem> menu = new List<MenuItem>();
            menu.Add(new MenuItem("Dashboard", "fas fa-home", false, "Doctor", "Home", "Index"));
            menu.Add(new MenuItem("Appointments", "fa-regular fa-calendar-days", false, "Doctor", "Appointments", "Index"));
            menu.Add(new MenuItem("Appointments Confirmed", "fa-regular fa-calendar-check", false, "Doctor", "ConfirmedAppt", "Index"));
            menu.Add(new MenuItem("Completed Appointments", "fa-regular fa-calendar", false, "Doctor", "CompletedAppt", "Index"));
            menu.Add(new MenuItem("Appointments Cancelled", "fa-regular fa-calendar-xmark", false, "Doctor", "CancelledAppt", "Index"));
            menu.Add(new MenuItem("Registration", "fa-regular fa-calendar-plus", false, "Doctor", "Registration", "Index"));
            menu.Add(new MenuItem("Log out", "fas fa-sign-out-alt", false, "", "Account", "Logout"));

            foreach (MenuItem item in menu)
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