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

            _context.Companies.Add(company);
            await _context.SaveChangesAsync();

            var user = new User
            {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CompanyId = company.Id
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new CompanyResponse(company.Id, company.Name);
        }

        public async Task<List<CompanyResponse>> GetAllAsync()
        {
            return await _context.Companies
                .Select(c => new CompanyResponse(c.Id, c.Name))
                .ToListAsync();
        }

        public async Task<CompanyResponse?> GetByIdAsync(int id)
        {
            return await _context.Companies
                .Where(c => c.Id == id)
                .Select(c => new CompanyResponse(c.Id, c.Name))
                .FirstOrDefaultAsync();
        }

        public async Task<CompanyResponse?> UpdateAsync(int id, UpdateCompanyRequest dto)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null) return null;

            company.Name = dto.Name;

            await _context.SaveChangesAsync();

            return new CompanyResponse(company.Id, company.Name);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var company = await _context.Companies.FindAsync(id);

            if (company == null) return false;

            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}