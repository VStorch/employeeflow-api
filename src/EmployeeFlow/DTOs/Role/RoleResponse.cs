namespace EmployeeFlow.DTOs.Role
{
    public record RoleResponse(
        int Id,
        string Name,
        int CompanyId
    );
}