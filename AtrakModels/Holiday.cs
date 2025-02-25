using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Holiday
{
    public int Id { get; set; }

    public string? LeaveTypeId { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public virtual ICollection<HolidayFixedDay> HolidayFixedDays { get; set; } = new List<HolidayFixedDay>();

    public virtual ICollection<HolidayGroupTxn> HolidayGroupTxns { get; set; } = new List<HolidayGroupTxn>();

    public virtual ICollection<HolidayZoneTxn> HolidayZoneTxns { get; set; } = new List<HolidayZoneTxn>();

    public virtual LeaveType? LeaveType { get; set; }
}
