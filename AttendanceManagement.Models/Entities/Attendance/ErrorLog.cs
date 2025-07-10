using System;
using System.Collections.Generic;

namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class ErrorLog
{
    public int Id { get; set; }

    public string Module { get; set; } = null!;

    public string HttpMethod { get; set; } = null!;

    public string ApiEndpoint { get; set; } = null!;

    public string ErrorMessage { get; set; } = null!;

    public string? StackTrace { get; set; }

    public string? InnerException { get; set; }

    public int CreatedBy { get; set; }

    public string? Payload { get; set; }

    public DateTime CreatedUtc { get; set; }
}
