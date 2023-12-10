using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.Validation
{
    public class ValidationResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }

    }
}