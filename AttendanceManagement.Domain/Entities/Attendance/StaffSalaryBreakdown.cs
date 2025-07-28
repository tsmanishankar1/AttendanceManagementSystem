namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class StaffSalaryBreakdown
{
    public int Id { get; set; }

    public int StaffSalaryId { get; set; }

    public int ComponentId { get; set; }

    public decimal Amount { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual SalaryComponent Component { get; set; } = null!;

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffSalary StaffSalary { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
