using EmployeeFlow.Data;
using EmployeeFlow.DTOs.Role;
using EmployeeFlow.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeFlow.Services
{
    public class RoleService
    {
        private readonly AppDbContext _context;

        public RoleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RoleResponse> CreateAsync(CreateRoleRequest dto)
        {
            await EnsureCompanyExistsAsync(dto.CompanyId);

            await EnsureRoleIsUniqueAsync(dto.Name, dto.CompanyId);

            var role = new Role
            {
                Name = dto.Name,
                CompanyId = dto.CompanyId
            };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return new RoleResponse(role.Id, role.Name, role.CompanyId);
        }

        public async Task<List<RoleResponse>> GetByCompanyAsync(int companyId)
        {
            await EnsureCompanyExistsAsync(companyId);

            return await _context.Roles
                .Where(r => r.CompanyId == companyId)
                .Select(r => new RoleResponse(r.Id, r.Name, r.CompanyId))
                .ToListAsync();
        }

        private async Task EnsureCompanyExistsAsync(int companyId)
        {
            var companyExists = await _context.Companies
                .AnyAsync(c => c.Id == companyId);

            if (!companyExists)
                throw new Exception("Company not found");
        }

        private async Task EnsureRoleIsUniqueAsync(string name, int companyId)
        {
            var alreadyExists = await _context.Roles
                .AnyAsync(r => r.Name == name && r.CompanyId == companyId);

            if (alreadyExists)
                throw new Exception("Role already exists in this company");
        }
    }
}