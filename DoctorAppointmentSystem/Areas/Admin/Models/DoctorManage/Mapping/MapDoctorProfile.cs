using AutoMapper;
using DoctorAppointmentSystem.Models.DB;

using DoctorAppointmentSystem.Services.ServiceInterface;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.DoctorManage.Mapping
{
    public class MapDoctorProfile : Profile
    {
        public MapDoctorProfile(ISystemParamService sysParam)
        {

            CreateMap<DOCTOR, DoctorViewModel>()

                .ForMember(dest => dest.DEPARTMENT, act => act.MapFrom(src => src.DEPARTMENT != null ? src.DEPARTMENT.DEPARTMENTNAME : "none"))
                .ForMember(dest => dest.DOCTORDATEOFBIRTH, act => act.MapFrom(src => src.DOCTORDATEOFBIRTH.ToString(@"MM-dd-yyyy")))
                .ForMember(dest => dest.WORKINGENDDATE, act => act.MapFrom(src => src.WORKINGENDDATE.ToString(@"MM-dd-yyyy")))
                .ForMember(dest => dest.WORKINGSTARTDATE, act => act.MapFrom(src => src.WORKINGSTARTDATE.ToString(@"MM-dd-yyyy")))
                .ForMember(dest => dest.CREATEDDATE, act => act.MapFrom(src => src.CREATEDDATE.Value.ToString(@"MM-dd-yyyy hh\:mm tt")))
                .ForMember(dest => dest.UPDATEDDATE, act => act.MapFrom(src => src.UPDATEDDATE.Value.ToString(@"MM-dd-yyyy hh\:mm tt")))
                .ForMember(dest => dest.EMAIL, act => act.MapFrom(src => src.USER != null ? src.USER.EMAIL : "none"))
                .ForMember(dest => dest.USERNAME, act => act.MapFrom(src => src.USER != null ? src.USER.USERNAME : "none"))
                .ForMember(dest => dest.LOGINLOCKDATE, act => act.MapFrom(src => (src.USER != null) ? ((src.USER.LOGINLOCKDATE != null) ? src.USER.LOGINLOCKDATE.Value.ToString(@"MM-dd-yyyy hh\:mm tt") : "none") : "none"))
                .ForMember(dest => dest.LOGINRETRYCOUNT, act => act.MapFrom(src => (src.USER != null) ? ((src.USER.LOGINRETRYCOUNT != null) ? src.USER.LOGINRETRYCOUNT.Value : 0) : 0))
                .ForMember(dest => dest.DOCTORGENDER, act => act.MapFrom(src => sysParam.GetAllParam().Where( s=>s.ID == src.DOCTORGENDER && s.GROUPID == "Gender" ).Select(s=>s.PARAVAL).FirstOrDefault()))
                ;

            CreateMap<DoctorCreateModel, DOCTOR>()

                ;

            CreateMap<DOCTOR, DoctorViewEditModel>()
                
                .ForMember(dest => dest.EMAIL, act => act.MapFrom(src => src.USER != null ? src.USER.EMAIL : "none"))
                .ForMember(dest => dest.USERNAME, act => act.MapFrom(src => src.USER != null ? src.USER.USERNAME : "none"))
                .ForMember(dest => dest.LOGINLOCKDATE, act => act.MapFrom(src => (src.USER != null) ? ((src.USER.LOGINLOCKDATE != null) ? src.USER.LOGINLOCKDATE.Value.ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd")) : DateTime.Now.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.DOCTORDATEOFBIRTH, act => act.MapFrom(src => src.DOCTORDATEOFBIRTH.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.WORKINGENDDATE, act => act.MapFrom(src => src.WORKINGENDDATE.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.WORKINGSTARTDATE, act => act.MapFrom(src => src.WORKINGSTARTDATE.ToString("yyyy-MM-dd")))
                .ForMember(dest => dest.DOCTORADDRESS, act => act.MapFrom(src => src.DOCTORADDRESS.Trim()))
                .ForMember(dest => dest.DOCTORNATIONALID, act => act.MapFrom(src => src.DOCTORNATIONALID.Trim()))
                .ForMember(dest => dest.DOCTORGENDER, act => act.MapFrom(src => Convert.ToInt32(src.DOCTORGENDER )))
                ;
            CreateMap<DoctorEditModel, DOCTOR>()
                ;

        }
    }
}