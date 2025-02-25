using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class RuleGroupTxn
{
    public int Id { get; set; }

    public int Rulegroupid { get; set; }

    public int Ruleid { get; set; }

    public string Value { get; set; } = null!;

    public string Defaultvalue { get; set; } = null!;

    public bool Isactive { get; set; }

    public string? LocationId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CategoryId { get; set; }

    public virtual Rule Rule { get; set; } = null!;

    public virtual RuleGroup Rulegroup { get; set; } = null!;
}
