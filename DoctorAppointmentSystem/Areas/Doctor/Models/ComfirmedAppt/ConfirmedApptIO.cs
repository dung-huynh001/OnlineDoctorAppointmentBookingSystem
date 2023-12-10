using DoctorAppointmentSystem.Areas.Doctor.Models.ViewModels;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Doctor.Models.ComfirmedAppt
{
    public class ConfirmedApptIO
    {
        public List<AppointmentViewModel> GetConfirmedAppts(int doctorID, string appointmentStatus)
        {
            List<AppointmentViewModel> results = new List<AppointmentViewModel>();
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    List<APPOINTMENT> appointmentList = dbContext.APPOINTMENT
                        .Where(a => a.DOCTORID.Equals(doctorID)
                        && a.APPOIMENTSTATUS.ToLower().Equals(appointmentStatus)
                        && a.DELETEDFLAG == false)
                        .ToList();

                    if (appointmentList.Count == 0)
                    {
                        throw new Exception();
                    }

                    foreach (APPOINTMENT appointment in appointmentList)
                    {
                        PATIENT patient = dbContext.PATIENT.Find(appointment.PATIENTID);

                        if (patient == null)
                        {
                            throw new Exception();
                        }

                        AppointmentViewModel avm = new AppointmentViewModel();

                        avm.appointmentID = appointment.APPOINTMENTID;
                        avm.patientName = patient.PATIENTNAME;

                        string patientGender = SystemParaHelper.GetParaval(patient.PATIENTGENDER);
                        avm.patientGender = patientGender;
                        avm.patientDateOfBirth = patient.PATIENTDATEOFBIRTH.ToString("MM-dd-yyyy");
                        avm.dateOfConsultation = appointment.DATEOFCONSULTATION.ToString(@"MM-dd-yyyy HH\:mm tt");
                        avm.consultationTime = appointment.DATEOFCONSULTATION.ToString(@"hh\:mm tt");
                        avm.consultationDay = appointment.DATEOFCONSULTATION.DayOfWeek.ToString();
                        avm.appointmentDate = appointment.APPOINTMENTDATE.Value.ToString("MM-dd-yyyy");
                        avm.appointmentTime = appointment.APPOINTMENTDATE.Value.ToString(@"hh\:mm tt");
                        avm.appointmentDay = appointment.APPOINTMENTDATE.Value.DayOfWeek.ToString();
                        avm.appointmentStatus = appointment.APPOIMENTSTATUS;

                        results.Add(avm);
                    }
                }

                return results;
            }
            catch (Exception ex)
            {
                string username = GetInfo.Username;
                string sEventCatg = GetInfo.GetUserType(username).ToUpper() + "PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "LoadData";
                string sEventType = "S";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return results;
            }
        }

        public MemberDetailViewModel GetMemberDetails(int appointmentID)
        {
            MemberDetailViewModel results = new MemberDetailViewModel();
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    APPOINTMENT appointment = dbContext.APPOINTMENT.Find(appointmentID);
                    if (appointment == null)
                    {
                        throw new Exception();
                    }
                    PATIENT patient = dbContext.PATIENT.Find(appointment.PATIENTID);
                    if (patient == null)
                    {
                        throw new Exception();
                    }
                    USER user = dbContext.USER.Find(patient.USERID);
                    results.appointmentID = appointment.APPOINTMENTID;
                    results.fullName = patient.PATIENTNAME;
                    results.dateOfBirth = patient.PATIENTDATEOFBIRTH.ToString("MM-dd-yyyy");
                    string gender = SystemParaHelper.GetParaval(patient.PATIENTGENDER);
                    results.gender = gender;
                    results.nationalID = patient.PATIENTNATIONALID;
                    string avatarUrl = GetInfo.GetImgPath(user.USERNAME);
                    results.avatar = avatarUrl;
                    results.email = user.EMAIL;
                    results.phoneNumber = patient.PATIENTMOBILENO;
                    results.address = patient.PATIENTADDRESS;
                    string modeOfConsultant = SystemParaHelper.GetParaval(appointment.MODEOFCONSULTANT);
                    results.modeOfConsutant = modeOfConsultant;
                    string consultantType = SystemParaHelper.GetParaval(appointment.CONSULTANTTYPE);
                    results.consultantType = consultantType;
                    results.dateOfConsultation = appointment.DATEOFCONSULTATION.Date.ToString("MM-dd-yyyy");
                    results.consultationTime = appointment.DATEOFCONSULTATION.TimeOfDay.ToString(@"hh\:mm");
                    results.appointmentDate = appointment.APPOINTMENTDATE.Value.Date.ToString("MM-dd-yyyy");
                    results.appointmentTime = appointment.APPOINTMENTDATE.Value.TimeOfDay.ToString(@"hh\:mm");
                    results.symtoms = appointment.SYMTOMS;
                    results.existingIllness = appointment.EXISTINGILLNESS;
                    results.drugAlergies = appointment.DRUGALLERGIES;

                    return results;
                }
            }
            catch (Exception ex)
            {
                string username = GetInfo.Username;
                string sEventCatg = GetInfo.GetUserType(username).ToUpper() + "PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "GetMemberDetails";
                string sEventType = "S";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return results;
            }
        }

        public bool ChangeAppointmentDate(int id, DateTime appointmentDate, out string message)
        {
            bool success = false;
            try
            {
                if (appointmentDate.CompareTo(DateTime.Now) <= 0)
                {
                    message = "Appointment date have to greater than current date.";
                    return success;
                }

                using (DBContext _dbContext = new DBContext())
                {
                    APPOINTMENT apptCondition = _dbContext.APPOINTMENT
                        .FirstOrDefault(a => a.APPOINTMENTDATE.Value.Equals(appointmentDate) && a.APPOINTMENTID != id);

                    if (apptCondition != null)
                    {
                        message = "You have an appointment coming up during that time.";
                        return success;
                    }

                    APPOINTMENT appointment = _dbContext.APPOINTMENT.Find(id);
                    if (appointment == null)
                    {
                        message = "Appointment date is required.";
                        return success;
                    }
                    else
                    {
                        int doctorId = appointment.DOCTORID;

                        List<SCHEDULE> schedules = _dbContext.SCHEDULE
                            .Where(s => s.DOCTORID.Equals(doctorId)
                            && s.WORKINGDAY.Equals(appointmentDate.Date)
                            && s.SHIFTTIME.CompareTo(appointmentDate.TimeOfDay) <= 0
                            && s.BREAKTIME.CompareTo(appointmentDate.TimeOfDay) > 0)
                            .ToList();

                        if (schedules.Count == 0)
                        {
                            message = "There are no schedule during this time.";
                            return success;
                        }



                        appointment.APPOINTMENTDATE = appointmentDate;
                        appointment.UPDATEDBY = GetInfo.Username;
                        appointment.UPDATEDDATE = DateTime.Now;

                        _dbContext.SaveChanges();

                        success = true;
                    }

                    message = "Appointment Date has been updated.";
                    return success;
                }
            }
            catch (Exception ex)
            {
                string username = GetInfo.Username;
                string sEventCatg = GetInfo.GetUserType(username).ToUpper() + "PORTAL";
                string sEventMsg = ex.Message;
                string sEventSrc = nameof(ChangeAppointmentDate);
                string sEventType = "U";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);

                message = "Error! An error occurred. Please try again later";
                return success;
            }
        }

        public DiagnosisViewModel GetDiagnosis(int id)
        {
            DiagnosisViewModel result = new DiagnosisViewModel();
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    APPOINTMENT appointment = dbContext.APPOINTMENT.Find(id);
                    if (appointment == null)
                    {
                        throw new Exception();
                    }
                    else
                    {
                        result.appointmentID = appointment.APPOINTMENTID;
                        result.diagnosis = appointment.DIAGNOSIS;
                        result.adviceToPatient = appointment.ADVICETOPATIENT;
                        result.caseNote = appointment.CASENOTE;
                    }
                }
            }
            catch (Exception ex)
            {
                string username = GetInfo.Username;
                string sEventCatg = GetInfo.GetUserType(username).ToUpper() + "PORTAL";
                string sEventMsg = ex.Message;
                string sEventSrc = nameof(GetDiagnosis);
                string sEventType = "S";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
            }
            return result;
        }

        public List<MedicationViewModel> GetMedication(int id)
        {
            List<MedicationViewModel> results = new List<MedicationViewModel>();
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    List<APPOINTMENT_PRESCRIPTION> apptNotes = dbContext.APPOINTMENT_PRESCRIPTION
                        .Where(a => a.APPOINTMENTID.Equals(id))
                        .ToList();

                    if (apptNotes.Count == 0)
                    {
                        throw new Exception();
                    }
                    else
                    {
                        foreach (APPOINTMENT_PRESCRIPTION apptNote in apptNotes)
                        {
                            PRESCRIPTION prescription = dbContext.PRESCRIPTION.Find(apptNote.PRECRIPTIONID);
                            if (prescription == null)
                            {
                                throw new Exception();
                            }

                            MedicationViewModel medication = new MedicationViewModel();
                            medication.appointmentID = apptNote.APPOINTMENTID;
                            medication.prescriptionID = apptNote.PRECRIPTIONID;
                            medication.drug = prescription.DRUG;
                            medication.frequency = prescription.FREQUENCY;
                            medication.medicationDays = prescription.MEDICATIONDAYS;
                            medication.quantity = prescription.QUANTITY;

                            results.Add(medication);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string username = GetInfo.Username;
                string sEventCatg = GetInfo.GetUserType(username).ToUpper() + "PORTAL";
                string sEventMsg = ex.Message;
                string sEventSrc = nameof(GetMedication);
                string sEventType = "S";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
            }
            return results;
        }

        public bool SaveMedication(int id, List<MedicationViewModel> data)
        {
            bool success = false;
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    if (data.Count == 0)
                    {
                        throw new Exception();
                    }
                    else
                    {
                        var currPressList = dbContext.PRESCRIPTION
                            .Join(dbContext.APPOINTMENT_PRESCRIPTION,
                            p => p.PRECRIPTIONID, a => a.PRECRIPTIONID, (p, a) => p)
                            .Where(p => p.DELETEDFLAG == false)
                            .ToList();

                        if (currPressList.Count != 0)
                        {
                            foreach (var pres in currPressList)
                            {
                                pres.UPDATEDBY = GetInfo.Username;
                                pres.UPDATEDDATE = DateTime.Now;
                                pres.DELETEDFLAG = true;

                                dbContext.SaveChanges();
                            }
                        }

                        foreach (var item in data)
                        {
                            APPOINTMENT appointment = dbContext.APPOINTMENT.Find(item.appointmentID);
                            if (appointment == null)
                            {
                                throw new Exception();
                            }

                            int patientId = appointment.PATIENTID;
                            PATIENT patient = dbContext.PATIENT.Find(patientId);
                            if (patient == null)
                            {
                                throw new Exception();
                            }

                            PRESCRIPTION prescription = new PRESCRIPTION();
                            prescription.PATIENTNAME = patient.PATIENTNAME;
                            prescription.DRUG = item.drug;
                            prescription.MEDICATIONDAYS = item.medicationDays;
                            prescription.QUANTITY = item.quantity;
                            prescription.NOTE = item.note;
                            prescription.FREQUENCY = item.frequency;
                            prescription.UNIT = item.unit;
                            prescription.CREATEDBY = GetInfo.Username;
                            prescription.UPDATEDBY = GetInfo.Username;
                            prescription.CREATEDDATE = DateTime.Now;
                            prescription.UPDATEDDATE = DateTime.Now;



                            dbContext.PRESCRIPTION.Add(prescription);

                            APPOINTMENT_PRESCRIPTION apptPres = new APPOINTMENT_PRESCRIPTION();
                            apptPres.PRESCRIPTION = prescription;
                            apptPres.APPOINTMENTID = item.appointmentID;

                            dbContext.APPOINTMENT_PRESCRIPTION.Add(apptPres);
                            dbContext.SaveChanges();
                        }
                    }
                    success = true;
                }
            }
            catch (Exception ex)
            {
                string username = GetInfo.Username;
                string sEventCatg = GetInfo.GetUserType(username).ToUpper() + "PORTAL";
                string sEventMsg = ex.Message;
                string sEventSrc = nameof(SaveMedication);
                string sEventType = "U";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
            }
            return success;
        }

        public bool SaveDiagnosis(DiagnosisViewModel notes)
        {
            bool success = false;
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    if (String.IsNullOrEmpty(notes.diagnosis) || String.IsNullOrEmpty(notes.adviceToPatient))
                    {
                        throw new Exception();
                    }
                    else
                    {
                        APPOINTMENT appointment = dbContext.APPOINTMENT.Find(notes.appointmentID);

                        if (appointment == null)
                        {
                            throw new Exception();
                        }

                        appointment.DIAGNOSIS = notes.diagnosis;
                        appointment.CASENOTE = notes.caseNote;
                        appointment.ADVICETOPATIENT = notes.adviceToPatient;
                        appointment.UPDATEDBY = GetInfo.Username;
                        appointment.UPDATEDDATE = DateTime.Now;

                        dbContext.SaveChanges();

                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                string username = GetInfo.Username;
                string sEventCatg = GetInfo.GetUserType(username).ToUpper() + "PORTAL";
                string sEventMsg = ex.Message;
                string sEventSrc = nameof(SaveDiagnosis);
                string sEventType = "U";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
            }
            return success;
        }

        public bool MarkAsCompleted(int id, List<MedicationViewModel> data, DiagnosisViewModel notes)
        {
            bool success = false;
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    if (SaveMedication(id, data) && SaveDiagnosis(notes))
                    {
                        APPOINTMENT appointment = dbContext.APPOINTMENT.Find(id);
                        if (appointment == null)
                        {
                            throw new Exception();
                        }
                        else
                        {
                            appointment.APPOIMENTSTATUS = "Completed";
                            appointment.UPDATEDBY = GetInfo.Username;
                            appointment.UPDATEDDATE = DateTime.Now;
                            appointment.CLOSEDBY = GetInfo.Username;
                            appointment.CLOSEDDATE = DateTime.Now;

                            dbContext.SaveChanges();
                        }

                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                string username = GetInfo.Username;
                string sEventCatg = GetInfo.GetUserType(username).ToUpper() + "PORTAL";
                string sEventMsg = ex.Message;
                string sEventSrc = nameof(MarkAsCompleted);
                string sEventType = "U";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
            }
            return success;
        }

        public bool CancelAppointment(int id)
        {
            bool success = false;

            try
            {
                using (DBContext _dbContext = new DBContext())
                {
                    APPOINTMENT appointment = _dbContext.APPOINTMENT.Find(id);
                    if (appointment == null)
                    {
                        throw new Exception();
                    }
                    else
                    {
                        appointment.APPOIMENTSTATUS = "Cancel";
                        appointment.UPDATEDBY = GetInfo.Username;
                        appointment.UPDATEDDATE = DateTime.Now;

                        _dbContext.SaveChanges();

                        success = true;
                    }
                    return success;
                }
            }
            catch (Exception ex)
            {
                string username = GetInfo.Username;
                string sEventCatg = GetInfo.GetUserType(username).ToUpper() + "PORTAL";
                string sEventMsg = ex.Message;
                string sEventSrc = nameof(CancelAppointment);
                string sEventType = "U";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);

                return success;
            }
        }
    }
}