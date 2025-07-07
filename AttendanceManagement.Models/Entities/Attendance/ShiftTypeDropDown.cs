namespace AttendanceManagement.Domain.Entities.Attendance;
public partial class ShiftTypeDropDown
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public bool IsActive { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<HolidayCalendarConfiguration> HolidayCalendarConfigurations { get; set; } = new List<HolidayCalendarConfiguration>();

    public virtual ICollection<Shift> Shifts { get; set; } = new List<Shift>();

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
