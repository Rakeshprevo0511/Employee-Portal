using AutoMapper;
using dotnet_core_MVC.Models;


namespace dotnet_core_MVC.DTO
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeDto, Employee>();
        }
    }
}
