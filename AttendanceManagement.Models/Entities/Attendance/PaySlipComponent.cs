namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class PaySlipComponent
{
    public int Id { get; set; }

    public string? ComponentType { get; set; }

    public string? ComponentName { get; set; }

    public int? PaySlipId { get; set; }

    public int? StaffId { get; set; }

    public string? Amount { get; set; }

    public bool IsTaxable { get; set; }

    public string? Remarks { get; set; }

    public bool? IsActive { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation? CreatedByNavigation { get; set; }

    public virtual PaySlip? PaySlip { get; set; }

    public virtual StaffCreation? Staff { get; set; }

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
