using AutoMapper;
using EmployeeFlow.DTOs.Employees;
using EmployeeFlow.Entities;

namespace EmployeeFlow.Mappings
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<CreateEmployeeRequest, Employee>();
            CreateMap<UpdateEmployeeRequest, Employee>();

            CreateMap<Employee, EmployeeResponse>()
                .ForCtorParam("Id", opt => opt.MapFrom(src => src.Id))
                .ForCtorParam("Name", opt => opt.MapFrom(src => src.Name))
                .ForCtorParam("Email", opt => opt.MapFrom(src => src.Email))
                .ForCtorParam("DepartmentId", opt => opt.MapFrom(src => src.DepartmentId))
                .ForCtorParam("Department", opt => opt.MapFrom(src => src.Department.Name))
                .ForCtorParam("RoleId", opt => opt.MapFrom(src => src.RoleId))
                .ForCtorParam("Role", opt => opt.MapFrom(src => src.Role.Name))
                .ForCtorParam("CompanyId", opt => opt.MapFrom(src => src.CompanyId));
        }
    }
}