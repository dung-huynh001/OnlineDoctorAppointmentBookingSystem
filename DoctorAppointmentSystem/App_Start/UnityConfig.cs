using DoctorAppointmentSystem.Areas.Admin.Models;
using DoctorAppointmentSystem.Areas.Doctor.Models.Appointments;
using DoctorAppointmentSystem.Areas.Doctor.Models.Home;
using DoctorAppointmentSystem.Models.Account;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Unity;
using Unity.AspNet.Mvc;

namespace DoctorAppointmentSystem.App_Start
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
            var container = new UnityContainer();
            container.RegisterType<DbContext, DBContext>(TypeLifetime.Transient);
            container.RegisterType<ISystemParamService, SystemParamService>(TypeLifetime.Singleton);
            container.RegisterType<IMapperService, MapperService>(TypeLifetime.Transient);
            container.RegisterType<IloggerService, LoggerService>(TypeLifetime.Transient);
            //register admin IO 
            container.RegisterType<DoctorScheduleIO, DoctorScheduleIO>(TypeLifetime.Transient);
            container.RegisterType<AppointmentIO, AppointmentIO>(TypeLifetime.Transient);
            container.RegisterType<DashboardIO, DashboardIO>(TypeLifetime.Transient);
            container.RegisterType<DoctorIO, DoctorIO>(TypeLifetime.Transient);
            //register doctor IO 
            container.RegisterType<AppointmentsDoctorIO, AppointmentsDoctorIO>(TypeLifetime.Transient);
            container.RegisterType<calendarIO, calendarIO>(TypeLifetime.Transient);
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
        }
    }
}