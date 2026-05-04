namespace EmployeeFlow.DTOs
{
    public record CreateEmployeeRequest(
        string Name,
        string Email,
        int CompanyId,
        int DepartmentId,
        int RoleId
    );
}