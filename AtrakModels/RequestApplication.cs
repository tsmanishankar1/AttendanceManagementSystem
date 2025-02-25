using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class RequestApplication
{
    public string Id { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string? LeaveTypeId { get; set; }

    public int LeaveStartDurationId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int LeaveEndDurationId { get; set; }

    public decimal TotalDays { get; set; }

    public string? PermissionType { get; set; }

    public string? Otrange { get; set; }

    public string? Odduration { get; set; }

    public string? NewShiftId { get; set; }

    public int Rhid { get; set; }

    public DateTime? TotalHours { get; set; }

    public string? Remarks { get; set; }

    public int ReasonId { get; set; }

    public string? ContactNumber { get; set; }

    public string? PunchType { get; set; }

    public DateTime? ApplicationDate { get; set; }

    public string AppliedBy { get; set; } = null!;

    public bool IsCancelled { get; set; }

    public DateTime? CancelledDate { get; set; }

    public string? CancelledBy { get; set; }

    public bool IsApproved { get; set; }

    public bool IsRejected { get; set; }

    public string RequestApplicationType { get; set; } = null!;

    public bool IsCancelApprovalRequired { get; set; }

    public bool IsCancelApproved { get; set; }

    public bool IsCancelRejected { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public DateTime? WorkedDate { get; set; }

    public string? Ottype { get; set; }

    public string? ShiftExtensionType { get; set; }

    public string? DurationOfHoursExtension { get; set; }

    public DateTime? HoursBeforeShift { get; set; }

    public DateTime? HoursAfterShift { get; set; }

    public virtual ICollection<DocumentUpload> DocumentUploads { get; set; } = new List<DocumentUpload>();
}
