using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.HelperClasses
{
    public class Logger
    {
        public static void TraceLog(string sEventCatg, string sEventMsg, string sEventSrc, string sEventType, string sInsBy)
        {
            try
            {
                // Trace log to txt file
                string sTraceMsg = sEventCatg + "\t" + sEventMsg + "\t" + sEventSrc + "\t" + sEventType + "\t" + sInsBy + "\n";

                string sTraceTime = DateTime.Now.ToString("ddMMMyyyyHHmmss");
                string sLogFormat = DateTime.Now.ToShortDateString().ToString() + " " + DateTime.Now.ToLongTimeString().ToString() + " ==> ";
                string mapfolder = HttpContext.Current.Server.MapPath("~/Log/Exception/");
                if (!Directory.Exists(mapfolder))
                {
                    Directory.CreateDirectory(mapfolder);
                }

                string lstPathSeparator = Path.DirectorySeparatorChar.ToString();
                string lstMonth = DateTime.Now.Month < 10
                                             ? "0" + DateTime.Now.Month.ToString()
                                             : DateTime.Now.Month.ToString();
                string lstYear = DateTime.Now.Year.ToString();
                string lstDestination = mapfolder + lstPathSeparator + lstYear + lstMonth + lstPathSeparator + DateTime.Now.ToString("ddMMM") + lstPathSeparator;
                if (!Directory.Exists(lstDestination))
                    Directory.CreateDirectory(lstDestination);
                string sPathName = lstDestination + "\\" + sTraceTime + ".txt";
                StreamWriter sw = new StreamWriter(sPathName, true);
                sw.WriteLine(sLogFormat + sTraceMsg);
                sw.Flush();
                sw.Close();
            }
            catch
            {
                return;
            }
            // Trace log to DB
            using (DBContext dbContext = new DBContext())
            {
                try
                {
                    LOG traceLog = new LOG();

                    traceLog.EVENTCATG = sEventCatg;
                    traceLog.EVENTMSG = sEventMsg;
                    traceLog.EVENTSRC = sEventSrc;
                    traceLog.EVENTTYPE = sEventType;
                    traceLog.CREATEDBY = sInsBy;
                    traceLog.CREATEDDATE = DateTime.Now;

                    dbContext.LOG.Add(traceLog);
                    dbContext.SaveChanges();
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
    }
}