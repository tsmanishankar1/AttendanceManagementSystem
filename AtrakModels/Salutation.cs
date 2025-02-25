using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Salutation
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsActive { get; set; }

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
