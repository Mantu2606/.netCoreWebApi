using AutoMapper;
using WebPocHub.Models;
using WebPocHub.WebApi.DTO; 

namespace WebPocHub.WebApi.Profiles
{
    public class EmployeesProfile:Profile
    {
        public EmployeesProfile()
        {
           CreateMap<Employee, EmployeeDto>();
           CreateMap<NewEmployeeDto, Employee>();
            CreateMap<UpdateEmployeeDto, Employee>();
        }
    }
}
