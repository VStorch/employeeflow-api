using AutoMapper;
using EmployeeFlow.Entities;
using EmployeeFlow.DTOs.Role;

namespace EmployeeFlow.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<CreateRoleRequest, Role>();
            CreateMap<UpdateRoleRequest, Role>();
            
            CreateMap<Role, RoleResponse>();
        }
    }
}