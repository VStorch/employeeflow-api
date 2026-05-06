using System.ComponentModel.DataAnnotations;

namespace EmployeeFlow.DTOs.Employees
{
    public record CreateEmployeeRequest(
        
        [Required]
        [MaxLength(100)]
        string Name,

        [Required]
        [EmailAddress]
        string Email,

        [Required]
        int DepartmentId,

        [Required]
        int RoleId
    );
}