using System.ComponentModel.DataAnnotations;

namespace EmployeeFlow.DTOs.Company
{
    public record CreateCompanyRequest
    (
        [Required]
        [MaxLength(100)]
        string Name,

        [Required]
        [EmailAddress]
        string Email,

        [Required]
        [StringLength(100, MinimumLength = 6)]
        string Password
    );
}