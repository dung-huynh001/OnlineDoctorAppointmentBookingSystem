
using DoctorAppointmentSystem.Areas.Admin;
using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Models.Account.AuthenQuestion;
using DoctorAppointmentSystem.Models.Account.Register;
using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Models.Account
{
    public class RegisterIO
    {
        private readonly DBContext dbContext;

        public RegisterIO()
        {
            this.dbContext = new DBContext();
        }

        public bool VerifyUserInfo(UserViewModel user, out string message)
        {
            user.passwordRecoveryAns1 = user.passwordRecoveryAns1 != null ? user.passwordRecoveryAns1.Trim() : user.passwordRecoveryAns1;
            user.passwordRecoveryAns2 = user.passwordRecoveryAns2 != null ? user.passwordRecoveryAns2.Trim() : user.passwordRecoveryAns2;
            user.passwordRecoveryAns3 = user.passwordRecoveryAns3 != null ? user.passwordRecoveryAns3.Trim() : user.passwordRecoveryAns3;

            bool success = false;
            message = "";

            // Check if the username is already in use
            bool username_exists = dbContext.USER.Where(u => u.USERNAME.Equals(user.username)).FirstOrDefault() == null ? false : true;

            //Check if the email is already in use
            bool email_exists = dbContext.USER.Where(u => u.EMAIL.Equals(user.email)).FirstOrDefault() == null ? false : true;

            if (String.IsNullOrEmpty(user.username))
            {
                message = "Please enter your username.";
            }
            else if (username_exists)
            {
                message = "Username already exists! Please choose another.";
            }
            else if (String.IsNullOrEmpty(user.password))
            {
                message = "Please enter your password.";
            }
            else if (String.IsNullOrEmpty(user.email))
            {
                message = "Please enter your email.";
            }
            else if (email_exists)
            {
                message = "Email address already exists! Please choose another.";
            }
            else if (user.passwordRecoveryQue1.ToString().Trim() == "0" || user.passwordRecoveryQue2.ToString().Trim() == "0" || user.passwordRecoveryQue3.ToString().Trim() == "0")
            {
                message = "Please select all authentication questions.";
            }
            else if (String.IsNullOrEmpty(user.passwordRecoveryAns1) || String.IsNullOrEmpty(user.passwordRecoveryAns2) || String.IsNullOrEmpty(user.passwordRecoveryAns3))
            {
                message = "Please answer all authentication questions.";
            }
            else
            {
                success = true;
            }
            return success;
        }

        public bool VerifyPatientInfo(PatientViewModel patient, out string message)
        {
            bool success = false;
            message = "";
            try
            {
                patient.fullName = patient.fullName != null ? patient.fullName.Trim() : patient.fullName;
                patient.address = patient.address != null ? patient.address.Trim() : patient.address;

                //Check if the email is already in use
                bool nationalID_exists = dbContext.PATIENT.Any(p => p.PATIENTNATIONALID.Equals(patient.nationalID)) ? true : false;

                if (String.IsNullOrWhiteSpace(patient.fullName))
                {
                    message = "Please enter your full name.";
                }
                else if (String.IsNullOrWhiteSpace(patient.nationalID))
                {
                    message = "Please enter your National ID.";
                }
                else if (nationalID_exists)
                {
                    message = "National ID already exists! Please choose another.";
                }
                else if (String.IsNullOrWhiteSpace(patient.dateOfBirth))
                {
                    message = "Please enter your date of birth.";
                }
                else if (patient.gender.ToString().Trim() == "0")
                {
                    message = "Please select gender.";
                }
                else if (String.IsNullOrWhiteSpace(patient.phoneNumber))
                {
                    message = "Please enter your phone number.";
                }
                else if (String.IsNullOrWhiteSpace(patient.address))
                {
                    message = "Please enter your address.";
                }
                else
                {
                    success = true;
                }
                return success;
            }
            catch (Exception ex)
            {
                string sEventCatg = "PATIENT PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = nameof(VerifyPatientInfo);
                string sEventType = "S";
                string sInsBy = patient.fullName;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return success;
            }

        }

        int GetUserID(string username)
        {
            int id = 0;
            try
            {
                id = dbContext.USER.Where(u => u.USERNAME.Equals(username)).FirstOrDefault().USERID;
                return id;
            }
            catch (Exception ex)
            {
                string sEventCatg = "PATIENT PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "GetUserID";
                string sEventType = "S";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return id;
            }

        }

        void CreateNewUser(UserViewModel uvm)
        {
            try
            {
                USER user = new USER();

                user.ROLEID = 1303;
                user.USERNAME = uvm.username.Trim();
                user.PASSWORDHASH = PasswordHelper.HashPassword(uvm.password).Trim();
                user.EMAIL = uvm.email.Trim();
                user.LASTLOGIN = null;
                user.USERTYPE = "Patient";
                user.PASSWORDRECOVERYQUE1 = uvm.passwordRecoveryQue1;
                user.PASSWORDRECOVERYANS1 = uvm.passwordRecoveryAns1.Trim();
                user.PASSWORDRECOVERYQUE2 = uvm.passwordRecoveryQue2;
                user.PASSWORDRECOVERYANS2 = uvm.passwordRecoveryAns2.Trim();
                user.PASSWORDRECOVERYQUE3 = uvm.passwordRecoveryQue3;
                user.PASSWORDRECOVERYANS3 = uvm.passwordRecoveryAns3.Trim();
                user.STATUS = true;
                user.LOGINRETRYCOUNT = 0;
                user.LOGINLOCKDATE = null;
                user.CREATEDBY = uvm.username.Trim();
                user.CREATEDDATE = DateTime.Now;
                user.UPDATEDBY = uvm.username.Trim();
                user.UPDATEDDATE = DateTime.Now;
                user.DELETEDFLAG = false;
                if (uvm.profilePicture != null)
                {
                    user.AVATARURL = RenameProfilePicture(uvm.username, uvm.profilePicture);
                }
                else
                {
                    user.AVATARURL = null;
                }

                dbContext.USER.Add(user);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                string sEventCatg = "PATIENT PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "CreateNewUser";
                string sEventType = "I";
                string sInsBy = uvm.username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
            }

        }

        string RenameProfilePicture(string username, string path)
        {
            string fileName = "";
            string imgFormat = path.Substring(path.LastIndexOf("."));
            fileName = username + "_avatar" + imgFormat;
            return fileName;
        }

        void CreateNewPatient(PatientViewModel pvm, string username)
        {
            try
            {
                PATIENT patient = new PATIENT();

                patient.PATIENTNAME = pvm.fullName.Trim();
                patient.USERID = GetUserID(username);
                patient.PATIENTNATIONALID = pvm.nationalID.Trim();
                patient.PATIENTGENDER = pvm.gender;
                patient.PATIENTMOBILENO = pvm.phoneNumber.Trim();
                patient.PATIENTADDRESS = pvm.address.Trim();
                patient.PATIENTDATEOFBIRTH = DateTime.Parse(pvm.dateOfBirth);
                patient.CREATEDBY = username.Trim();
                patient.CREATEDDATE = DateTime.Now;
                patient.UPDATEDBY = username.Trim();
                patient.UPDATEDDATE = DateTime.Now;
                patient.DELETEDFLAG = false;

                dbContext.PATIENT.Add(patient);
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                string sEventCatg = "PATIENT PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "CreateNewPatient";
                string sEventType = "I";
                string sInsBy = GetInfo.Username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
            }

        }

        public void CreateNewAccount(UserViewModel user, PatientViewModel patient, out string message)
        {
            message = "Please check your email to activate your account before logging in.";
            CreateNewUser(user);
            CreateNewPatient(patient, user.username);
        }

        public bool IsValidToken(string token, string expires, ref string username)
        {
            try
            {
                if (token == null)
                {
                    throw new ArgumentNullException();
                }

                List<USER> uList = dbContext.USER.Where(u => u.LASTLOGIN == null).ToList();
                foreach (USER user in uList)
                {
                    string reGenerateToken = PasswordHelper.HashPassword(user.USERNAME.Trim()).Trim();
                    if (token.Equals(reGenerateToken))
                    {
                        username = user.USERNAME;
                        long currentUnixTime = DateTimeOffset.Now.ToUnixTimeSeconds();
                        long expirationUnixTime = long.Parse(expires);

                        if (currentUnixTime > expirationUnixTime)
                        {
                            throw new Exception("The activation link has expires.");
                        }

                        return true;
                    }
                }

                throw new Exception("Account not found!");
            }
            catch (Exception ex)
            {
                string sEventCatg = "PATIENT PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "IsValidToken";
                string sEventType = "S";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);

                return false;
            }
        }



        public void ActivateAccount(string username)
        {
            try
            {
                USER user = dbContext.USER.Where(u => u.USERNAME.Equals(username)).FirstOrDefault();
                if (user != null)
                {
                    user.LASTLOGIN = DateTime.Now;
                    dbContext.SaveChanges();
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch (Exception ex)
            {
                string sEventCatg = "PATIENT PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "ActivateAccount";
                string sEventType = "U";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
            }
        }


        public bool SetAuthenQuestions(AuthenQuestionModel question)
        {
            USER user = dbContext.USER.Where(u => u.USERNAME.Equals(question.username)).FirstOrDefault();
            try
            {

                if (user != null && (!IsAnswerPaswdRecovery(user.USERNAME)))
                {


                    user.PASSWORDRECOVERYQUE1 = question.passwordRecoveryQue1;
                    user.PASSWORDRECOVERYANS1 = question.passwordRecoveryAns1.Trim();
                    user.PASSWORDRECOVERYQUE2 = question.passwordRecoveryQue2;
                    user.PASSWORDRECOVERYANS2 = question.passwordRecoveryAns2.Trim();
                    user.PASSWORDRECOVERYQUE3 = question.passwordRecoveryQue3;
                    user.PASSWORDRECOVERYANS3 = question.passwordRecoveryAns3.Trim();
                    user.UPDATEDBY = question.username;
                    user.UPDATEDDATE = DateTime.Now;
                    dbContext.SaveChanges();
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch (Exception ex)
            {
                string sEventCatg = "NONE PORTAL";
                if (user != null)
                {
                    sEventCatg = user.USERTYPE == "Doctor" ? "DOCTOR PORTAL" : "PATIENT";
                }

                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = nameof(SetAuthenQuestions);
                string sEventType = "U";
                string sInsBy = question.username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
            }


            return true;
        }

        public bool IsAnswerPaswdRecovery(string username)
        {
            USER user = dbContext.USER.Where(u => u.USERNAME.Equals(username)).FirstOrDefault();

            if (user != null)
            {
                if (user.PASSWORDRECOVERYANS1 != null || user.USERTYPE == "Admin") return true;
            }

            return false;

        }
        public USER GetUserByUserName(string username)
        {
            USER user;
            try
            {
                user = dbContext.USER.Where(u => u.USERNAME.Equals(username)).FirstOrDefault();
                return user;
            }
            catch (Exception ex)
            {
                string sEventCatg = "PATIENT PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "GetUserID";
                string sEventType = "S";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return null;
            }

        }
    }
}