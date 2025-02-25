using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VwMaintenanceOffApproval
{
    public string MaintenanceOffId { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public string? FirstName { get; set; }

    public string LeaveTypeId { get; set; } = null!;

    public string LeaveShortName { get; set; } = null!;

    public int IsAccountable { get; set; }

    public string? FromDate { get; set; }

    public string? ToDate { get; set; }

    public string MaintenanceOffReason { get; set; } = null!;

    public string? ContactNumber { get; set; }

    public int ApprovalStatusId { get; set; }

    public string ApprovalStatusName { get; set; } = null!;

    public string ApprovalStaffId { get; set; } = null!;

    public string? ApprovalStaffName { get; set; }

    public string ApplicationApprovalId { get; set; } = null!;

    public string? ApprovedOnDate { get; set; }

    public string? ApprovedOnTime { get; set; }

    public string Comment { get; set; } = null!;

    public string Iscancelled { get; set; } = null!;

    public string Isflexible { get; set; } = null!;

    public string ApprovalOwner { get; set; } = null!;

    public DateTime? ApplicationDate { get; set; }

    public string? ApprovalOwnerName { get; set; }

    public int MoffYear { get; set; }
}
