using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class UserManagement
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int StaffCreationId { get; set; }

    public virtual ICollection<AttendanceStatus> AttendanceStatuses { get; set; } = new List<AttendanceStatus>();

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<DailyReport> DailyReports { get; set; } = new List<DailyReport>();

    public virtual ICollection<LeaveAvailability> LeaveAvailabilities { get; set; } = new List<LeaveAvailability>();

    public virtual ICollection<ManualAttendanceProcessing> ManualAttendanceProcessings { get; set; } = new List<ManualAttendanceProcessing>();

    public virtual ICollection<MyApplication> MyApplications { get; set; } = new List<MyApplication>();

    public virtual ICollection<PasswordHistory> PasswordHistoryCreatedByNavigations { get; set; } = new List<PasswordHistory>();

    public virtual ICollection<PasswordHistory> PasswordHistoryUpdatedByNavigations { get; set; } = new List<PasswordHistory>();

    public virtual ICollection<PunchRegularizationApproval> PunchRegularizationApprovals { get; set; } = new List<PunchRegularizationApproval>();

    public virtual StaffCreation StaffCreation { get; set; } = null!;

    public virtual ICollection<StatutoryReport> StatutoryReports { get; set; } = new List<StatutoryReport>();

    public virtual ICollection<TeamApplication> TeamApplications { get; set; } = new List<TeamApplication>();

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
