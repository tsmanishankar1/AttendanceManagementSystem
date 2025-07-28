namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class TaxSlab
{
    public int Id { get; set; }

    public decimal MinSalary { get; set; }

    public decimal? MaxSalary { get; set; }

    public decimal TaxPercentage { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
