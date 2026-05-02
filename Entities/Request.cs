namespace EmployeeFlow.Entities
{
    public class Request
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;

        public RequestStatus Status { get; set; } = RequestStatus.Pending;

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}