using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class MaritalStatus
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<StaffPersonal> StaffPersonals { get; set; } = new List<StaffPersonal>();
}
