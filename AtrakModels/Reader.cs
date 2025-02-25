using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Reader
{
    public string Id { get; set; } = null!;

    public string? DoorId { get; set; }

    public string Description { get; set; } = null!;

    public byte Inout { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime ModifiedOn { get; set; }

    public string MofifiedBy { get; set; } = null!;

    public virtual Door? Door { get; set; }
}
