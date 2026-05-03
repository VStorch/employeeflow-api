namespace EmployeeFlow.DTOs.Role
{
    public record CreateRoleRequest(
        string Name,
        int CompanyId
    );
}