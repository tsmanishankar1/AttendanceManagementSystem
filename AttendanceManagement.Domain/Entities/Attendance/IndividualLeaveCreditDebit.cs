using System;
using System.Collections.Generic;

namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class IndividualLeaveCreditDebit
{
    public int Id { get; set; }

    public int LeaveTypeId { get; set; }

    public bool TransactionFlag { get; set; }

    public string LeaveReason { get; set; } = null!;

    public string Month { get; set; } = null!;

    public int Year { get; set; }

    public decimal LeaveCount { get; set; }

    public string? Remarks { get; set; }

    public decimal ActualBalance { get; set; }

    public decimal AvailableBalance { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int StaffCreationId { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual LeaveType LeaveType { get; set; } = null!;

    public virtual StaffCreation StaffCreation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
