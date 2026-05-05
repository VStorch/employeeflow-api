using System.ComponentModel.DataAnnotations;

namespace EmployeeFlow.DTOs.Department
{
    public record UpdateDepartmentRequest(
        
        [Required]
        [MaxLength(100)]
        string Name
    );
}