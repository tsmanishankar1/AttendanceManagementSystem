using System;
using System.Collections.Generic;

namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class CompanyMaster
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public string LegalName { get; set; } = null!;

    public string? Address { get; set; }

    public string? Website { get; set; }

    public string? RegisterNumber { get; set; }

    public string? Tngsnumber { get; set; }

    public string? Cstnumber { get; set; }

    public string? Tinnumber { get; set; }

    public string? ServiceTaxNo { get; set; }

    public string? Pannumber { get; set; }

    public string? Pfnumber { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual ICollection<BranchMaster> BranchMasters { get; set; } = new List<BranchMaster>();

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<ManualAttendanceProcessing> ManualAttendanceProcessings { get; set; } = new List<ManualAttendanceProcessing>();

    public virtual ICollection<StaffCreation> StaffCreations { get; set; } = new List<StaffCreation>();

    public virtual ICollection<StatutoryReport> StatutoryReports { get; set; } = new List<StatutoryReport>();

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
