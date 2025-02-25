using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Branch
{
    public string Id { get; set; } = null!;

    public string? CompanyId { get; set; }

    public string? PeopleSoftCode { get; set; }

    public string Name { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public string? District { get; set; }

    public string State { get; set; } = null!;

    public string Country { get; set; } = null!;

    public int PostalCode { get; set; }

    public string Phone { get; set; } = null!;

    public string? Fax { get; set; }

    public string? Email { get; set; }

    public bool IsHeadOffice { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual Company? Company { get; set; }

    public virtual ICollection<StaffOfficial> StaffOfficials { get; set; } = new List<StaffOfficial>();
}
