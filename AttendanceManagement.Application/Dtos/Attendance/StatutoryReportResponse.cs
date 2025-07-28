namespace AttendanceManagement.Application.Dtos.Attendance
{
    public class StatutoryReportResponse
    {
        public int StaffId { get; set; }
        public decimal NoOfDaysInMonth { get; set; }
        public decimal SumOfPresent { get; set; }
        public decimal NoOfDaysAbsent { get; set; }
        public decimal SumOfCl { get; set; }
        public decimal SumOfSl { get; set; }
        public decimal SumOfPl { get; set; }
        public decimal SumOfNcl { get; set; }
        public decimal SumOfMgl { get; set; }
        public decimal TotalLeaves { get; set; }
        public decimal Lop {  get; set; }
        public decimal NumberOfDaysToBePaid { get; set; }
        public decimal NightShiftPresentDays { get; set; }
        public bool IsAttendanceBonus { get; set; }
        public bool AttendanceBonusMonths { get; set; }
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
        public decimal SumOfPresent { get; set; }
        public decimal NoOfDaysAbsent { get; set; }
        public decimal SumOfCl { get; set; }
        public decimal SumOfSl { get; set; }
        public decimal SumOfPl { get; set; }
        public decimal SumOfNcl { get; set; }
        public decimal SumOfMgl { get; set; }
        public decimal TotalLeaves { get; set; }
        public decimal Lop { get; set; }
        public decimal NumberOfDaysToBePaid { get; set; }
        public decimal NightShiftPresentDays { get; set; }
        public bool IsAttendanceBonus { get; set; }
        public bool? AttendanceBonusMonths { get; set; }
    }
}