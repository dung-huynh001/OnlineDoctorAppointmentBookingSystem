using AutoMapper;
using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Areas.Admin.Models.AppointmentManage.Mapping
{
    public class MapAppointmentProfile : Profile
    {
        public MapAppointmentProfile(ISystemParamService sysParam) {
            CreateMap<APPOINTMENT, ApponitmentViewModel>()
                .ForMember(dest => dest.PATIENTNAME, act =>
                                                     act.MapFrom(src => src.PATIENT != null ? src.PATIENT.PATIENTNAME : ""))
                .ForMember(dest => dest.DOCTORNAME, act =>
                                                    act.MapFrom(src => src.SCHEDULE == null ? "" :
                                                                       src.SCHEDULE.DOCTOR != null ? src.SCHEDULE.DOCTOR.DOCTORNAME : ""))

                 .ForMember(dest => dest.APPOINTMENTDATE, act => act.MapFrom(src => src.APPOINTMENTDATE != null ? src.APPOINTMENTDATE.Value.ToString(@"MM-dd-yyyy hh\:mm tt") : ""))
                .ForMember(dest => dest.DATEOFCONSUITATION, act => act.MapFrom(src => src.DATEOFCONSULTATION != null ? src.DATEOFCONSULTATION.ToString(@"MM-dd-yyyy hh\:mm tt") : ""))

                .ForMember(dest => dest.CLOSEDDATE, act => act.MapFrom(src => src.CLOSEDDATE != null ? src.CLOSEDDATE.Value.ToString(@"MM-dd-yyyy hh\:mm tt") : ""))
                .ForMember(dest => dest.CREATEDDATE, act => act.MapFrom(src => src.CREATEDDATE != null ? src.CREATEDDATE.Value.ToString(@"MM-dd-yyyy hh\:mm tt") : ""))
                .ForMember(dest => dest.UPDATEDDATE, act => act.MapFrom(src => src.UPDATEDDATE != null ? src.UPDATEDDATE.Value.ToString(@"MM-dd-yyyy hh\:mm tt") : ""))
                ;

            CreateMap<APPOINTMENT, CompletedApptViewDetailsModel>()
                // doctor
                .ForMember(dest => dest.doctorID, act => act.MapFrom(src => src.DOCTORID ))
                .ForMember(dest => dest.doctorName, act => act.MapFrom(src => src.SCHEDULE != null && src.SCHEDULE.DOCTOR != null ?
                                                                   src.SCHEDULE.DOCTOR.DOCTORNAME : ""))
                .ForMember(dest => dest.doctorGender, act => act.MapFrom(src => src.SCHEDULE!= null && src.SCHEDULE.DOCTOR != null ?
                                                                    sysParam.GetAllParam().Where(s => 
                                                                    s.ID == src.SCHEDULE.DOCTOR.DOCTORGENDER && 
                                                                    s.GROUPID == "Gender").Select(s => s.PARAVAL)
                                                                    .FirstOrDefault() 
                                                                : "" ))
                .ForMember(dest => dest.doctorSpeciality, act => act.MapFrom(src => src.SCHEDULE != null && src.SCHEDULE.DOCTOR != null ?
                                                                   src.SCHEDULE.DOCTOR.SPECIALITY : ""))
                //patient
                .ForMember(dest => dest.patientName, act => act.MapFrom(src => src.PATIENT != null?
                                                                   src.PATIENT.PATIENTNAME : ""))
                .ForMember(dest => dest.patientGender, act => act.MapFrom(src => src.PATIENT != null  ?
                                                                    sysParam.GetAllParam().Where(s =>
                                                                    s.ID == src.PATIENT.PATIENTGENDER &&
                                                                    s.GROUPID == "Gender").Select(s => s.PARAVAL)
                                                                    .FirstOrDefault()
                                                                : ""))
                .ForMember(dest => dest.patientDateOfBirth, act => act.MapFrom(src => src.PATIENT != null ?
                                                                   src.PATIENT.PATIENTDATEOFBIRTH.ToString("yyyy-MM-dd") : ""))
                // Appointment Details
                .ForMember(dest => dest.modeOfConsultant, act => act.MapFrom(src => src.MODEOFCONSULTANT))
                .ForMember(dest => dest.consultantType, act => act.MapFrom(src => src.CONSULTANTTYPE))
                .ForMember(dest => dest.dateOfConsultation, act => act.MapFrom(src => src.DATEOFCONSULTATION.ToShortDateString()))
                .ForMember(dest => dest.consultationTime, act => act.MapFrom(src => src.DATEOFCONSULTATION.TimeOfDay.ToString(@"hh\:mm")))
                .ForMember(dest => dest.appointmentDate, act => act.MapFrom(src => src.APPOINTMENTDATE!=null? src.APPOINTMENTDATE.Value.ToShortDateString():""))
                .ForMember(dest => dest.appointmentTime, act => act.MapFrom(src => src.APPOINTMENTDATE != null ? src.APPOINTMENTDATE.Value.TimeOfDay.ToString(@"hh\:mm") : ""))
                .ForMember(dest => dest.consultantTime, act => act.MapFrom(src => sysParam.GetAllParam()
                                                                                            .Where(p=>p.ID == src.SCHEDULE.CONSULTANTTIME)
                                                                                            .Select(s => s.PARAVAL)
                                                                                            .FirstOrDefault()))
                .ForMember(dest => dest.symtoms, act => act.MapFrom(src => src.SYMTOMS))
                .ForMember(dest => dest.existingIllness, act => act.MapFrom(src => src.EXISTINGILLNESS))
                .ForMember(dest => dest.drugAlergies, act => act.MapFrom(src => src.DRUGALLERGIES))
                ;
        }
    }
}