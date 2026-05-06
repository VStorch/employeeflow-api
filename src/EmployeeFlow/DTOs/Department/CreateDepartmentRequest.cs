using System.ComponentModel.DataAnnotations;

namespace EmployeeFlow.DTOs.Department
{
    public record CreateDepartmentRequest(
        
        [Required]
        [MaxLength(100)]
        string Name
    );
}