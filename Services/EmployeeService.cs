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
            await EnsureCompanyExistsAsync(dto.CompanyId);
            await EnsureDepartmentBelongsToCompanyAsync(dto.DepartmentId, dto.CompanyId);
            await EnsureRoleBelongsToCompanyAsync(dto.RoleId, dto.CompanyId);

            var employee = _mapper.Map<Employee>(dto);

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            var employeeWithRelations = await _context.Employees
                .AsNoTracking()
                .Include(e => e.Department)
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.Id == employee.Id);

            if (employeeWithRelations is null)
                throw new Exception("Employee not found after creation.");

            return _mapper.Map<EmployeeResponse>(employeeWithRelations);
        }

        private async Task EnsureCompanyExistsAsync(int companyId)
        {
            if (!await _context.Companies.AnyAsync(c => c.Id == companyId))
                throw new Exception("Company not found.");
        }

        private async Task EnsureDepartmentBelongsToCompanyAsync(int departmentId, int companyId)
        {
            if (!await _context.Departments.AnyAsync(d => d.Id == departmentId && d.CompanyId == companyId))
                throw new Exception("Department not found or does not belong to this company.");
        }

        private async Task EnsureRoleBelongsToCompanyAsync(int roleId, int companyId)
        {
            if (!await _context.Roles.AnyAsync(r => r.Id == roleId && r.CompanyId == companyId))
                throw new Exception("Role not found or does not belong to this company.");
        }
    }
}