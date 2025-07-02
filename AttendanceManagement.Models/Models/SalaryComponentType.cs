using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class SalaryComponentType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<SalaryComponent> SalaryComponents { get; set; } = new List<SalaryComponent>();
}
