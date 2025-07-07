namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class AgmApproval
{
    public int Id { get; set; }

    public int EmployeePerformanceReviewId { get; set; }

    public int AgmId { get; set; }

    public bool? IsAgmApproved { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int Year { get; set; }

    public string Quarter { get; set; } = null!;

    public virtual StaffCreation Agm { get; set; } = null!;

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual EmployeePerformanceReview EmployeePerformanceReview { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
