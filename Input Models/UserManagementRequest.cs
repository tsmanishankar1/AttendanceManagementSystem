namespace AttendanceManagement.Input_Models
{
    public class UserManagementRequest
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int CreatedBy { get; set; }

        public int StaffCreationId { get; set; }
    }

    public class UserManagementResponse
    {
        public int UserManagementId { get; set; }
        public string StaffName { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
        public int CreatedBy { get; set; }
    }
}
