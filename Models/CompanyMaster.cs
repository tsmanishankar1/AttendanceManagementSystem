using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class CompanyMaster
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public string LegalName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Website { get; set; } = null!;

    public string RegisterNumber { get; set; } = null!;

    public string Tngsnumber { get; set; } = null!;

    public string Cstnumber { get; set; } = null!;

    public string Tinnumber { get; set; } = null!;

    public string ServiceTaxNo { get; set; } = null!;

    public string Pannumber { get; set; } = null!;

    public string Pfnumber { get; set; } = null!;

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual ICollection<BranchMaster> BranchMasters { get; set; } = new List<BranchMaster>();

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<DailyReport> DailyReports { get; set; } = new List<DailyReport>();

    public virtual ICollection<ManualAttendanceProcessing> ManualAttendanceProcessings { get; set; } = new List<ManualAttendanceProcessing>();

    public virtual ICollection<StaffCreation> StaffCreations { get; set; } = new List<StaffCreation>();

    public virtual ICollection<StatutoryReport> StatutoryReports { get; set; } = new List<StatutoryReport>();

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
