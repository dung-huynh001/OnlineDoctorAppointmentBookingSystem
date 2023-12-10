using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Models.Account.AuthenQuestion
{
    public class AuthenQuestionModel
    {
        public string username { get; set; }
        public int passwordRecoveryQue1 { get; set; }
        public string passwordRecoveryAns1 { get; set; }
        public int passwordRecoveryQue2 { get; set; }
        public string passwordRecoveryAns2 { get; set; }
        public int passwordRecoveryQue3 { get; set; }
        public string passwordRecoveryAns3 { get; set; }
    }
}