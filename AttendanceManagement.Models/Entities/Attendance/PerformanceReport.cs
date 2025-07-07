namespace AttendanceManagement.Domain.Entities.Attendance;
public partial class PerformanceReport
{
    public int Id { get; set; }

    public string EmpId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int EmpDivisionId { get; set; }

    public decimal ProdPercentage { get; set; }

    public decimal ProdScore { get; set; }

    public string ProdGrade { get; set; } = null!;

    public decimal QualityPercentage { get; set; }

    public decimal QualityScore { get; set; }

    public string QualityGrade { get; set; } = null!;

    public int? NoOfAbsents { get; set; }

    public decimal AttendancePercentage { get; set; }

    public decimal AttendanceScore { get; set; }

    public string AttendanceGrade { get; set; } = null!;

    public decimal TotalScore { get; set; }

    public int WorkingMonths { get; set; }

    public decimal Score { get; set; }

    public decimal FinalPercentage { get; set; }

    public string FinalGrade { get; set; } = null!;

    public string Comments { get; set; } = null!;

    public int PerformanceTypeId { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual DivisionMaster EmpDivision { get; set; } = null!;

    public virtual PerformanceUploadType PerformanceType { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
