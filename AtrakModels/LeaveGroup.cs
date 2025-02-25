using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class LeaveGroup
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<LeaveGroupTxn> LeaveGroupTxns { get; set; } = new List<LeaveGroupTxn>();

    public virtual ICollection<StaffOfficial> StaffOfficials { get; set; } = new List<StaffOfficial>();
}
