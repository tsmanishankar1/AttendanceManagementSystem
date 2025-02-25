using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class SecurityGroup
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<StaffOfficial> StaffOfficials { get; set; } = new List<StaffOfficial>();
}
