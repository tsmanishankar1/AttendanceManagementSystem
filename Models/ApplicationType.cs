using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class ApplicationType
{
    public int Id { get; set; }

    public string ApplicationTypeName { get; set; } = null!;

    public virtual ICollection<ApprovalNotification> ApprovalNotifications { get; set; } = new List<ApprovalNotification>();

    public virtual ICollection<BusinessTravel> BusinessTravels { get; set; } = new List<BusinessTravel>();

    public virtual ICollection<CommonPermission> CommonPermissions { get; set; } = new List<CommonPermission>();

    public virtual ICollection<CompOffAvail> CompOffAvails { get; set; } = new List<CompOffAvail>();

    public virtual ICollection<CompOffCredit> CompOffCredits { get; set; } = new List<CompOffCredit>();

    public virtual ICollection<LeaveRequisition> LeaveRequisitions { get; set; } = new List<LeaveRequisition>();

    public virtual ICollection<ManualPunchRequistion> ManualPunchRequistions { get; set; } = new List<ManualPunchRequistion>();

    public virtual ICollection<OnBehalfApplicationApproval> OnBehalfApplicationApprovals { get; set; } = new List<OnBehalfApplicationApproval>();

    public virtual ICollection<OnDutyRequisition> OnDutyRequisitions { get; set; } = new List<OnDutyRequisition>();

    public virtual ICollection<PermissionRequistion> PermissionRequistions { get; set; } = new List<PermissionRequistion>();

    public virtual ICollection<ShiftChange> ShiftChanges { get; set; } = new List<ShiftChange>();

    public virtual ICollection<ShiftExtension> ShiftExtensions { get; set; } = new List<ShiftExtension>();

    public virtual ICollection<WeeklyOffHolidayWorking> WeeklyOffHolidayWorkings { get; set; } = new List<WeeklyOffHolidayWorking>();

    public virtual ICollection<WorkFromHome> WorkFromHomes { get; set; } = new List<WorkFromHome>();
}
