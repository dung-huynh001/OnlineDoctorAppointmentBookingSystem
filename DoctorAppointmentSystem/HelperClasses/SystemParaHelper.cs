using DoctorAppointmentSystem.Models.Account.Register;
using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.HelperClasses
{
    public class SystemParaHelper
    {
        public static List<GenderViewModel> GenerateGender()
        {
            using (DBContext dbContext = new DBContext())
            {
                List<SYSTEM_PARA> system_paraList = dbContext.SYSTEM_PARA.Where(c => c.GROUPID.Equals("Gender")).ToList();
                List<GenderViewModel> genderList = new List<GenderViewModel>();
                foreach (SYSTEM_PARA item in system_paraList)
                {
                    GenderViewModel gender = new GenderViewModel(item.ID, item.PARAID, item.GROUPID, item.PARAVAL);
                    genderList.Add(gender);
                }
                return genderList;
            }
        }

        public static string GetParaval(int paraID)
        {
            using (DBContext dbContext = new DBContext())
            {
                string paraval = "Cannot connected to database!";

                SYSTEM_PARA para = dbContext.SYSTEM_PARA.Find(paraID);
                if(para != null)
                {
                    paraval = para.PARAVAL;
                }

                return paraval;
            }
        }

        public static List<SYSTEM_PARA> GenerateByGroup(string groupID)
        {
            using (DBContext dbContext = new DBContext()) 
            {
                List<SYSTEM_PARA> list = dbContext.SYSTEM_PARA.Where(l => l.GROUPID.Equals(groupID)).ToList();
                return list;
            }
        }

        public static List<AuthQuestionViewModel> GenerateAuthQuestion()
        {
            using (DBContext dbContext = new DBContext())
            {
                List<SYSTEM_PARA> system_paraList = dbContext.SYSTEM_PARA.Where(c => c.GROUPID.Equals("authQuestion")).ToList();
                List<AuthQuestionViewModel> questionList = new List<AuthQuestionViewModel>();
                foreach (SYSTEM_PARA item in system_paraList)
                {
                    AuthQuestionViewModel question = new AuthQuestionViewModel(item.ID, item.PARAID, item.GROUPID, item.PARAVAL);
                    questionList.Add(question);
                }
                return questionList;
            }
        }
    }
}