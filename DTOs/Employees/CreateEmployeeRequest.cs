namespace EmployeeFlow.DTOs
{
    public record CreateEmployeeDTO(
        string Name,
        string Email,
        int CompanyId,
        int DepartmentId,
        int RoleId
    );
}