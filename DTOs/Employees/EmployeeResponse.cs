namespace EmployeeFlow.DTOs.Employees
{
    public record EmployeeResponse
    (
        int Id,
        string Name,
        string Email,
        string Department,
        string Role
    );
}