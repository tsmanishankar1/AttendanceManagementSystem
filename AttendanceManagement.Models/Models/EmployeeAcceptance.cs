using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class EmployeeAcceptance
{
    public int Id { get; set; }

    public string EmpId { get; set; } = null!;

    public string EmpName { get; set; } = null!;

    public string Division { get; set; } = null!;

    public string Department { get; set; } = null!;

    public bool IsAccepted { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int FileId { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual LetterGeneration File { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
