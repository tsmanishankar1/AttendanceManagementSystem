using System;
using System.Collections.Generic;

namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class ProbationReport
{
    public int Id { get; set; }

    public string EmpId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int DepartmentId { get; set; }

    public decimal ProdScore { get; set; }

    public decimal ProdPercentage { get; set; }

    public string ProdGrade { get; set; } = null!;

    public decimal QualityScore { get; set; }

    public decimal QualityPercentage { get; set; }

    public decimal NoOfAbsent { get; set; }

    public decimal AttendanceScore { get; set; }

    public decimal AttendancePercentage { get; set; }

    public string AttendanceGrade { get; set; } = null!;

    public decimal FinalTotal { get; set; }

    public decimal TotalScore { get; set; }

    public decimal FinalScorePercentage { get; set; }

    public string FinalGrade { get; set; } = null!;

    public decimal? ProductionAchievedPercentageJan { get; set; }

    public decimal? ProductionAchievedPercentageFeb { get; set; }

    public decimal? ProductionAchievedPercentageMar { get; set; }

    public decimal? ProductionAchievedPercentageApr { get; set; }

    public decimal? ProductionAchievedPercentageMay { get; set; }

    public decimal? ProductionAchievedPercentageJun { get; set; }

    public decimal? ProductionAchievedPercentageJul { get; set; }

    public decimal? ProductionAchievedPercentageAug { get; set; }

    public decimal? ProductionAchievedPercentageSep { get; set; }

    public decimal? ProductionAchievedPercentageOct { get; set; }

    public decimal? ProductionAchievedPercentageNov { get; set; }

    public decimal? ProductionAchievedPercentageDec { get; set; }

    public int ProductivityYear { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int? Month { get; set; }

    public int? NumberOfMonths { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
