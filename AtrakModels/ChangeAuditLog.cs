using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ChangeAuditLog
{
    public int Id { get; set; }

    public string UserId { get; set; } = null!;

    public string ChangeLog { get; set; } = null!;

    public string ActionType { get; set; } = null!;

    public string TableName { get; set; } = null!;

    public string PrimaryKeyValue { get; set; } = null!;

    public DateTime CreatedOn { get; set; }
}
