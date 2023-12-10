using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.AdminUser
{
    public class AdminUserCreateModel
    {
        public string USERNAME { get; set; } 
        public string PASSWORD { get; set; } 
        public string EMAIL { get; set; }
    }
}