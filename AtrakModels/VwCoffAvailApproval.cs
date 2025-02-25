using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwCoffAvailApproval
{
    public string CoffId { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LeaveTypeId { get; set; } = null!;

    public decimal TotalDays { get; set; }

    public string LeaveShortName { get; set; } = null!;

    public int IsAccountable { get; set; }

    public string? StartDate { get; set; }

    public string StartDuration { get; set; } = null!;

    public string? EndDate { get; set; }

    public string EndDuration { get; set; } = null!;

    public string? CoffReason { get; set; }

    public int Approval1StatusId { get; set; }

    public string Approval1StatusName { get; set; } = null!;

    public string Approval1StaffId { get; set; } = null!;

    public string ApprovalStaffName { get; set; } = null!;

    public string ApplicationApprovalId { get; set; } = null!;

    public string? Approved1OnDate { get; set; }

    public string? Approved1OnTime { get; set; }

    public string? Comment { get; set; }

    public string Approval1Owner { get; set; } = null!;

    public string ParentType { get; set; } = null!;

    public int Approval2statusId { get; set; }

    public string? Approval2statusName { get; set; }

    public string? Approval2Owner { get; set; }

    public string? Approval2By { get; set; }

    public string? Approval2On { get; set; }

    public string IsApprover1Cancelled { get; set; } = null!;

    public string IsApprover2Cancelled { get; set; } = null!;

    public string IsCancelled { get; set; } = null!;

    public string AppliedBy { get; set; } = null!;

    public DateTime? WorkedDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public DateTime? ApplicationDate { get; set; }

    public bool IsRejected { get; set; }
}
