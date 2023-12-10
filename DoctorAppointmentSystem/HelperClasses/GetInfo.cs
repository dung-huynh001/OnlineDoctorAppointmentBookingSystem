using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.HelperClasses
{
    public class GetInfo
    {
        public static string Username { get; set; }

        public static string GetFullName(string username)
        {
            string fullName = "";
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    USER user = dbContext.USER.Where(u => u.USERNAME.Equals(username)).FirstOrDefault();

                    if(user == null)
                    {
                        throw new Exception();
                    }

                    if (user.USERTYPE.ToLower() == "patient")
                    {
                        PATIENT patient = dbContext.PATIENT.Where(p => p.USERID.Equals(user.USERID)).FirstOrDefault();
                        fullName = patient.PATIENTNAME;
                    }
                    else if (user.USERTYPE.ToLower() == "doctor")
                    {
                        DOCTOR doctor = dbContext.DOCTOR.Where(d => d.USERID.Equals(user.USERID)).FirstOrDefault();
                        fullName = doctor.DOCTORNAME;
                    }
                    else
                    {
                        fullName = "Admin";
                    }
                    return fullName;
                }
            } 
            catch (Exception ex)
            {
                string sEventCatg = "";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "GetFullName";
                string sEventType = "S";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return fullName;
            }
            
        }

        public static string GetUserType(string username)
        {
            string userType = "";
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    int id = GetUserID(username);
                    USER user = dbContext.USER.Find(id);

                    if(user == null)
                    {
                        throw new Exception();
                    }
                    else
                    {
                        userType = user.USERTYPE;
                    }

                    return userType;
                }
            }
            catch (Exception ex)
            {
                string sEventCatg = "";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "GetUserType";
                string sEventType = "S";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return userType;
            }
        }

        public static int GetUserID(string username)
        {
            int id = 0;
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    USER user = dbContext.USER.Where(u => u.USERNAME.Equals(username)).FirstOrDefault();
                    if (user != null)
                    {
                        id = user.USERID;
                        return id;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
            catch (Exception ex)
            {
                string sEventCatg = "";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "GetUserID";
                string sEventType = "S";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return id;
            }
            
        }

        public static int GetPatientID(int userID)
        {
            int id = 0;
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    PATIENT patient = dbContext.PATIENT.Where(p => p.USERID.Equals(userID)).FirstOrDefault();
                    if (patient != null)
                    {
                        id = patient.PATIENTID;
                        return id;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
            catch (Exception ex)
            {
                string sEventCatg = "";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "GetPatientID";
                string sEventType = "S";
                string sInsBy = "";

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return id;
            }
            
        }

        public static int GetDoctorID(string username)
        {
            int id = 0;
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    int userID = GetUserID(username);

                    DOCTOR doctor = dbContext.DOCTOR.Where(p => p.USERID.Equals(userID)).FirstOrDefault();
                    if (doctor != null)
                    {
                        id = doctor.DOCTORID;
                        return id;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
            catch (Exception ex)
            {
                string sEventCatg = "";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "GetPatientID";
                string sEventType = "S";
                string sInsBy = "";

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);
                return id;
            }

        }

        public static string GetImgPath(string username)
        {
            string imgName = "default-user-image.png";
            string imgPath = "/Uploads/" + imgName;
            try
            {
                using (DBContext dbContext = new DBContext())
                {
                    USER user = dbContext.USER.Where(u => u.USERNAME.Equals(username)).FirstOrDefault();

                    if(user == null)
                    {
                        throw new Exception();
                    }

                    if (!String.IsNullOrEmpty(user.AVATARURL))
                    {
                        imgName = user.AVATARURL;
                    }
                    imgPath = "/Uploads/" + imgName;
                    return imgPath;
                }
            }
            catch (Exception ex)
            {
                string sEventCatg = "PATIENT PORTAL";
                string sEventMsg = "Exception: " + ex.Message;
                string sEventSrc = "GetFullName";
                string sEventType = "S";
                string sInsBy = username;

                Logger.TraceLog(sEventCatg, sEventMsg, sEventSrc, sEventType, sInsBy);

                return imgPath;
            }
        }
    }
}