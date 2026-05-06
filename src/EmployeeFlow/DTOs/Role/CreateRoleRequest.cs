using System.ComponentModel.DataAnnotations;

namespace EmployeeFlow.DTOs.Role
{
    public record CreateRoleRequest(
        
        [Required]
        [MaxLength(100)]
        string Name
    );
}