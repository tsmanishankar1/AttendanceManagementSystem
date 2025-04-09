namespace AttendanceManagement.Atrak_InputModels
{
    public class SmaxTransactionResponse
    {
        public int? StaffId { get; set; }
        public string? StaffCreationId { get; set; }
        public DateTime? Date { get; set; }
        public DateTime? CheckInTime { get; set; }
        public DateTime? CheckOutTime { get; set; }
        public string? ReaderNameIn { get; set; }
        public string? ReaderNameOut { get; set; }
    }
}
