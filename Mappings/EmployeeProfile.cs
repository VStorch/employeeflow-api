using AutoMapper;
using EmployeeFlow.Entities;

namespace EmployeeFlow.Mappings
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<DTOs.CreateEmployeeDTO, Employee>();
        }
    }
}