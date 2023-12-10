using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.AdminUser
{
    public class AdminUserViewEditModel
    {
        public int USERID { get; set; }
        public string USERNAME { get; set; } 
        public string EMAIL { get; set; }

    }
}