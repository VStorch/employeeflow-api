using EmployeeFlow.Data;
using EmployeeFlow.DTOs.Department;
using EmployeeFlow.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeFlow.Services
{
    public class DepartmentService
    {
        private readonly AppDbContext _context;

        public DepartmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DepartmentResponse> CreateAsync(CreateDepartmentRequest dto)
        {
            await EnsureCompanyExistsAsync(dto.CompanyId);

            await EnsureDepartmentIsUniqueAsync(dto.Name, dto.CompanyId);

            var department = new Department
            {
                Name = dto.Name,
                CompanyId = dto.CompanyId
            };

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return new DepartmentResponse(department.Id, department.Name, department.CompanyId);
        }

        public async Task<List<DepartmentResponse>> GetByCompanyAsync(int companyId)
        {
            await EnsureCompanyExistsAsync(companyId);

            return await _context.Departments
                .Where(d => d.CompanyId == companyId)
                .Select(d => new DepartmentResponse(d.Id, d.Name, d.CompanyId))
                .ToListAsync();
        }

        private async Task EnsureCompanyExistsAsync(int companyId)
        {
            if (!await _context.Companies.AnyAsync(c => c.Id == companyId))
                throw new Exception("Company not found");
        }

        private async Task EnsureDepartmentIsUniqueAsync(string name, int companyId)
        {
            if (await _context.Departments.AnyAsync(d => d.Name == name && d.CompanyId == companyId))
                throw new Exception("Department already exists in this company");
        }
    }
}