namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class MonthlyPerformance
{
    public int Id { get; set; }

    public string EmployeeCode { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

    public string Designation { get; set; } = null!;

    public decimal ProductivityScore { get; set; }

    public decimal QualityScore { get; set; }

    public decimal PresentScore { get; set; }

    public decimal TotalScore { get; set; }

    public decimal ProductivityPercentage { get; set; }

    public decimal QualityPercentage { get; set; }

    public decimal PresentPercentage { get; set; }

    public decimal FinalPercentage { get; set; }

    public string Grade { get; set; } = null!;

    public decimal TotalAbsents { get; set; }

    public string ReportingHead { get; set; } = null!;

    public decimal TenureYears { get; set; }

    public string HrComments { get; set; } = null!;

    public int PerformanceTypeId { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual PerformanceUploadType PerformanceType { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
