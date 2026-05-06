using AutoMapper;

namespace EmployeeFlow.Mappings
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile()
        {
            CreateMap<DTOs.Department.CreateDepartmentRequest, Entities.Department>();
            CreateMap<DTOs.Department.UpdateDepartmentRequest, Entities.Department>();
            
            CreateMap<Entities.Department, DTOs.Department.DepartmentResponse>();
        }
    }
}