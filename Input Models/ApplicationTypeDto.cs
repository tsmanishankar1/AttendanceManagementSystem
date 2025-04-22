using System.ComponentModel.DataAnnotations;

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
    public class CancelAppliedLeave
    {
        public int ApplicationTypeId { get; set; }
        public int Id { get; set; }
        public bool IsCancelled { get; set; }
        public int UpdatedBy { get;set; }
    }
    public class ApplicationDetails
    {
        public int StaffId { get; set; }
        public int ApplicationTypeId { get; set; }
    }
    public class MonthlyCalendar
    {
        public int StaffId { get; set; }
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
        public decimal TotalDays { get; set; }
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
        public decimal TotalDays { get; set; }
    }
    public class CompOffCreditDto
    {
        public int ApplicationTypeId { get; set; }
        public int? StaffId { get; set; }
        public DateOnly WorkedDate { get; set; }
        public decimal TotalDays { get; set; }
        public string Reason { get; set; } = null!;
        public int CreatedBy { get; set; }
    }
    public class CompOffCreditResponseDto
    {
        public int ApplicationTypeId { get; set; }
        public int Id { get; set; }
        public DateOnly WorkedDate { get; set; }
        public decimal TotalDays { get; set; }
        public string Reason { get; set; } = null!;
    }
    public class ReimbursementRequestModel
    {
        public DateOnly BillDate { get; set; }
        public string BillNo { get; set; } = null!;
        public string? Description { get; set; }
        public string BillPeriod { get; set; } = null!;
        public decimal Amount { get; set; }
        public int ApplicationTypeId { get; set; }
        public IFormFile File { get; set; } 
        public int CreatedBy { get; set; }
        public int? StaffId { get; set; }
        public int ReimbursementTypeId { get; set; }
    }

    public class ReimbursementResponse
    {
        public int Id { get; set; }

        public DateOnly BillDate { get; set; }

        public string BillNo { get; set; } = null!;

        public string? Description { get; set; }

        public string BillPeriod { get; set; } = null!;

        public decimal Amount { get; set; }
        public string ReimbursementType { get; set; } = null!;
        public string UploadFilePath { get; set; } = null!;
    }
}