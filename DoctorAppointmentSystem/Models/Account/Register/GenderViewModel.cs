using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Models.Account.Register
{
    public class GenderViewModel
    {
        public GenderViewModel()
        {
        }

        public GenderViewModel(int iD, string pARAID, string gROUPID, string pARAVAL)
        {
            id = iD;
            paraid = pARAID;
            groupid = gROUPID;
            paraval = pARAVAL;
        }
        public int id { get; set; }
        public string paraid { get; set; }
        public string groupid { get; set; }
        public string paraval { get; set; }
    }
}