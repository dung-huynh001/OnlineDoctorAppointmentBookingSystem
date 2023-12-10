using DoctorAppointmentSystem.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoctorAppointmentSystem.Areas.Admin.Models.UserManage
{
    public class UserViewModel 
    {
        public int USERID { get; set; } //1
        public string FULLNAME { get; set; } //2
        public string USERNAME { get; set; } //3
        public string USERTYPE { get; set; } //4
        public string GENDER { get; set; }//5
        public string MOBILENO { get; set; }//6
        public string EMAIL { get; set; }//7
        public string LASTLOGIN { get; set; }//8
        public string LOGINLOCKDATE { get; set; } //9
        public int LOGINRETRYCOUNT { get; set; } //10
        public string CREATEDBY { get; set; }//11
        public string CREATEDDATE { get; set; }//12
        public string UPDATEDBY { get; set; }//13
        public string UPDATEDDATE { get; set; }//14
        public string AVATARURL { get; set; }//15
        public bool STATUS { get; set; }//16

    }
}