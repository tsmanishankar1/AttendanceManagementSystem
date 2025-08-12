using AttendanceManagement.Domain.Entities.Attendance;

namespace AttendanceManagement.Application.Dtos.Attendance
{
    public class ProbationRequest
    {
        public int StaffId { get; set; }
        public DateOnly ProbationStartDate { get; set; }
        public DateOnly ProbationEndDate { get; set; }
        public int CreatedBy { get; set; }
    }

    public class AssignManagerRequest
    {
        public int ProbationId { get; set; }
        public int ManagerId { get; set; }
        public int CreatedBy { get; set; }
    }

    public class ProbationResponse
    {
        public int ProbationId { get; set; }
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string StaffName { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
        public DateOnly ProbationStartDate { get; set; }
        public DateOnly ProbationEndDate { get; set; }
        public bool? IsAssigned { get; set; }
        public int? ManagerId { get; set; }
        public bool? IsApproved { get; set; }
        public string? FeedBackText { get; set; }
        public int CreatedBy { get; set; }
        public ProbationReport? ProbationReport { get; set; }
    }

    public class ProbationReportResponse
    {
        public string EmpId { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Department { get; set; } = null!;
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
        public int? NumberOfMonths { get; set; }
        public int ProductivityYear { get; set; }
    }

    public class UpdateProbation
    {
        public int ProbationId { get; set; }
        public int StaffId { get; set; }
        public DateOnly ProbationStartDate { get; set; }
        public DateOnly ProbationEndDate { get; set; }
        public bool? IsCompleted { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class GeneratedLetterResponse
    {
        public int Id { get; set; }
        public string LetterPath { get; set; } = null!;
        public byte[]? LetterContent { get; set; }
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string? FileName { get; set; }
    }
}