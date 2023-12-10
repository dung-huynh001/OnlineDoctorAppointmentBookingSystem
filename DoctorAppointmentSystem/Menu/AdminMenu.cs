using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Menu
{
    public class AdminMenu
    {
        public List<MenuItem> RenderMenu(string idActive)
        {
            List<MenuItem> menu = new List<MenuItem>();
            menu.Add(new MenuItem("Dashboard", "fa-solid fa-house", false, "Admin", "Manage", "Index"));
            menu.Add(new MenuItem("User Management", "fa-solid fa-user", false, "Admin", "UserManage", "Index"));
            menu.Add(new MenuItem("Doctor Management", "fa-solid fa-user-nurse", false, "Admin", "DoctorManage", "Index"));
            menu.Add(new MenuItem("Patient Management", "fa-solid fa-wheelchair", false, "Admin", "PatientManage", "Index"));
            menu.Add(new MenuItem("Admin Management", "fa-solid fa-person-military-pointing", false, "Admin", "AdminUserManage", "Index"));
            menu.Add(new MenuItem("Doctor Schedules", "fa-solid fa-user-clock", false, "Admin", "DoctorScheduleManage", "Index"));
            menu.Add(new MenuItem("Appointments", "fa-solid fa-file-medical", false, "Admin", "AppointmentManage", "Index"));
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