namespace AttendanceManagement.Atrak_InputModels
{
    public class SmaxTransactionResponse
    {
        public int? StaffId { get; set; }
        public string? StaffCreationId { get; set; }
        public string ShiftName { get; set; } = null!;
        public string? Date { get; set; }
        public string? CheckInTime { get; set; }
        public string? CheckOutTime { get; set; }
        public string? ReaderNameIn { get; set; }
        public string? ReaderNameOut { get; set; }
        public string? Duration { get; set; }
    }
}