namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class EmailLog
{
    public int Id { get; set; }

    public string From { get; set; } = null!;

    public string To { get; set; } = null!;

    public string? Cc { get; set; }

    public string? Bcc { get; set; }

    public string EmailSubject { get; set; } = null!;

    public string EmailBody { get; set; } = null!;

    public bool IsSent { get; set; }

    public bool IsError { get; set; }

    public string? ErrorDescription { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public string? ErrorStackTrace { get; set; }

    public string? ErrorInnerException { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;
}
