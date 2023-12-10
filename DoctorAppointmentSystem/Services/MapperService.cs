using AutoMapper;
using DoctorAppointmentSystem.Areas.Admin.Models.AdminUser.Mapping;
using DoctorAppointmentSystem.Areas.Admin.Models.AppointmentManage.Mapping;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorManage.Mapping;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorSchedule.Mapping;
using DoctorAppointmentSystem.Areas.Admin.Models.PatientManage.Mapping;
using DoctorAppointmentSystem.Areas.Admin.Models.UserManage.Mapping;
using DoctorAppointmentSystem.Areas.Doctor.Models.Appointments.Mapping;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Services
{
    
    public class MapperService : IMapperService
    {
        private readonly ISystemParamService _sysParam;
        private Mapper _mapper;

        public MapperService(ISystemParamService sysParam)
        {
            _sysParam = sysParam;
            InitAutomapper();
        }

        private void InitAutomapper()
        {

            var config = new MapperConfiguration(cfg =>
            {
                //register mapper for doctor model
                cfg.AddProfile( new MapDoctorProfile(_sysParam));
                //register mapper for patient model
                cfg.AddProfile(new MapPatientProfile());
                //register mapper for user model
                cfg.AddProfile(new MapUserProfile(_sysParam));
                //register mapper for admin user model
                cfg.AddProfile(new MapAdminUserProfile());
                //register mapper for doctor schedule
                cfg.AddProfile(new MappDoctorScheduleProfile(_sysParam));
                //register mapper for appointment
                cfg.AddProfile(new MapAppointmentProfile(_sysParam));
                //register mapper for appointment doctor
                cfg.AddProfile(new AppointmentDoctorMappingProfile(_sysParam));


            });

            _mapper = new Mapper(config);
            

        }

        public Mapper GetMapper()
        {
            if (_mapper == null)
                InitAutomapper();
            return _mapper;
        }

        
    }          
}