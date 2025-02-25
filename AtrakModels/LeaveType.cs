using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class LeaveType
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public bool IsAccountable { get; set; }

    public bool IsEncashable { get; set; }

    public bool IsPaidLeave { get; set; }

    public bool IsCommon { get; set; }

    public bool IsPermission { get; set; }

    public bool CarryForward { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual ICollection<EmployeeLeaveAccount> EmployeeLeaveAccounts { get; set; } = new List<EmployeeLeaveAccount>();

    public virtual ICollection<Holiday> Holidays { get; set; } = new List<Holiday>();

    public virtual ICollection<LeaveApplication> LeaveApplications { get; set; } = new List<LeaveApplication>();

    public virtual ICollection<LeaveGroupTxn> LeaveGroupTxns { get; set; } = new List<LeaveGroupTxn>();

    public virtual ICollection<PrefixSuffixSetting> PrefixSuffixSettingLeaveTypes { get; set; } = new List<PrefixSuffixSetting>();

    public virtual ICollection<PrefixSuffixSetting> PrefixSuffixSettingPrefixLeaveTypes { get; set; } = new List<PrefixSuffixSetting>();

    public virtual ICollection<PrefixSuffixSetting> PrefixSuffixSettingSuffixLeaveTypes { get; set; } = new List<PrefixSuffixSetting>();

    public virtual ICollection<RestrictedHoliday> RestrictedHolidays { get; set; } = new List<RestrictedHoliday>();
}
