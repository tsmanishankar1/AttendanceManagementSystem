namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class HolidayZoneConfiguration
{
    public int Id { get; set; }

    public string HolidayZoneName { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int HolidayCalendarId { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual HolidayCalendarConfiguration HolidayCalendar { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
