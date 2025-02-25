using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class HolidayWorking
{
    public int Id { get; set; }

    public string StaffId { get; set; } = null!;

    public string? ShiftId { get; set; }

    public DateTime? TxnDate { get; set; }

    public DateTime? ShiftInDate { get; set; }

    public DateTime? ShiftInTime { get; set; }

    public DateTime? ShiftOutDate { get; set; }

    public DateTime? ShiftOutTime { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }
}
