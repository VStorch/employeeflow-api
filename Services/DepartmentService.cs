using AutoMapper;
using AutoMapper.QueryableExtensions;
using EmployeeFlow.Data;
using EmployeeFlow.DTOs.Department;
using EmployeeFlow.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeFlow.Services
{
    public class DepartmentService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public DepartmentService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<DepartmentResponse> CreateAsync(CreateDepartmentRequest dto)
        {
            await EnsureCompanyExistsAsync(dto.CompanyId);
            await EnsureDepartmentIsUniqueAsync(dto.Name, dto.CompanyId);

            var department = _mapper.Map<Department>(dto);

            _context.Departments.Add(department);
            await _context.SaveChangesAsync();

            return await _context.Departments
                .AsNoTracking()
                .Where(d => d.Id == department.Id)
                .ProjectTo<DepartmentResponse>(_mapper.ConfigurationProvider)
                .SingleAsync();
        }

        public async Task<List<DepartmentResponse>> GetByCompanyAsync(int companyId)
        {
            await EnsureCompanyExistsAsync(companyId);

            return await _context.Departments
                .AsNoTracking()
                .Where(d => d.CompanyId == companyId)
                .ProjectTo<DepartmentResponse>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<DepartmentResponse?> GetByIdAsync(int id, int companyId)
        {
            return await _context.Departments
                .AsNoTracking()
                .Where(d => d.Id == id && d.CompanyId == companyId)
                .ProjectTo<DepartmentResponse>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<DepartmentResponse> UpdateAsync(int id, int companyId, UpdateDepartmentRequest dto)
        {
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.Id == id && d.CompanyId == companyId);

            if (department is null)
                throw new KeyNotFoundException("Department not found.");

            await EnsureDepartmentIsUniqueAsync(dto.Name, companyId, id);

            _mapper.Map(dto, department);

            await _context.SaveChangesAsync();

            return await _context.Departments
                .AsNoTracking()
                .Where(d => d.Id == id)
                .ProjectTo<DepartmentResponse>(_mapper.ConfigurationProvider)
                .SingleAsync();
        }

        public async Task DeleteAsync(int id, int companyId)
        {
            var department = await _context.Departments
                .FirstOrDefaultAsync(d => d.Id == id && d.CompanyId == companyId);

            if (department is null)
                throw new KeyNotFoundException("Department not found.");

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
        }

        private async Task EnsureCompanyExistsAsync(int companyId)
        {
            if (!await _context.Companies.AnyAsync(c => c.Id == companyId))
                throw new KeyNotFoundException("Company not found.");
        }

        private async Task EnsureDepartmentIsUniqueAsync(string name, int companyId, int? ignoreId = null)
        {
            var exists = await _context.Departments
                .AnyAsync(d =>
                    d.Name == name &&
                    d.CompanyId == companyId &&
                    (!ignoreId.HasValue || d.Id != ignoreId));

            if (exists)
                throw new ArgumentException("Department already exists in this company.");
        }
    }
}