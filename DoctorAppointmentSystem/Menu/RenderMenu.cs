using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Menu
{
    public class RenderMenu
    {
        public static List<MenuItem> RenderAdminMenu(string index)
        {
            AdminMenu menu = new AdminMenu();
            return menu.RenderMenu(index);
        }
        public static List<MenuItem> RenderDoctorMenu(string index)
        {
            DoctorMenu menu = new DoctorMenu();
            return menu.RenderMenu(index);
        }
        public static List<MenuItem> RenderPatientMenu(string index)
        {
            PatientMenu menu = new PatientMenu();
            return menu.RenderMenu(index);
        }
    }
}