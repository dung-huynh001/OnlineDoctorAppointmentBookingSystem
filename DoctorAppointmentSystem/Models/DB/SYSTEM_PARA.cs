//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DoctorAppointmentSystem.Models.DB
{
    using System;
    using System.Collections.Generic;
    
    public partial class SYSTEM_PARA
    {
        public int ID { get; set; }
        public string PARAID { get; set; }
        public string GROUPID { get; set; }
        public string PARAVAL { get; set; }
        public string NOTE { get; set; }
        public string CREATEDBY { get; set; }
        public Nullable<System.DateTime> CREATEDDATE { get; set; }
        public string UPDATEDBY { get; set; }
        public Nullable<System.DateTime> UPDATEDDATE { get; set; }
        public Nullable<bool> DELETEDFLAG { get; set; }
    }
}
