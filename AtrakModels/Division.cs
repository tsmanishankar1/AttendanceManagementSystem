using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Division
{
    public string Id { get; set; } = null!;

    public string? PeopleSoftCode { get; set; }

    public string Name { get; set; } = null!;

    public string? ShortName { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual ICollection<StaffOfficial> StaffOfficials { get; set; } = new List<StaffOfficial>();
}
