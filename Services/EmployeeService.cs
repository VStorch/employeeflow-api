using AutoMapper;
using EmployeeFlow.Data;
using EmployeeFlow.DTOs;
using EmployeeFlow.DTOs.Employees;
using EmployeeFlow.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeFlow.Services
{
    public class EmployeeService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public EmployeeService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<EmployeeResponse> CreateAsync(CreateEmployeeDTO dto)
        {
            await ValidateDepartmentAndRole(dto.DepartmentId, dto.RoleId);
        
            var employee = _mapper.Map<Employee>(dto);
        
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
        
            var employeeWithRelations = await _context.Employees
                .AsNoTracking()
                .Include(e => e.Department)
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.Id == employee.Id);
        
            if (employeeWithRelations is null)
                throw new Exception("Employee not found");
        
            return _mapper.Map<EmployeeResponse>(employeeWithRelations);
        }

        private async Task ValidateDepartmentAndRole(int departmentId, int roleId)
        {
            if (!await _context.Departments.AnyAsync(d => d.Id == departmentId))
                throw new Exception("Department not found.");
            
            if (!await _context.Roles.AnyAsync(r => r.Id == roleId))
                throw new Exception("Role not found.");
        }
    }
}