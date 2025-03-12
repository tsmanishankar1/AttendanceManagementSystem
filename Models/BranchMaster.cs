using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class BranchMaster
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public string City { get; set; } = null!;

    public string District { get; set; } = null!;

    public string State { get; set; } = null!;

    public string Country { get; set; } = null!;

    public int PostalCode { get; set; }

    public long PhoneNumber { get; set; }

    public string? Fax { get; set; }

    public string? Email { get; set; }

    public bool IsHeadOffice { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int CompanyMasterId { get; set; }

    public string Address { get; set; } = null!;

    public virtual CompanyMaster CompanyMaster { get; set; } = null!;

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<ManualAttendanceProcessing> ManualAttendanceProcessings { get; set; } = new List<ManualAttendanceProcessing>();

    public virtual ICollection<StaffCreation> StaffCreations { get; set; } = new List<StaffCreation>();

    public virtual ICollection<StatutoryReport> StatutoryReports { get; set; } = new List<StatutoryReport>();

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
