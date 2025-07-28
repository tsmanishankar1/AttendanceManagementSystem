using Microsoft.AspNetCore.Http;

namespace AttendanceManagement.Application.Dtos.Attendance
{
    public class KraDto
    {
        public IEnumerable<SelectedKra> SelectedRows { get; set; } = null!;
        public int CreatedBy { get; set; }
    }

    public class SelectedKra
    {
        public string Kra { get; set; } = null!;
        public decimal Weightage { get; set; }
        public string? Quarter { get; set; }
        public int? Month { get; set; }
        public int Year { get; set; }
        public List<int> StaffId { get; set; } = new List<int>();
        public int AppraisalId { get; set; }
    }

    public class KraResponse
    {
        public int Id { get; set; }
        public string Kra { get; set; } = null!;
        public decimal Weightage { get; set; }
        public int Year { get; set; }
        public string? Quarter { get; set; }
        public int? Month { get; set; }
        public int CreatedBy { get; set; }
        public List<int> AssignedStaffIds { get; set; } = new();
        public List<string>? AssignedStaffNames { get; set; }
    }

    public class SelfEvaluationRequest
    {
        public int GoalId { get; set; }
        public int SelfEvaluationScale { get; set; }
        public decimal SelfScore { get; set; }
        public string SelfEvaluationComments { get; set; } = null!;
        public IFormFile? AttachmentsSelf { get; set; }
        public int AppraisalId { get; set; }
        public int CreatedBy { get; set; }
    }

    public class SelectedSelfEvaluation
    {
        public int GoalId { get; set; }
        public int SelfEvaluationScale { get; set; }
        public decimal SelfScore { get; set; }
        public string SelfEvaluationComments { get; set; } = null!;
        public IFormFile? AttachmentsSelf { get; set; }
        public int Year { get; set; }
        public string? Quarter { get; set; }
        public int? Month { get; set; }
        public int AppraisalId { get; set; }
    }

    public class SelfEvaluationResponse
    {
        public int Id { get; set; }
        public int GoalId { get; set; }
        public decimal SelfEvaluationScale { get; set; }
        public decimal SelfScore { get; set; }
        public string SelfEvaluationComments { get; set; } = null!;
        public string? AttachmentsSelf { get; set; }
        public string StaffId { get; set; } = null!;
        public string StaffName { get; set; } = null!;
        public int Year { get; set; }
        public string? Quarter { get; set; }
        public int? Month { get; set; }
        public bool? IsSelfEvaluation { get; set; }
        public int CreatedBy { get; set; }
    }

    public class ManagerEvaluationRequest
    {
        public int KraSelfReviewId { get; set; }
        public decimal ManagerEvaluationScale { get; set; }
        public decimal ManagerScore { get; set; }
        public string ManagerEvaluationComments { get; set; } = null!;
        public IFormFile? AttachmentsManager { get; set; }
        public int AppraisalId { get; set; }
        public int CreatedBy { get; set; }
    }

    public class SelectedManagerEvaluation
    {
        public int KraSelfReviewId { get; set; }
        public decimal ManagerEvaluationScale { get; set; }
        public decimal ManagerScore { get; set; }
        public string ManagerEvaluationComments { get; set; } = null!;
        public IFormFile? AttachmentsManager { get; set; }
        public int Year { get; set; }
        public string? Quarter { get; set; }
        public int? Month { get; set; }
        public int AppraisalId { get; set; }
    }

    public class ManagerEvaluationResponse
    {
        public int Id { get; set; }
        public int KraSelfReviewId { get; set; }
        public decimal ManagerEvaluationScale { get; set; }
        public decimal ManagerScore { get; set; }
        public string ManagerEvaluationComments { get; set; } = null!;
        public string? AttachmentsManager { get; set; }
        public string ManagerId { get; set; } = null!;
        public string ManagerName { get; set; } = null!;
        public int Year { get; set; }
        public string? Quarter { get; set; }
        public int? Month { get; set; }
        public bool? IsCompleted { get; set; }
        public bool? IsManagerEvaluation { get; set; }
        public int CreatedBy { get; set; }
    }
}
