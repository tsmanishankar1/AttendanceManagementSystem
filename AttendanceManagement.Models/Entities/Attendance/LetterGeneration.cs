namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class LetterGeneration
{
    public int Id { get; set; }

    public string LetterPath { get; set; } = null!;

    public byte[]? LetterContent { get; set; }

    public int StaffCreationId { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public bool IsActive { get; set; }

    public string? FileName { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<EmployeeAcceptance> EmployeeAcceptances { get; set; } = new List<EmployeeAcceptance>();

    public virtual StaffCreation StaffCreation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
