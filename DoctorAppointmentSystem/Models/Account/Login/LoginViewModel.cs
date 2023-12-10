using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Models.Account.Login
{
    public class LoginViewModel
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}