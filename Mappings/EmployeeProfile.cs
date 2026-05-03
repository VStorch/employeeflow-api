using AutoMapper;
using EmployeeFlow.DTOs.Employees;
using EmployeeFlow.Entities;

namespace EmployeeFlow.Mappings
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<DTOs.CreateEmployeeDTO, Employee>();
            CreateMap<Employee, EmployeeResponse>()
                .ForMember(dest => dest.Department, opt => opt.MapFrom(src => src.Department.Name))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.Name));
        }
    }
}