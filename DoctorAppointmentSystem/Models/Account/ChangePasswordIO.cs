using DoctorAppointmentSystem.HelperClasses;
using DoctorAppointmentSystem.Models.Account.ChangePassword;
using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Models.Account
{
    public class ChangePasswordIO
    {
        public List<string> GetAuthQuestions()
        {
            string username = GetInfo.Username;
            int id = GetInfo.GetUserID(username);

            List<string> authQuestions = new List<string>();
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    USER user = dbContext.USER.Find(id);

                    if (user == null)
                    {
                        throw new ArgumentNullException();
                    }
                    else
                    {
                        string ques1 = SystemParaHelper.GetParaval((int)user.PASSWORDRECOVERYQUE1);
                        string ques2 = SystemParaHelper.GetParaval((int)user.PASSWORDRECOVERYQUE2);
                        string ques3 = SystemParaHelper.GetParaval((int)user.PASSWORDRECOVERYQUE3);

                        authQuestions.Add(ques1);
                        authQuestions.Add(ques2);
                        authQuestions.Add(ques3);

                        return authQuestions;
                    }
                }
            }
            catch (Exception ex)
            {
                string sEventCatg = "PATIENT PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "GetAuthQuestions";
                string sEventType = "S";
                string sInsBy = GetInfo.Username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return authQuestions;
            }
        }

        public List<string> GetAuthQuestions(string username)
        {
            
            int id = GetInfo.GetUserID(username);

            List<string> authQuestions = new List<string>();
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    USER user = dbContext.USER.Find(id);

                    if (user == null)
                    {
                        throw new ArgumentNullException();
                    }
                    else
                    {
                        string ques1 = SystemParaHelper.GetParaval((int)user.PASSWORDRECOVERYQUE1);
                        string ques2 = SystemParaHelper.GetParaval((int)user.PASSWORDRECOVERYQUE2);
                        string ques3 = SystemParaHelper.GetParaval((int)user.PASSWORDRECOVERYQUE3);

                        authQuestions.Add(ques1);
                        authQuestions.Add(ques2);
                        authQuestions.Add(ques3);

                        return authQuestions;
                    }
                }
            }
            catch (Exception ex)
            {
                string sEventCatg = "PATIENT PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "GetAuthQuestions";
                string sEventType = "S";
                string sInsBy = GetInfo.Username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return authQuestions;
            }
        }

        public List<string> GetAnswers()
        {
            string username = GetInfo.Username;
            int id = GetInfo.GetUserID(username);

            List<string> answers = new List<string>();
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    USER user = dbContext.USER.Find(id);

                    if (user == null)
                    {
                        throw new ArgumentNullException();
                    }
                    else
                    {
                        answers.Add(user.PASSWORDRECOVERYANS1);
                        answers.Add(user.PASSWORDRECOVERYANS2);
                        answers.Add(user.PASSWORDRECOVERYANS3);

                        return answers;
                    }
                }
            }
            catch (Exception ex)
            {
                string sEventCatg = "PATIENT PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "GetAuthQuestions";
                string sEventType = "S";
                string sInsBy = GetInfo.Username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return answers;
            }
        }
        public List<string> GetAnswers( string username)
        {
            
            int id = GetInfo.GetUserID(username);

            List<string> answers = new List<string>();
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    USER user = dbContext.USER.Find(id);

                    if (user == null)
                    {
                        throw new ArgumentNullException();
                    }
                    else
                    {
                        answers.Add(user.PASSWORDRECOVERYANS1);
                        answers.Add(user.PASSWORDRECOVERYANS2);
                        answers.Add(user.PASSWORDRECOVERYANS3);

                        return answers;
                    }
                }
            }
            catch (Exception ex)
            {
                string sEventCatg = "PATIENT PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "GetAuthQuestions";
                string sEventType = "S";
                string sInsBy = GetInfo.Username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return answers;
            }
        }

        public bool VerifyAccount(VerifyAccountViewModel vam ,string username)
        {
            try
            {
                List<string> answers = GetAnswers(username);
                if (vam.ans1.Equals(answers[0])
                        && vam.ans2.Equals(answers[1])
                        && vam.ans3.Equals(answers[2]))
                {
                    return true;
                }
                else
                {
                    throw new Exception("User failed to verify account.");
                }
            }
            catch (Exception ex)
            {
                string sEventCatg = "PATIENT PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "VerifyAccount";
                string sEventType = "S";
                string sInsBy = GetInfo.Username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return false;
            }
        }

        public bool ChangePassword(ChangePasswordViewModel data)
        {
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    int id = GetInfo.GetUserID(GetInfo.Username);
                    USER user = dbContext.USER.Find(id);
                    if (user == null)
                    {
                        throw new Exception("No matching users found");
                    }
                    else
                    {
                        string currPassHashed = PasswordHelper.HashPassword(data.currPass);
                        if (currPassHashed.Equals(user.PASSWORDHASH.Trim()))
                        {
                            user.PASSWORDHASH = PasswordHelper.HashPassword(data.newPass);
                            dbContext.SaveChanges();
                            return true;
                        }
                        else
                        {
                            throw new Exception("User failed to change password. Incorrect password.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string sEventCatg = "PATIENT PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "ChangePassword";
                string sEventType = "U";
                string sInsBy = GetInfo.Username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return false;
            }
        }

    }
}
