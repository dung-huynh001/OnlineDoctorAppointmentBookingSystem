using AutoMapper;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorManage;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorScheduleManage;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.DoctorSchedule.Mapping
{
    public class MappDoctorScheduleProfile : Profile
    {
        public MappDoctorScheduleProfile(ISystemParamService sysParam)
        {
            CreateMap<SCHEDULE, DoctorScheduleViewModel>()
                .ForMember(dest => dest.DOCTORID, act => act.MapFrom(src => src.DOCTORID))
                .ForMember(dest => dest.DOCTORNAME, act => act.MapFrom(src => (src.DOCTOR!=null)? src.DOCTOR.DOCTORNAME: ""))
                .ForMember(dest => dest.WORKINGDAY, act => act.MapFrom(src => src.WORKINGDAY.ToString(@"MMMM dd, yyyy")))
                .ForMember(dest => dest.APPOINTMENTNUM, act => act.MapFrom(src => src.APPOINTMENT.Count ))
                .ForMember(dest => dest.SHIFTTIME, act => act.MapFrom(src => DateTime.Today.Add( src.SHIFTTIME).ToString("hh:mm tt")))
                .ForMember(dest => dest.BREAKTIME, act => act.MapFrom(src => DateTime.Today.Add(src.BREAKTIME).ToString("hh:mm tt") ))
                .ForMember(dest => dest.CONSULTANTTIME, act => act.MapFrom(src => 
                                                                                (sysParam.GetAllParam().Where(p => p.ID == src.CONSULTANTTIME))!=null? 
                                                                                sysParam.GetAllParam().Where(p=>p.ID == src.CONSULTANTTIME).FirstOrDefault().NOTE:"" ))

                .ForMember(dest => dest.CREATEDDATE, act => act.MapFrom(src => src.CREATEDDATE.Value.ToString(@"MM-dd-yyyy hh\:mm tt")))
                .ForMember(dest => dest.UPDATEDDATE, act => act.MapFrom(src => src.UPDATEDDATE.Value.ToString(@"MM-dd-yyyy hh\:mm tt")))
                ;
            CreateMap<SCHEDULE,DoctorScheduleViewEditModel>()
                .ForMember(dest => dest.WORKINGDAY, act => act.MapFrom(src => src.WORKINGDAY.ToShortDateString()))
                .ForMember(dest => dest.SHIFTTIME, act => act.MapFrom(src => src.SHIFTTIME.ToString()))
                .ForMember(dest => dest.BREAKTIME, act => act.MapFrom(src => src.BREAKTIME.ToString()))
                ;
            CreateMap<DoctorScheduleCreateModel, SCHEDULE>()
                .ForMember(dest => dest.SCHEDULEID, act => act.MapFrom(src => 0))
                ;
            CreateMap<DoctorScheduleEditModel, SCHEDULE>();
        }
    }
}
