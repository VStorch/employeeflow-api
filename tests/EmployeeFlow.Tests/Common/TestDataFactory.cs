using EmployeeFlow.DTOs.Employees;
using EmployeeFlow.Entities;

namespace EmployeeFlow.Tests.Common
{
    public static class TestDataFactory
    {
        public static Company CreateCompany(int id = 1)
            => new Company
            {
                Id = id,
                Name = "Test Company"
            };

        public static Department CreateDepartment(int companyId = 1, int id = 1)
            => new Department
            {
                Id = id,
                CompanyId = companyId,
                Name = "IT"
            };

        public static Role CreateRole(int companyId = 1, int id = 1)
            => new Role
            {
                Id = id,
                CompanyId = companyId,
                Name = "Developer"
            };

        public static CreateEmployeeRequest CreateEmployeeRequest()
            => new CreateEmployeeRequest(
                Name: "Test User",
                Email: "test@test.com",
                DepartmentId: 1,
                RoleId: 1
            );
    }
}