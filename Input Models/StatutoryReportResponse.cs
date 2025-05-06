namespace AttendanceManagement.Input_Models
{
    public class StatutoryReportResponse
    {
        public int StaffId { get; set; }
        public decimal NoOfDaysInMonth { get; set; }
        public decimal Lop {  get; set; }
        public decimal NumberOfDaysToBePaid { get; set; }
        public decimal NightShiftPresentDays { get; set; }
        public bool IsAttendanceBonus { get; set; }
        public decimal AttendanceBonusMonths { get; set; }
        public DateOnly ReportFromDate { get; set; }
        public DateOnly ReportToDate { get; set; }
    }

    public class StatutoryReportRequest
    {
        public List<int> StaffIds { get; set; } = new List<int>();
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public int CreatedBy {  get; set; }
    }

    public class StatutoryReportDto
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Department {  get; set; } = null!;
        public string Designation { get; set; } = null!;
        public List<KeyValuePair<string, string>> DayWiseAttendance { get; set; } = new();
        public decimal NoOfDaysInMonth { get; set; }
        public decimal Lop { get; set; }
        public decimal NumberOfDaysToBePaid { get; set; }
        public decimal NightShiftPresentDays { get; set; }
        public bool IsAttendanceBonus { get; set; }
        public decimal? AttendanceBonusMonths { get; set; }
    }
}