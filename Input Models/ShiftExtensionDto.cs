namespace AttendanceManagement.Input_Models
{
    public class ShiftExtensionDto
    {
        public int ApplicationTypeId { get; set; }
        public int? StaffId { get; set; }
        public DateOnly TransactionDate { get; set; }
        public string? DurationHours { get; set; }
        public DateTime? BeforeShiftHours { get; set; }
        public DateTime? AfterShiftHours { get; set; }
        public string? Remarks { get; set; }
        public int CreatedBy { get; set; }
    }

}