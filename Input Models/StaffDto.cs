namespace AttendanceManagement.Input_Models
{
    public class StaffDto
    {
        public int Id { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string StaffName { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
    }
}
