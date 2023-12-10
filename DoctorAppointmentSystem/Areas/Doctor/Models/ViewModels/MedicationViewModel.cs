using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Doctor.Models.ViewModels
{
    public class MedicationViewModel
    {
        public int appointmentID { get; set; }
        public int prescriptionID { get; set; }
        public string patientName { get; set; }
        public string drug { get; set; }
        public string note { get; set; }
        public string medicationDays { get; set; }
        public string quantity { get; set; }
        public int frequency { get; set; }
        public int unit { get; set; }
    }
}