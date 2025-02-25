using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class RelationType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<StaffFamily> StaffFamilies { get; set; } = new List<StaffFamily>();
}
