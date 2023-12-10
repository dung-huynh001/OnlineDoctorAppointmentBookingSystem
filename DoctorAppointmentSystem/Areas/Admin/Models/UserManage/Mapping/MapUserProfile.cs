using AutoMapper;
using DoctorAppointmentSystem.Areas.Admin.Models.DoctorManage;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.UserManage.Mapping
{
    public class MapUserProfile : Profile
    {
        public MapUserProfile(ISystemParamService sysParam)
        {
            CreateMap<USER, UserViewModel>()


                .ForMember(dest => dest.EMAIL, act => act.MapFrom(src => src.EMAIL))
                .ForMember(dest => dest.USERNAME, act => act.MapFrom(src => src.USERNAME))
                .ForMember(dest => dest.LOGINLOCKDATE, act => act.MapFrom(src => src.LOGINLOCKDATE !=null? src.LOGINLOCKDATE.Value.ToString() :""))
                .ForMember(dest => dest.LOGINRETRYCOUNT, act => act.MapFrom(src => src.LOGINRETRYCOUNT))
                .ForMember(dest => dest.CREATEDDATE, act => act.MapFrom(src => src.CREATEDDATE.Value.ToString()))
                .ForMember(dest => dest.UPDATEDDATE, act => act.MapFrom(src => src.UPDATEDDATE.Value.ToString()))
                .ForMember(dest => dest.LASTLOGIN, act => act.MapFrom(src => src.LASTLOGIN != null? src.LASTLOGIN.Value.ToString(): ""))
                .ForMember(dest => dest.STATUS, act => act.MapFrom(src => src.STATUS))

                .ForMember(dest => dest.FULLNAME, act => act.MapFrom(src =>
                                        (src.USERTYPE == "Admin") ? "" :
                                        (src.USERTYPE == "Doctor") ? src.DOCTOR.FirstOrDefault().DOCTORNAME :
                                        (src.USERTYPE == "Patient") ? src.PATIENT.FirstOrDefault().PATIENTNAME :
                                        ""
                                        ))
                .ForMember(dest => dest.GENDER, act => act.MapFrom(src =>
                                        (src.USERTYPE == "Admin") ? "" :
                                        (src.USERTYPE == "Doctor") ?   sysParam.GetAllParam().Where(s => s.ID == Convert.ToInt32(src.DOCTOR.FirstOrDefault().DOCTORGENDER) && s.GROUPID == "Gender").Select(s => s.PARAVAL).FirstOrDefault() :
                                        (src.USERTYPE == "Patient") ?  sysParam.GetAllParam().Where(s => s.ID == Convert.ToInt32(src.PATIENT.FirstOrDefault().PATIENTGENDER) && s.GROUPID == "Gender").Select(s => s.PARAVAL).FirstOrDefault() :
                                        ""
                                        ))
                 .ForMember(dest => dest.MOBILENO, act => act.MapFrom(src =>
                                        (src.USERTYPE == "Admin") ? "" :
                                        (src.USERTYPE == "Doctor") ? src.DOCTOR.FirstOrDefault().DOCTORMOBILENO :
                                        (src.USERTYPE == "Patient") ? src.PATIENT.FirstOrDefault().PATIENTMOBILENO :
                                        ""
                                        ))
            ;
        }
    }
}