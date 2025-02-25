using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class EmailSetting
{
    public string OutgoingServer { get; set; } = null!;

    public int OutgoingPort { get; set; }

    public string UserName { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool EnableSsl { get; set; }

    public string SenderEmail { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;
}
