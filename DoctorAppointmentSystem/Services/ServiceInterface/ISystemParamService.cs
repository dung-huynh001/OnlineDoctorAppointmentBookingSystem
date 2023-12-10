using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Services.ServiceInterface
{
    public interface ISystemParamService
    {
        List<SYSTEM_PARA> GetAllParam();
    }
}