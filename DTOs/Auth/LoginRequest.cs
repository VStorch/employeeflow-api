namespace EmployeeFlow.DTOs.Auth
{
    public record LoginRequest
    (
        string Email,
        string Password
    );
}