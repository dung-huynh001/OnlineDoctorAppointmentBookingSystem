using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Doctor.Models.ViewModels
{
    public class DiagnosisViewModel
    {
        public int appointmentID { get; set; }
        public string diagnosis { get; set; }
        public string caseNote { get; set; }
        public string adviceToPatient { get; set; }
        
    }
}