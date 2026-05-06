namespace EmployeeFlow.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public int CompanyId { get; set; }
        public Company Company { get; set; } = null!;

        public List<Employee> Employees { get; set; } = new();
    }
}