using EmployeeFlow.DTOs.Employees;
using EmployeeFlow.Services;
using EmployeeFlow.Tests.Common;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

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

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenDepartmentInvalid()
        {
            var dto = new CreateEmployeeRequest(
                Name: "Test User",
                Email: "test@test.com",
                DepartmentId: 999,
                RoleId: 1
            );

            Func<Task> act = async () => await _service.CreateAsync(1, dto);

            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage("Department not found or does not belong to this company.");
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenRoleInvalid()
        {
            var dto = new CreateEmployeeRequest(
                Name: "Test User",
                Email: "test@test.com",
                DepartmentId: 1,
                RoleId: 999
            );

            Func<Task> act = async () => await _service.CreateAsync(1, dto);

            await act.Should()
                .ThrowAsync<ArgumentException>()
                .WithMessage("Role not found or does not belong to this company.");
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenCompanyInvalid()
        {
            var dto = TestDataFactory.CreateEmployeeRequest();

            Func<Task> act = async () => await _service.CreateAsync(999, dto);

            await act.Should()
                .ThrowAsync<KeyNotFoundException>()
                .WithMessage("Company not found.");
        }

        [Fact]
        public async Task CreateAsync_ShouldThrow_WhenEmailAlreadyExists()
        {
            var dto = TestDataFactory.CreateEmployeeRequest();

            await _service.CreateAsync(1, dto);

            Func<Task> act = async () => await _service.CreateAsync(1, dto);

            var exception = await act.Should().ThrowAsync<DbUpdateException>();

            exception.Which.InnerException!.Message
                .Should().Contain("UNIQUE");
        }
    }
}