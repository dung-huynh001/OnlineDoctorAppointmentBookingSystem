using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.UserManage
{
    public class ResetPasswordModel
    {
        public int USERID { get; set; }
        public string PASSWORD { get; set; }
    }
}