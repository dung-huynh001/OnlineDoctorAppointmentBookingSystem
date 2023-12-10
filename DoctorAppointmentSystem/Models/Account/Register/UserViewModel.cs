using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Models.Account.Register
{
    public class UserViewModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public int passwordRecoveryQue1 { get; set; }
        public string passwordRecoveryAns1 { get; set; }
        public int passwordRecoveryQue2 { get; set; }
        public string passwordRecoveryAns2 { get; set; }
        public int passwordRecoveryQue3 { get; set; }
        public string passwordRecoveryAns3 { get; set; }
        public string profilePicture { get; set; }
    }
}