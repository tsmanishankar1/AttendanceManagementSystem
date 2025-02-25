using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Door
{
    public string Id { get; set; } = null!;

    public string Description { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime ModifiedOn { get; set; }

    public string MofifiedBy { get; set; } = null!;

    public virtual ICollection<Reader> Readers { get; set; } = new List<Reader>();
}
