using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Company
{
    public string Id { get; set; } = null!;

    public string? PeopleSoftCode { get; set; }

    public string Name { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public string? LegalName { get; set; }

    public string? Website { get; set; }

    public string? RegisterNo { get; set; }

    public string? Tngsno { get; set; }

    public string? Cstno { get; set; }

    public string? Tinno { get; set; }

    public string? ServiceTaxNo { get; set; }

    public string? Panno { get; set; }

    public string? Pfno { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();

    public virtual ICollection<RestrictedHoliday> RestrictedHolidays { get; set; } = new List<RestrictedHoliday>();

    public virtual ICollection<StaffOfficial> StaffOfficials { get; set; } = new List<StaffOfficial>();
}
