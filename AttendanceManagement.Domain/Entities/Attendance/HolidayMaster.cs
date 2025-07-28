using System;
using System.Collections.Generic;

namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class HolidayMaster
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int HolidayTypeId { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<HolidayCalendarTransaction> HolidayCalendarTransactions { get; set; } = new List<HolidayCalendarTransaction>();

    public virtual HolidayType HolidayType { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
