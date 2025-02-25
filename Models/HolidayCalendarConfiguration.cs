using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class HolidayCalendarConfiguration
{
    public int Id { get; set; }

    public string GroupName { get; set; } = null!;

    public int CalendarYear { get; set; }

    public bool Currents { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<HolidayCalendarTransaction> HolidayCalendarTransactions { get; set; } = new List<HolidayCalendarTransaction>();

    public virtual ICollection<HolidayZoneConfiguration> HolidayZoneConfigurations { get; set; } = new List<HolidayZoneConfiguration>();

    public virtual ICollection<StaffCreation> StaffCreations { get; set; } = new List<StaffCreation>();

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
