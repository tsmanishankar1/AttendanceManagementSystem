using System;
using System.Collections.Generic;

namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class SelectedNonProductionEmployee
{
    public int Id { get; set; }

    public string EmployeeId { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

    public string? TenureInYears { get; set; }

    public string ReportingManagers { get; set; } = null!;

    public string Division { get; set; } = null!;

    public string Department { get; set; } = null!;

    public bool? IsCompleted { get; set; }

    public int AppraisalId { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int Year { get; set; }

    public string Quarter { get; set; } = null!;

    public virtual AppraisalSelectionDropDown Appraisal { get; set; } = null!;

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
