using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class EmployeePerformanceReview
{
    public int Id { get; set; }

    public string EmpId { get; set; } = null!;

    public string EmpName { get; set; } = null!;

    public decimal TenureInYears { get; set; }

    public string ReportingManagers { get; set; } = null!;

    public string Division { get; set; } = null!;

    public string Department { get; set; } = null!;

    public decimal ProductivityPercentage { get; set; }

    public decimal QualityPercentage { get; set; }

    public decimal PresentPercentage { get; set; }

    public decimal FinalPercentage { get; set; }

    public string Grade { get; set; } = null!;

    public decimal AbsentDays { get; set; }

    public int AppraisalId { get; set; }

    public bool? IsCompleted { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public string? HrComments { get; set; }

    public int Year { get; set; }

    public string Quarter { get; set; } = null!;

    public virtual ICollection<AgmApproval> AgmApprovals { get; set; } = new List<AgmApproval>();

    public virtual AppraisalSelectionDropDown Appraisal { get; set; } = null!;

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
