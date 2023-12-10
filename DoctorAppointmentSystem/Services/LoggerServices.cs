using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Services
{
    public class LoggerService : IloggerService
    {

        private readonly DBContext _dbContext;

       

        public LoggerService(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void InsertLog(string eventCatalog, string eventMessage, string eventSource, string eventType, string insertBy)
        {
            if (!string.IsNullOrEmpty(eventCatalog) && !string.IsNullOrEmpty(eventMessage) && !string.IsNullOrEmpty(eventSource) && !string.IsNullOrEmpty(eventType))
            {
                LOG log = new LOG() {
                    
                    EVENTCATG = eventCatalog,
                    EVENTMSG = eventMessage,
                    EVENTSRC = eventSource,
                    EVENTTYPE = eventType, 
                    CREATEDBY = insertBy,
                    CREATEDDATE = DateTime.Now,
                };

            _dbContext.LOG.Add(log);
            _dbContext.SaveChanges();
            }
        }
    }
}