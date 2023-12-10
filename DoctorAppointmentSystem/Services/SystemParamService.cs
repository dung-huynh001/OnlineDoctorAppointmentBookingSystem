using DoctorAppointmentSystem.Models.DB;
using DoctorAppointmentSystem.Services.ServiceInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unity;

namespace DoctorAppointmentSystem.Services
{
 
    public class SystemParamService : ISystemParamService
    {
        private readonly DBContext _dbContext;
        private List<SYSTEM_PARA> _syprama;
        public SystemParamService(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<SYSTEM_PARA> GetAllParam() {

            if(_syprama == null)
                try
                {
                     _syprama = _dbContext.SYSTEM_PARA.ToList();

                }catch 
                {
                   
                }
                
            if(_syprama == null) 
                return new List<SYSTEM_PARA>();
            return _syprama;
        }

    }
}