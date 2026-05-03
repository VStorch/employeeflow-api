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
            var companyExists = await _context.Companies
                .AnyAsync(c => c.Id == dto.CompanyId);

            if (!companyExists)
                throw new Exception("Company not found");

            var alreadyExists = await _context.Departments
                .AnyAsync(d => d.Name == dto.Name && d.CompanyId == dto.CompanyId);

            if (alreadyExists)
                throw new Exception("Department already exists in this company");

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
            return await _context.Departments
                .Where(d => d.CompanyId == companyId)
                .Select(d => new DepartmentResponse(d.Id, d.Name, d.CompanyId))
                .ToListAsync();
        }
    }
}