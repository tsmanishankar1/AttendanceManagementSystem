using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class PolicyDocUpload
{
    public int Id { get; set; }

    public string PolicyName { get; set; } = null!;

    public string FileType { get; set; } = null!;

    public byte[] FileExtension { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string? Createdby { get; set; }

    public bool IsCancelled { get; set; }

    public DateTime CancelledOn { get; set; }

    public string? CancelledBy { get; set; }
}
