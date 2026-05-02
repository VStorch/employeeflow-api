namespace EmployeeFlow.DTOs
{
    public record CreateEmployeeDTO(
        string Name, 
        string Email, 
        int DepartmentId, 
        int RoleId
    );
}