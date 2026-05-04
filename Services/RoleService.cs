using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmployeeFlow.Data;
using EmployeeFlow.DTOs.Role;
using EmployeeFlow.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeFlow.Services
{
    public class RoleService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public RoleService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<RoleResponse> CreateAsync(CreateRoleRequest dto)
        {
            await EnsureCompanyExistsAsync(dto.CompanyId);
            await EnsureRoleIsUniqueAsync(dto.Name, dto.CompanyId);

            var role = _mapper.Map<Role>(dto);

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return await _context.Roles
                .AsNoTracking()
                .Where(r => r.Id == role.Id)
                .ProjectTo<RoleResponse>(_mapper.ConfigurationProvider)
                .SingleAsync();
        }

        public async Task<List<RoleResponse>> GetByCompanyAsync(int companyId)
        {
            await EnsureCompanyExistsAsync(companyId);

            return await _context.Roles
                .AsNoTracking()
                .Where(r => r.CompanyId == companyId)
                .ProjectTo<RoleResponse>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<RoleResponse?> GetByIdAsync(int id, int companyId)
        {
            return await _context.Roles
                .AsNoTracking()
                .Where(r => r.Id == id && r.CompanyId == companyId)
                .ProjectTo<RoleResponse>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<RoleResponse> UpdateAsync(int id, int companyId, UpdateRoleRequest dto)
        {
            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == id && r.CompanyId == companyId);

            if (role is null)
                throw new KeyNotFoundException("Role not found.");

            await EnsureRoleIsUniqueAsync(dto.Name, companyId, id);

            _mapper.Map(dto, role);

            await _context.SaveChangesAsync();

            return await _context.Roles
                .AsNoTracking()
                .Where(r => r.Id == id)
                .ProjectTo<RoleResponse>(_mapper.ConfigurationProvider)
                .SingleAsync();
        }

        public async Task DeleteAsync(int id, int companyId)
        {
            var role = await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == id && r.CompanyId == companyId);

            if (role is null)
                throw new KeyNotFoundException("Role not found.");

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }

        private async Task EnsureCompanyExistsAsync(int companyId)
        {
            if (!await _context.Companies.AnyAsync(c => c.Id == companyId))
                throw new KeyNotFoundException("Company not found.");
        }

        private async Task EnsureRoleIsUniqueAsync(string name, int companyId, int? ignoreId = null)
        {
            var exists = await _context.Roles
                .AnyAsync(r =>
                    r.Name == name &&
                    r.CompanyId == companyId &&
                    (!ignoreId.HasValue || r.Id != ignoreId));

            if (exists)
                throw new ArgumentException("Role already exists in this company.");
        }
    }
}