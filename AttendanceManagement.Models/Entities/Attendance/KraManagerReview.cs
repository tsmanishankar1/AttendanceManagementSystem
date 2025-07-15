using System;
using System.Collections.Generic;

namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class KraManagerReview
{
    public int Id { get; set; }

    public int KraSelfReviewId { get; set; }

    public decimal ManagerEvaluationScale { get; set; }

    public decimal ManagerScore { get; set; }

    public string ManagerEvaluationComments { get; set; } = null!;

    public string? AttachmentsManager { get; set; }

    public bool? IsCompleted { get; set; }

    public int AppraisalId { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public bool? IsManagerEvaluation { get; set; }

    public virtual AppraisalSelectionDropDown Appraisal { get; set; } = null!;

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual KraSelfReview KraSelfReview { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
