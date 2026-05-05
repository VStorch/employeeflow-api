using System.ComponentModel.DataAnnotations;

namespace EmployeeFlow.DTOs.Auth
{
    public record LoginRequest
    (
        [Required]
        [EmailAddress]
        string Email,

        [Required]
        [StringLength(100, MinimumLength = 6)]
        string Password
    );
}