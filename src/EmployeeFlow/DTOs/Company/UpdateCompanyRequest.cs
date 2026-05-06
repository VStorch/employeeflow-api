using System.ComponentModel.DataAnnotations;

namespace EmployeeFlow.DTOs.Company
{
    public record UpdateCompanyRequest(
        
        [Required]
        [MaxLength(100)]
        string Name
    );
}