namespace AttendanceManagement.Input_Models
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
        public string EvaluationPeriod { get; set; } = null!;
        public int AppraisalId { get; set; }
    }

    public class KraResponse
    {
        public int Id { get; set; }
        public string Kra { get; set; } = null!;
        public decimal Weightage { get; set; }
        public string EvaluationPeriod { get; set; } = null!;
        public int CreatedBy { get; set; }
    }

    public class SelfEvaluationRequest
    {
        public IEnumerable<SelectedSelfEvaluation> SelectedRows { get; set; } = null!;
        public int CreatedBy { get; set; }
    }

    public class SelectedSelfEvaluation
    {
        public int GoalId { get; set; }
        public int SelfEvaluationScale { get; set; }
        public decimal SelfScore { get; set; }
        public string SelfEvaluationComments { get; set; } = null!;
        public IFormFile? AttachmentsSelf { get; set; }
        public int AppraisalId { get; set; }
    }

    public class SelfEvaluationResponse
    {
        public int Id { get; set; }
        public int GoalId { get; set; }
        public int SelfEvaluationScale { get; set; }
        public decimal SelfScore { get; set; }
        public string SelfEvaluationComments { get; set; } = null!;
        public string? AttachmentsSelf { get; set; }
        public string StaffName { get; set; } = null!;
        public int CreatedBy { get; set; }
    }

    public class ManagerEvaluationRequest
    {
        public IEnumerable<SelectedManagerEvaluation> SelectedRows { get; set; } = null!;
        public int CreatedBy { get; set; }
    }

    public class SelectedManagerEvaluation
    {
        public int KraSelfReviewId { get; set; }
        public int ManagerEvaluationScale { get; set; }
        public decimal ManagerScore { get; set; }
        public string ManagerEvaluationComments { get; set; } = null!;
        public IFormFile? AttachmentsManager { get; set; }
        public int AppraisalId { get; set; }
    }

    public class ManagerEvaluationResponse
    {
        public int Id { get; set; }
        public int KraSelfReviewId { get; set; }
        public int ManagerEvaluationScale { get; set; }
        public decimal ManagerScore { get; set; }
        public string ManagerEvaluationComments { get; set; } = null!;
        public string? AttachmentsManager { get; set; }
        public string ManagerName { get; set; } = null!;
        public bool? IsCompleted { get; set; }
        public int CreatedBy { get; set; }
    }
}
