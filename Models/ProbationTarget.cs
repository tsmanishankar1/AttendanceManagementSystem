using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class ProbationTarget
{
    public int Id { get; set; }

    public string EmpId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int DivisionId { get; set; }

    public decimal? Jan { get; set; }

    public decimal? Feb { get; set; }

    public decimal? Mar { get; set; }

    public decimal? Apr { get; set; }

    public decimal? May { get; set; }

    public decimal? Jun { get; set; }

    public decimal? Jul { get; set; }

    public decimal? Aug { get; set; }

    public decimal? Sep { get; set; }

    public decimal? Oct { get; set; }

    public decimal? Nov { get; set; }

    public decimal? Dec { get; set; }

    public int ProductivityYear { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual DivisionMaster Division { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
