namespace EmployeeFlow.DTOs.Company
{
    public record CreateCompanyRequest
    (
        string Name,
        string Email,
        string Password
    );
}