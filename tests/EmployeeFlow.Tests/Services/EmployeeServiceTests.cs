using EmployeeFlow.Services;
using EmployeeFlow.Tests.Common;
using FluentAssertions;

namespace EmployeeFlow.Tests.Services
{
    public class EmployeeServiceTests : TestBase
    {
        private readonly EmployeeService _service;

        public EmployeeServiceTests()
        {
            _service = new EmployeeService(Context, Mapper);
            Seed();
        }

        private void Seed()
        {
            var company = TestDataFactory.CreateCompany();
            var department = TestDataFactory.CreateDepartment();
            var role = TestDataFactory.CreateRole();

            Context.Companies.Add(company);
            Context.Departments.Add(department);
            Context.Roles.Add(role);

            Context.SaveChanges();
        }

        [Fact]
        public async Task CreateAsync_ShouldCreateEmployee_WhenValid()
        {
            var dto = TestDataFactory.CreateEmployeeRequest();

            var result = await _service.CreateAsync(1, dto);

            result.Should().NotBeNull();
            result.Email.Should().Be(dto.Email);
        }
    }
}