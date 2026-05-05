namespace EmployeeFlow.Entities
{
    public class Company
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public List<User> Users { get; set; } = new();

        public List<Employee> Employees { get; set; } = new();
        public List<Department> Departments { get; set; } = new();
        public List<Role> Roles { get; set; } = new();
    }
}