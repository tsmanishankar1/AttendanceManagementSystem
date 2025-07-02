using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class AuditLog
{
    public int Id { get; set; }

    public string Module { get; set; } = null!;

    public string HttpMethod { get; set; } = null!;

    public string ApiEndpoint { get; set; } = null!;

    public string SuccessMessage { get; set; } = null!;

    public int CreatedBy { get; set; }

    public string? Payload { get; set; }

    public DateTime CreatedUtc { get; set; }
}
