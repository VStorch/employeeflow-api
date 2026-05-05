namespace EmployeeFlow.DTOs
{
    public record CreateEmployeeRequest(
        string Name,
        string Email,
        int DepartmentId,
        int RoleId
    );
}