namespace AttendanceManagement.Domain.Entities.Attendance;
public partial class StaffVaccination
{
    public int Id { get; set; }

    public int StaffId { get; set; }

    public DateOnly? VaccinatedDate { get; set; }

    public int VaccinationNumber { get; set; }

    public bool IsExempted { get; set; }

    public string? Comments { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public bool IsActive { get; set; }

    public DateTime? SecondVaccinationDate { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation Staff { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
