namespace AttendanceManagement.Application.Dtos.Attendance
{
    public class StaffDto
    {
        public int Id { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string StaffName { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
        public string? ProfilePhoto { get; set; }
    }
}