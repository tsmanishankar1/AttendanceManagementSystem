using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwLeaveApplicationApproval
{
    public string LeaveApplicationId { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LeaveTypeId { get; set; }

    public string LeaveTypeName { get; set; } = null!;

    public string LeaveShortName { get; set; } = null!;

    public bool IsAccountable { get; set; }

    public string? LeaveStartDate { get; set; }

    public string? LeaveStartDurationId { get; set; }

    public string? LeaveStartDurationName { get; set; }

    public string? LeaveEndDate { get; set; }

    public string? LeaveEndDurationId { get; set; }

    public string? LeaveEndDurationName { get; set; }

    public string? Remarks { get; set; }

    public string LeaveApplicationReason { get; set; } = null!;

    public string? ContactNumber { get; set; }

    public string? Approval1StatusId { get; set; }

    public string? Approval2StatusId { get; set; }

    public string? Approval1StatusName { get; set; }

    public string? Approval2StatusName { get; set; }

    public string? Approval1StaffId { get; set; }

    public string? Approval1StaffName { get; set; }

    public string? ApplicationApprovalId { get; set; }

    public string? ApprovedOn1Date { get; set; }

    public string? ApprovedOn1Time { get; set; }

    public string? Comment { get; set; }

    public string AppliedBy { get; set; } = null!;

    public string Approval1Owner { get; set; } = null!;

    public string? Approval2Owner { get; set; }

    public string? Approval2OwnerName { get; set; }

    public string? Approval1OwnerName { get; set; }

    public string IsCancelled { get; set; } = null!;

    public string ParentType { get; set; } = null!;

    public DateTime? ApplicationDate { get; set; }

    public decimal TotalDays { get; set; }

    public string? Approval2On { get; set; }

    public string IsApprover1Cancelled { get; set; } = null!;

    public string IsApprover2Cancelled { get; set; } = null!;

    public int? IsDocumentAvailable { get; set; }
}
