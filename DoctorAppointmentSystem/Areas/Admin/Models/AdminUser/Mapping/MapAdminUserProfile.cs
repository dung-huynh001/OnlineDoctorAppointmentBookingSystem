using AutoMapper;
using DoctorAppointmentSystem.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.AdminUser.Mapping
{
    public class MapAdminUserProfile : Profile
    {
        public MapAdminUserProfile()
        {
            CreateMap<USER, AdminUserViewModel>()

               .ForMember(dest => dest.EMAIL, act => act.MapFrom(src => src.EMAIL))
               .ForMember(dest => dest.USERNAME, act => act.MapFrom(src => src.USERNAME))
               .ForMember(dest => dest.LOGINLOCKDATE, act => act.MapFrom(src => src.LOGINLOCKDATE != null ? src.LOGINLOCKDATE.Value.ToString(@"MM-dd-yyyy") : ""))
               .ForMember(dest => dest.LOGINRETRYCOUNT, act => act.MapFrom(src => src.LOGINRETRYCOUNT))
               .ForMember(dest => dest.CREATEDDATE, act => act.MapFrom(src => src.CREATEDDATE.Value.ToString(@"MM-dd-yyyy")))
               .ForMember(dest => dest.UPDATEDDATE, act => act.MapFrom(src => src.UPDATEDDATE.Value.ToString(@"MM-dd-yyyy")))
               .ForMember(dest => dest.LASTLOGIN, act => act.MapFrom(src => src.LASTLOGIN != null ? src.LASTLOGIN.Value.ToString(@"MM-dd-yyyy") : ""))

           ;

            CreateMap<USER, AdminUserViewEditModel>();

        }
    }
}