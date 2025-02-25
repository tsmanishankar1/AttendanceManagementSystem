using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class DashBoardSwipe
{
    public string Id { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public DateTime TransactionTime { get; set; }

    public int TransactionTypeId { get; set; }

    public string? TransactionType { get; set; }

    public string? IpAddress { get; set; }

    public string? Lattitude { get; set; }

    public string? Longitude { get; set; }
}
