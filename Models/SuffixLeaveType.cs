using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class SuffixLeaveType
{
    public int Id { get; set; }

    public string SuffixLeaveTypeName { get; set; } = null!;

    public virtual ICollection<PrefixAndSuffix> PrefixAndSuffixes { get; set; } = new List<PrefixAndSuffix>();
}
