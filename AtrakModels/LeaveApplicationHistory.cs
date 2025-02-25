using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class LeaveApplicationHistory
{
    public string? LeaveApplicationId { get; set; }

    public string? StaffId { get; set; }

    public string? LeaveTypeId { get; set; }

    public int? DurationId { get; set; }

    public string? ApprovalStatusById { get; set; }

    public int LeaveApprovalStatus { get; set; }

    public string? LeaveStartDate { get; set; }

    public string? LeaveEndDate { get; set; }

    public DateTime? ApplicationDate { get; set; }

    public string? LeaveReason { get; set; }

    public string? ContactNumber { get; set; }

    public bool? IsCancelled { get; set; }

    public string Comment { get; set; } = null!;

    public string? Leavename { get; set; }

    public string? DurationName { get; set; }

    public string ApprovalStatusName { get; set; } = null!;

    public string? Applicantname { get; set; }

    public string? Approvalstatusbyname { get; set; }

    public int Id { get; set; }

    public bool? IsPermission { get; set; }
}
