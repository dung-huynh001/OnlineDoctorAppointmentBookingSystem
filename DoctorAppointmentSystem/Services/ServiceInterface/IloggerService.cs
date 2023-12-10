using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoctorAppointmentSystem.Services.ServiceInterface
{
     public interface IloggerService
    {
        
        void InsertLog(string eventCatalog,string eventMessage,string eventSource,string eventType,string insertBy);

    }
}
