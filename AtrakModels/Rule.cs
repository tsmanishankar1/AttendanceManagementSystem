using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Rule
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Datatype { get; set; } = null!;

    public string Ruletype { get; set; } = null!;

    public bool Isactive { get; set; }

    public bool AllowUserToEdit { get; set; }

    public virtual ICollection<RuleGroupTxn> RuleGroupTxns { get; set; } = new List<RuleGroupTxn>();
}
