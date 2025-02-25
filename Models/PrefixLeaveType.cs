using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class PrefixLeaveType
{
    public int Id { get; set; }

    public string PrefixLeaveTypeName { get; set; } = null!;

    public virtual ICollection<PrefixAndSuffix> PrefixAndSuffixes { get; set; } = new List<PrefixAndSuffix>();
}
