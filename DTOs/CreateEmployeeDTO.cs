namespace EmployeeFlow.DTOs
{
    public class CreateEmployeeDTO
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int DepartmentId { get; set; }
        public int RoleId { get; set; }
    }
}