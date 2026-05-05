using AutoMapper;
using AutoMapper.QueryableExtensions;
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

        public async Task<EmployeeResponse> CreateAsync(int companyId, CreateEmployeeRequest dto)
        {
            await ValidateRelationsAsync(companyId, dto.DepartmentId, dto.RoleId);

            var employee = _mapper.Map<Employee>(dto);
            employee.CompanyId = companyId;

            try
            {
                _context.Employees.Add(employee);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (IsUniqueConstraintViolation(ex, "IX_Employees_Email_CompanyId"))
                    throw new ConflictException("An employee with this email already exists in this company.");

                throw;
            }

            return await _context.Employees
                .AsNoTracking()
                .Where(e => e.Id == employee.Id && e.CompanyId == companyId)
                .ProjectTo<EmployeeResponse>(_mapper.ConfigurationProvider)
                .SingleAsync();
        }

        public async Task<List<EmployeeResponse>> GetAllAsync(int companyId, int? departmentId = null, int? roleId = null)
        {
            var query = _context.Employees
                .AsNoTracking()
                .Where(e => e.CompanyId == companyId);

            if (departmentId.HasValue)
                query = query.Where(e => e.DepartmentId == departmentId);

            if (roleId.HasValue)
                query = query.Where(e => e.RoleId == roleId);

            return await query
                .ProjectTo<EmployeeResponse>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<EmployeeResponse?> GetByIdAsync(int id, int companyId)
        {
            return await _context.Employees
                .AsNoTracking()
                .Where(e => e.Id == id && e.CompanyId == companyId)
                .ProjectTo<EmployeeResponse>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<EmployeeResponse> UpdateAsync(int id, int companyId, UpdateEmployeeRequest dto)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == id && e.CompanyId == companyId);

            if (employee is null)
                throw new KeyNotFoundException("Employee not found.");

            await ValidateRelationsAsync(companyId, dto.DepartmentId, dto.RoleId);

            _mapper.Map(dto, employee);
            employee.CompanyId = companyId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (IsUniqueConstraintViolation(ex, "IX_Employees_Email_CompanyId"))
                    throw new ConflictException("An employee with this email already exists in this company.");

                throw;
            }

            return await _context.Employees
                .AsNoTracking()
                .Where(e => e.Id == id && e.CompanyId == companyId)
                .ProjectTo<EmployeeResponse>(_mapper.ConfigurationProvider)
                .SingleAsync();
        }

        public async Task DeleteAsync(int id, int companyId)
        {
            var employee = await _context.Employees
                .FirstOrDefaultAsync(e => e.Id == id && e.CompanyId == companyId);

            if (employee is null)
                throw new KeyNotFoundException("Employee not found.");

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
        }

        private async Task ValidateRelationsAsync(int companyId, int departmentId, int roleId)
        {
            var validation = await _context.Companies
                .Where(c => c.Id == companyId)
                .Select(c => new
                {
                    CompanyExists = true,
                    DepartmentValid = c.Departments.Any(d => d.Id == departmentId),
                    RoleValid = c.Roles.Any(r => r.Id == roleId)
                })
                .FirstOrDefaultAsync();

            if (validation == null)
                throw new KeyNotFoundException("Company not found.");

            if (!validation.DepartmentValid)
                throw new ArgumentException("Department not found or does not belong to this company.");

            if (!validation.RoleValid)
                throw new ArgumentException("Role not found or does not belong to this company.");
        }

        private static bool IsUniqueConstraintViolation(DbUpdateException ex, string indexName)
        {
            return ex.InnerException?.Message.Contains(indexName) == true;
        }
    }
}