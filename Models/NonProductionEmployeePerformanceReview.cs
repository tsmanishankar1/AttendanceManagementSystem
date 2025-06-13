using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class NonProductionEmployeePerformanceReview
{
    public int Id { get; set; }

    public string EmpId { get; set; } = null!;

    public string EmpName { get; set; } = null!;

    public decimal TenureInYears { get; set; }

    public string ReportingManagers { get; set; } = null!;

    public string Division { get; set; } = null!;

    public string Department { get; set; } = null!;

    public decimal FinalAverageKraGrade { get; set; }

    public decimal AbsentDays { get; set; }

    public string? HrComments { get; set; }

    public int AppraisalId { get; set; }

    public bool? IsCompleted { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual AppraisalSelectionDropDown Appraisal { get; set; } = null!;

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
