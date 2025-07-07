namespace AttendanceManagement.Application.Dtos.Attendance
{
    public class AnnouncementDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class AnnouncementResponse
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateOnly Date {  get; set; }
    }
}
