using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.AdminUser
{
    public class AdminUserViewModel
    {
        public int USERID { get; set; } //1
        public string USERNAME { get; set; } //2
        public string EMAIL { get; set; }//3
        public string LASTLOGIN { get; set; }//4
        public string LOGINLOCKDATE { get; set; }//5
        public int LOGINRETRYCOUNT { get; set; }//6
        public string CREATEDBY { get; set; }//7
        public string CREATEDDATE { get; set; }//8
        public string UPDATEDBY { get; set; }//9
        public string UPDATEDDATE { get; set; }//10
        public string AVATARURL { get; set; }//11


    }
}