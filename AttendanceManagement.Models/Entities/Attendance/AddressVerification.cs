namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class AddressVerification
{
    public int Id { get; set; }

    public int StaffCreationId { get; set; }

    public string AddressLine1 { get; set; } = null!;

    public string? AddressLine2 { get; set; }

    public string City { get; set; } = null!;

    public string State { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string PostalCode { get; set; } = null!;

    public bool? IsVerified { get; set; }

    public DateTime? VerificationDate { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime UpdatedUtc { get; set; }

    public int CreatedBy { get; set; }

    public int UpdatedBy { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation StaffCreation { get; set; } = null!;

    public virtual StaffCreation UpdatedByNavigation { get; set; } = null!;
}
