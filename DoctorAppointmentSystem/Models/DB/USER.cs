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
    
    public partial class USER
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public USER()
        {
            this.DOCTOR = new HashSet<DOCTOR>();
            this.PATIENT = new HashSet<PATIENT>();
        }
    
        public int USERID { get; set; }
        public int ROLEID { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORDHASH { get; set; }
        public string EMAIL { get; set; }
        public Nullable<System.DateTime> LASTLOGIN { get; set; }
        public string USERTYPE { get; set; }
        public string AVATARURL { get; set; }
        public Nullable<int> PASSWORDRECOVERYQUE1 { get; set; }
        public Nullable<int> PASSWORDRECOVERYQUE2 { get; set; }
        public Nullable<int> PASSWORDRECOVERYQUE3 { get; set; }
        public string PASSWORDRECOVERYANS1 { get; set; }
        public string PASSWORDRECOVERYANS2 { get; set; }
        public string PASSWORDRECOVERYANS3 { get; set; }
        public bool STATUS { get; set; }
        public Nullable<int> LOGINRETRYCOUNT { get; set; }
        public Nullable<System.DateTime> LOGINLOCKDATE { get; set; }
        public string CREATEDBY { get; set; }
        public Nullable<System.DateTime> CREATEDDATE { get; set; }
        public string UPDATEDBY { get; set; }
        public Nullable<System.DateTime> UPDATEDDATE { get; set; }
        public bool DELETEDFLAG { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DOCTOR> DOCTOR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PATIENT> PATIENT { get; set; }
        public virtual ROLE ROLE { get; set; }
    }
}