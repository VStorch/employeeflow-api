namespace EmployeeFlow.DTOs.Employees
{
    public record EmployeeResponse
    (
        int Id,
        string Name,
        string Email,
        int DepartmentId,
        string Department,
        int RoleId,
        string Role,
        int CompanyId
    );
}