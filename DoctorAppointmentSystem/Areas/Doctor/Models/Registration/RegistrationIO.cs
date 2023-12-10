using DoctorAppointmentSystem.Areas.Doctor.Models.ViewModels;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Doctor.Models.Registration
{
    public class RegistrationIO
    {
        public List<CreateMemberViewModel> SearchMembers(string nationalID, string name)
        {
            List<CreateMemberViewModel> results = new List<CreateMemberViewModel>();
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    if (String.IsNullOrWhiteSpace(name) && String.IsNullOrWhiteSpace(nationalID))
                    {
                        throw new Exception();
                    }
                    var members = dbContext.PATIENT
                        .Join(dbContext.USER,
                        p => p.USERID,
                        u => u.USERID, (p, u) => new { p, u })
                        .Where(c => c.p.PATIENTNATIONALID.Contains(nationalID)
                        && c.p.PATIENTNAME.Contains(name)
                        && c.u.DELETEDFLAG == false
                        && c.p.DELETEDFLAG == false)
                        .ToList();
                    if (members.Count == 0)
                    {
                        throw new Exception();
                    }
                    foreach (var member in members)
                    {
                        CreateMemberViewModel pvm = new CreateMemberViewModel();
                        var patient = member.p;
                        var user = member.u;

                        pvm.id = patient.PATIENTID;
                        pvm.name = patient.PATIENTNAME;
                        string gender = SystemParaHelper.GetParaval(patient.PATIENTGENDER);
                        pvm.gender = gender;
                        pvm.dateOfBirth = patient.PATIENTDATEOFBIRTH.ToString("yyyy-MM-dd");
                        pvm.nationalID = patient.PATIENTNATIONALID;
                        pvm.mobile = patient.PATIENTMOBILENO;
                        pvm.email = user.EMAIL;
                        pvm.address = patient.PATIENTADDRESS;

                        results.Add(pvm);
                    }
                }
            }
            catch (Exception ex)
            {
                string username = GetInfo.Username;
                string sEventCatg = GetInfo.GetUserType(username).ToUpper() + "PORTAL";
                string sEventMsg = ex.Message;
                string sEventSrc = nameof(SearchMembers);
                string sEventType = "S";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
            }
            return results;
        }

        public bool CreateNewMember(CreateMemberViewModel member, out string message)
        {
            bool success = false;
            message = "";
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    string email = dbContext.USER.FirstOrDefault(u => u.EMAIL.Equals(member.email)) == null ? member.email : null;
                    string username = dbContext.USER.FirstOrDefault(u => u.EMAIL.Equals(member.email)) == null ? member.email : null;
                    string nationalID = dbContext.USER.FirstOrDefault(u => u.EMAIL.Equals(member.email)) == null ? member.email : null;

                    if (email == null)
                    {
                        message = "Email address already exists.";
                        throw new Exception("Email address already exists.");
                    }
                    else if (username == null)
                    {
                        message = "Username already exists.";
                        throw new Exception("Username already exists.");
                    }
                    else if (nationalID == null)
                    {
                        message = "NationalID address already exists.";
                        throw new Exception("NationalID address already exists.");
                    }
                    else
                    {
                        USER user = new USER();
                        user.ROLEID = 1303;
                        user.USERNAME = member.username;
                        user.PASSWORDHASH = PasswordHelper.HashPassword(member.password);
                        user.EMAIL = member.email;
                        user.USERTYPE = "Patient";
                        user.STATUS = true;
                        user.CREATEDBY = GetInfo.Username;
                        user.CREATEDDATE = DateTime.Now;
                        user.UPDATEDBY = GetInfo.Username;
                        user.UPDATEDDATE = DateTime.Now;
                        user.DELETEDFLAG = false;

                        dbContext.USER.Add(user);

                        PATIENT patient = new PATIENT();
                        patient.USER = user;
                        patient.PATIENTNAME = member.name;
                        patient.PATIENTDATEOFBIRTH = Convert.ToDateTime(member.dateOfBirth);
                        patient.PATIENTGENDER = Convert.ToInt32(member.gender);
                        patient.PATIENTNATIONALID = member.nationalID;
                        patient.PATIENTMOBILENO = member.mobile;
                        patient.PATIENTADDRESS = member.address;
                        patient.CREATEDBY = GetInfo.Username;
                        patient.CREATEDDATE = DateTime.Now;
                        patient.UPDATEDBY = GetInfo.Username;
                        patient.UPDATEDDATE = DateTime.Now;
                        patient.DELETEDFLAG = false;

                        dbContext.PATIENT.Add(patient);

                        DateTime appointmentDate = member.appointmentDate.Add(member.appointmentTime);
                        int doctorID = GetInfo.GetDoctorID(GetInfo.Username);
                        SCHEDULE schedule = dbContext.SCHEDULE
                            .FirstOrDefault(s => s.DOCTORID.Equals(doctorID)
                            && s.WORKINGDAY.Equals(member.appointmentDate)
                            && s.SHIFTTIME.CompareTo(member.appointmentTime) <= 0
                            && s.BREAKTIME.CompareTo(member.appointmentTime) > 0);

                        if (schedule == null)
                        {
                            message = "You have no schedule during this time.";
                            throw new Exception();
                        }

                        APPOINTMENT appointment = new APPOINTMENT();
                        appointment.PATIENT = patient;
                        appointment.DOCTORID = doctorID;
                        appointment.SCHEDULEID = schedule.SCHEDULEID;
                        appointment.CONSULTANTTYPE = member.consultantType;
                        appointment.MODEOFCONSULTANT = member.modeOfConsultant;

                        appointment.APPOINTMENTDATE = appointmentDate;
                        appointment.DATEOFCONSULTATION = appointmentDate;
                        appointment.APPOIMENTSTATUS = "Confirm";
                        appointment.CREATEDBY = GetInfo.Username;
                        appointment.CREATEDDATE = DateTime.Now;
                        appointment.UPDATEDBY = GetInfo.Username;
                        appointment.UPDATEDDATE = DateTime.Now;
                        appointment.DELETEDFLAG = false;

                        dbContext.APPOINTMENT.Add(appointment);

                        dbContext.SaveChanges();

                        success = true;
                        message = "Created new member successfully.";
                    }

                }
            }
            catch (Exception ex)
            {
                string username = GetInfo.Username;
                string sEventCatg = GetInfo.GetUserType(username).ToUpper() + " PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = nameof(CreateNewMember);
                string sEventType = "C";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
            }

            return success;
        }

        public CreateMemberViewModel LoadAppointmentData(int id)
        {
            CreateMemberViewModel results = new CreateMemberViewModel();

            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    PATIENT patient = dbContext.PATIENT
                        .Where(p => p.PATIENTID.Equals(id))
                        .Include("USER").FirstOrDefault();

                    if(patient == null)
                    {
                        throw new Exception();
                    }

                    results.id = patient.PATIENTID;
                    results.name = patient.PATIENTNAME;
                    results.dateOfBirth = patient.PATIENTDATEOFBIRTH.ToString("yyyy-MM-dd");
                    results.gender = patient.PATIENTGENDER.ToString();
                    results.nationalID = patient.PATIENTNATIONALID;
                    results.mobile = patient.PATIENTMOBILENO;
                    results.email = patient.USER.EMAIL;
                    results.address = patient.PATIENTADDRESS;
                }
            }
            catch (Exception ex)
            {
                string username = GetInfo.Username;
                string sEventCatg = GetInfo.GetUserType(username).ToUpper() + " PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = nameof(LoadAppointmentData);
                string sEventType = "S";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
            }

            return results;
        }

        public bool MakeAppointment(CreateMemberViewModel member, out string message)
        {
            bool success = false;
            message = "";
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    DateTime appointmentDate = member.appointmentDate.Add(member.appointmentTime);
                    int doctorID = GetInfo.GetDoctorID(GetInfo.Username);
                    SCHEDULE schedule = dbContext.SCHEDULE
                        .FirstOrDefault(s => s.DOCTORID.Equals(doctorID)
                        && s.WORKINGDAY.Equals(member.appointmentDate)
                        && s.SHIFTTIME.CompareTo(member.appointmentTime) <= 0
                        && s.BREAKTIME.CompareTo(member.appointmentTime) > 0);

                    if(schedule == null)
                    {
                        message = "You have no schedule during this time.";
                        throw new Exception();
                    }

                    APPOINTMENT appointment = new APPOINTMENT();
                    appointment.PATIENTID = member.id;
                    appointment.DOCTORID = doctorID;
                    appointment.SCHEDULEID = schedule.SCHEDULEID;
                    appointment.CONSULTANTTYPE = member.consultantType;
                    appointment.MODEOFCONSULTANT = member.modeOfConsultant;

                    appointment.APPOINTMENTDATE = appointmentDate;
                    appointment.DATEOFCONSULTATION = appointmentDate;
                    appointment.APPOIMENTSTATUS = "Confirm";
                    appointment.CREATEDBY = GetInfo.Username;
                    appointment.CREATEDDATE = DateTime.Now;
                    appointment.UPDATEDBY = GetInfo.Username;
                    appointment.UPDATEDDATE = DateTime.Now;
                    appointment.DELETEDFLAG = false;

                    dbContext.APPOINTMENT.Add(appointment);

                    dbContext.SaveChanges();

                    success = true;
                    message = "Create a new appointment successfully.";
                }
            }
            catch (Exception ex)
            {
                string username = GetInfo.Username;
                string sEventCatg = GetInfo.GetUserType(username).ToUpper() + " PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = nameof(CreateNewMember);
                string sEventType = "C";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
            }

            return success;
        }
    }
}