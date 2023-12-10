using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoctorAppointmentSystem.Services.ServiceInterface
{
    public interface IMapperService
    {
        Mapper GetMapper();
    }
}