using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.AdminProfile
{
    public class AdminProfileViewModel
    {
        public string fullName { get; set; }
        public string email { get; set; }
        public string profilePicture { get; set; }
       
    }
}