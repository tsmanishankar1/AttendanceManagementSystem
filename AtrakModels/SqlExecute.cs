using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class SqlExecute
{
    public long Id { get; set; }

    public string Description { get; set; } = null!;

    public string SqlQuery { get; set; } = null!;

    public DateTime ExecuteDateTime { get; set; }

    public string ParentId { get; set; } = null!;

    public bool IsCancelled { get; set; }

    public DateTime CancelledOn { get; set; }

    public string CancelledBy { get; set; } = null!;

    public bool IsExecuted { get; set; }

    public DateTime ExecutedOn { get; set; }

    public string ExecutedBy { get; set; } = null!;

    public bool ExecuteEveryTime { get; set; }

    public string QueryType { get; set; } = null!;

    public int SeqId { get; set; }

    public long GroupId { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;
}
