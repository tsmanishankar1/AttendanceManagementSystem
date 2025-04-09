using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class ApprovalNotification
{
    public int Id { get; set; }

    public int StaffId { get; set; }

    public string Message { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual ICollection<BusinessTravel> BusinessTravels { get; set; } = new List<BusinessTravel>();

    public virtual ICollection<CommonPermission> CommonPermissions { get; set; } = new List<CommonPermission>();

    public virtual ICollection<CompOffAvail> CompOffAvails { get; set; } = new List<CompOffAvail>();

    public virtual ICollection<CompOffCredit> CompOffCredits { get; set; } = new List<CompOffCredit>();

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<LeaveRequisition> LeaveRequisitions { get; set; } = new List<LeaveRequisition>();

    public virtual ICollection<ManualPunchRequistion> ManualPunchRequistions { get; set; } = new List<ManualPunchRequistion>();

    public virtual ICollection<OnDutyRequisition> OnDutyRequisitions { get; set; } = new List<OnDutyRequisition>();

    public virtual ICollection<Probation> Probations { get; set; } = new List<Probation>();

    public virtual ICollection<Reimbursement> Reimbursements { get; set; } = new List<Reimbursement>();

    public virtual ICollection<ShiftChange> ShiftChanges { get; set; } = new List<ShiftChange>();

    public virtual ICollection<ShiftExtension> ShiftExtensions { get; set; } = new List<ShiftExtension>();

    public virtual StaffCreation Staff { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }

    public virtual ICollection<WeeklyOffHolidayWorking> WeeklyOffHolidayWorkings { get; set; } = new List<WeeklyOffHolidayWorking>();

    public virtual ICollection<WorkFromHome> WorkFromHomes { get; set; } = new List<WorkFromHome>();
}
