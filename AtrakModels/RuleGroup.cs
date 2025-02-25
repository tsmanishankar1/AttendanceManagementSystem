using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class RuleGroup
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool Isactive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual ICollection<RuleGroupTxn> RuleGroupTxns { get; set; } = new List<RuleGroupTxn>();

    public virtual ICollection<StaffOfficial> StaffOfficials { get; set; } = new List<StaffOfficial>();
}
