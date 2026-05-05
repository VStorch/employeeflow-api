using EmployeeFlow.Data;
using EmployeeFlow.DTOs.Company;
using EmployeeFlow.Entities;
using Microsoft.EntityFrameworkCore;

namespace EmployeeFlow.Services
{
    public class CompanyService
    {
        private readonly AppDbContext _context;

        public CompanyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CompanyResponse> CreateAsync(CreateCompanyRequest dto)
        {
            var company = new Company
            {
                Name = dto.Name
            };

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Company = company
            };

            _context.Users.Add(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                throw new ConflictException("This email is already registered.");
            }

            return new CompanyResponse(company.Id, company.Name);
        }

        public async Task<CompanyResponse?> GetByIdAsync(int companyId)
        {
            return await _context.Companies
                .Where(c => c.Id == companyId)
                .Select(c => new CompanyResponse(c.Id, c.Name))
                .FirstOrDefaultAsync();
        }

        public async Task<CompanyResponse?> UpdateAsync(int companyId, UpdateCompanyRequest dto)
        {
            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null) return null;

            company.Name = dto.Name;

            await _context.SaveChangesAsync();

            return new CompanyResponse(company.Id, company.Name);
        }

        public async Task<bool> DeleteAsync(int companyId)
        {
            var company = await _context.Companies
                .FirstOrDefaultAsync(c => c.Id == companyId);

            if (company == null) return false;

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}