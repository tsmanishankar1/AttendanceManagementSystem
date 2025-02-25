using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class HolidayZoneTxn
{
    public int Id { get; set; }

    public int HolidayZoneId { get; set; }

    public int HolidayId { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual Holiday Holiday { get; set; } = null!;

    public virtual HolidayZone HolidayZone { get; set; } = null!;
}
