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
    
    public partial class PATIENT
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public PATIENT()
        {
            this.APPOINTMENT = new HashSet<APPOINTMENT>();
        }
    
        public int PATIENTID { get; set; }
        public int USERID { get; set; }
        public string PATIENTNAME { get; set; }
        public string PATIENTNATIONALID { get; set; }
        public int PATIENTGENDER { get; set; }
        public string PATIENTMOBILENO { get; set; }
        public System.DateTime PATIENTDATEOFBIRTH { get; set; }
        public string PATIENTADDRESS { get; set; }
        public string CREATEDBY { get; set; }
        public Nullable<System.DateTime> CREATEDDATE { get; set; }
        public string UPDATEDBY { get; set; }
        public Nullable<System.DateTime> UPDATEDDATE { get; set; }
        public bool DELETEDFLAG { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<APPOINTMENT> APPOINTMENT { get; set; }
        public virtual USER USER { get; set; }
    }
}
