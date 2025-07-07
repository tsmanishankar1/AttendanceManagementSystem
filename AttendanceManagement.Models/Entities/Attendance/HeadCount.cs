namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class HeadCount
{
    public int Id { get; set; }

    public int DepartmentId { get; set; }

    public int CategoryId { get; set; }

    public int ShiftId { get; set; }

    public int HeadCount1 { get; set; }

    public int PresentCount { get; set; }

    public int? AbsentCount { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual CategoryMaster Category { get; set; } = null!;

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual DepartmentMaster Department { get; set; } = null!;

    public virtual Shift Shift { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
