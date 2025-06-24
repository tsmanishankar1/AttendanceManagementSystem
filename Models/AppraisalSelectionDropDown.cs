using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class AppraisalSelectionDropDown
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<EmployeePerformanceReview> EmployeePerformanceReviews { get; set; } = new List<EmployeePerformanceReview>();

    public virtual ICollection<Goal> Goals { get; set; } = new List<Goal>();

    public virtual ICollection<KraManagerReview> KraManagerReviews { get; set; } = new List<KraManagerReview>();

    public virtual ICollection<KraSelfReview> KraSelfReviews { get; set; } = new List<KraSelfReview>();

    public virtual ICollection<NonProductionEmployeePerformanceReview> NonProductionEmployeePerformanceReviews { get; set; } = new List<NonProductionEmployeePerformanceReview>();

    public virtual ICollection<SelectedEmployeesForAppraisal> SelectedEmployeesForAppraisals { get; set; } = new List<SelectedEmployeesForAppraisal>();

    public virtual ICollection<SelectedNonProductionEmployee> SelectedNonProductionEmployees { get; set; } = new List<SelectedNonProductionEmployee>();

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
