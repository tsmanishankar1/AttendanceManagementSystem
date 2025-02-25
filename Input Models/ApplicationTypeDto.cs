namespace AttendanceManagement.Input_Models
{
    public class ApplicationTypeDto
    {
        public int ApplicationTypeId { get; set; }
        public string ApplicationTypeName { get; set; } = null!;
    }

    public class ApplicationTypeRequest
    {
        public string ApplicationTypeName { get; set; } = null!;
    }

    public class ApplicationDetails
    {
        public int StaffId { get; set; }
        public int ApplicationTypeId { get; set; }
    }
    public class MonthlyCalendar
    {
        public string StaffId { get; set; } = null!;
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class GetLeaveRequest
    {
        public int ApproverId { get; set; }
        public List<int>? StaffIds { get; set; } = new List<int>();
        public int? ApplicationTypeId { get; set; }
        public DateOnly? FromDate { get; set; }
        public DateOnly? ToDate { get; set; }
    }
    public class OnBehalfApplicationRequest
    {
        public string Criteria { get; set; } = null!;

        public DateOnly? FromDate { get; set; }
        public bool IsActive { get; set; }

        public int ApplicationTypeId { get; set; }

        public int CreatedBy { get; set; }

        public int StaffCreationId { get; set; }
    }

    public class OnBehalfApplicationResponse
    {
        public int ApplicationApprovalId { get; set; }
        public string Criteria { get; set; } = null!;

        public DateOnly? FromDate { get; set; }

        public string ApplicationTypeName { get; set; } = null!;

        public string StaffCreationName { get; set; } = null!;
        public int CreatedBy { get; set; }
    }
    public class CompOffAvailRequest
    {
        public int ApplicationTypeId { get; set; }
        public int? StaffId { get; set; }
        public DateOnly WorkedDate { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public string FromDuration { get; set; } = null!;
        public string ToDuration { get; set; } = null!;
        public string Reason { get; set; } = null!;
        public int TotalDays { get; set; }
        public int CreatedBy { get; set; }
    }
    public class CompOffAvailDto
    {
        public int Id { get; set; }
        public int? staffId { get; set; }
        public int ApplicationTypeId { get; set; }
        public DateOnly WorkedDate { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        public string FromDuration { get; set; } = null!;
        public string ToDuration { get; set; } = null!;
        public string Reason { get; set; } = null!;
        public int TotalDays { get; set; }
    }
    public class CompOffCreditDto
    {
        public int ApplicationTypeId { get; set; }
        public int? StaffId { get; set; }
        public DateOnly WorkedDate { get; set; }
        public int TotalDays { get; set; }
        public string Reason { get; set; } = null!;
        public int CreatedBy { get; set; }
    }
    public class CompOffCreditResponseDto
    {
        public int ApplicationTypeId { get; set; }
        public int Id { get; set; }
        public DateOnly WorkedDate { get; set; }
        public int TotalDays { get; set; }
        public string Reason { get; set; } = null!;
    }
}