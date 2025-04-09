namespace AttendanceManagement.Input_Models
{
    public class AttendanceRecordDto
    {
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string StaffName { get; set; } = null!;
        public DateTime? FirstIn { get; set; }
        public DateTime? LastOut { get; set; }
        public int? ShiftId { get; set; }
        public bool? IsEarlyComing { get; set; }
        public bool? IsLateComing { get; set; }
        public bool? IsEarlyGoing { get; set; }
        public bool? IsLateGoing { get; set; }
        public decimal? BreakHours { get; set; }
        public bool? IsBreakHoursExceed { get; set; }
        public decimal? ExtraBreakHours { get; set; }
        public int StatusId { get; set; }
        public bool IsHolidayWorkingEligible { get; set; }
        public int? Norm {  get; set; }
        public int? CompletedFileCouunt {  get; set; }
        public decimal? TotalFte { get; set; }
        public bool? IsFteAchieved {  get; set; }
        public DateOnly? AttendanceDate { get; set; }
    }
}
