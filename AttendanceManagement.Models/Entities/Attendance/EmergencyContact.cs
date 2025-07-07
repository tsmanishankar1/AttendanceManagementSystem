namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class EmergencyContact
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Relationship { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public long? LandlineNo { get; set; }

    public long MobileNo { get; set; }

    public string? EmailId { get; set; }

    public long? OfficeExtensionPhoneNumber { get; set; }

    public string Address { get; set; } = null!;

    public int StaffCreationId { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation StaffCreation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
