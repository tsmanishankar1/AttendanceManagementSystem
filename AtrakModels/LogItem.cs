using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class LogItem
{
    public int LogId { get; set; }

    public string? LogType { get; set; }

    public string? UserId { get; set; }

    public string? Message { get; set; }

    public DateTime? CreatedOn { get; set; }
}
