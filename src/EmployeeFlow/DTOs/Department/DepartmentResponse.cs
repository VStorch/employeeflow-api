namespace EmployeeFlow.DTOs.Department
{
    public record DepartmentResponse(
        int Id,
        string Name,
        int CompanyId
    );
}