public record UpdateEmployeeRequest(
    string Name,
    string Email,
    int DepartmentId,
    int RoleId
);