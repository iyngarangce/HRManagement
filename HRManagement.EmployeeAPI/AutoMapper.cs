using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HRManagement.DTO;
using HRManagement.Model.Employee;

namespace HRManagement.EmployeeAPI
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<Employee, EmployeeDTO>();
            CreateMap<Temporary, EmployeeDTO>();
        }
    }
}