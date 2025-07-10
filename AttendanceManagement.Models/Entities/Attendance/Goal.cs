using System;
using System.Collections.Generic;

namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class Goal
{
    public int Id { get; set; }

    public string Kra { get; set; } = null!;

    public decimal Weightage { get; set; }

    public string EvaluationPeriod { get; set; } = null!;

    public int AppraisalId { get; set; }

    public bool? IsCompleted { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int Year { get; set; }

    public string Quarter { get; set; } = null!;

    public int StaffId { get; set; }

    public virtual AppraisalSelectionDropDown Appraisal { get; set; } = null!;

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<KraSelfReview> KraSelfReviews { get; set; } = new List<KraSelfReview>();

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
