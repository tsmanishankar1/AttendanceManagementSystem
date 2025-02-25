using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class EmailSendLog
{
    public long Id { get; set; }

    public string? From { get; set; }

    public string To { get; set; } = null!;

    public string? Cc { get; set; }

    public string? Bcc { get; set; }

    public string EmailSubject { get; set; } = null!;

    public string EmailBody { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public bool IsSent { get; set; }

    public DateTime SentOn { get; set; }

    public bool IsError { get; set; }

    public string? ErrorDescription { get; set; }

    public int SentCounter { get; set; }

    public byte[]? AttachmentByte { get; set; }

    public string? FileType { get; set; }

    public string? FilePathName { get; set; }

    public bool? IncludesAttachment { get; set; }
}
