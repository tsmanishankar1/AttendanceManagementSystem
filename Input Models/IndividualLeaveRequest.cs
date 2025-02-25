namespace AttendanceManagement.Input_Models
{
    public class IndividualLeaveRequest
    {
        public int LeaveTypeId { get; set; }

        public int DepartmentId { get; set; }
        public int StaffCreationId {  get; set; }

        public bool TransactionFlag { get; set; }

        public string LeaveReason { get; set; } = null!;

        public string Month { get; set; } = null!;

        public int Year { get; set; }

        public decimal LeaveCount { get; set; }
         public bool Isactive { get; set; }

        public string? Remarks { get; set; }

        public decimal? ActualBalance { get; set; }

        public decimal? AvailableBalance { get; set; }

        public int CreatedBy { get; set; }
    }
    public class LeaveCreditDebitRequest
    {
        public IEnumerable<SelectedStaff> SelectedRows { get; set; } = null!;
        public int LeaveTypeId { get; set; }
        public bool TransactionFlag { get; set; }
        public string LeaveReason { get; set; } = null!;
        public string Month { get; set; } = null!;
        public int Year { get; set; }
        public decimal LeaveCount { get; set; }
        public string? Remarks { get; set; }
        public int CreatedBy { get; set; }
    }
    public class SelectedStaff
    {
        public int StaffId { get; set; }
    }
}
