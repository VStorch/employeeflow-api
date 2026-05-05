using System.ComponentModel.DataAnnotations;

namespace EmployeeFlow.DTOs.Role
{
    public record UpdateRoleRequest(
        
        [Required]
        [MaxLength(100)]
        string Name
    );
}