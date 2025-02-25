using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class EventType
{
    public int Id { get; set; }

    public string EventName { get; set; } = null!;

    public bool IsActive { get; set; }
}
