using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class AdditionalFieldValue
{
    public int Id { get; set; }

    public string Staffid { get; set; } = null!;

    public int AddfId { get; set; }

    public string? ActualValue { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? Modifiedby { get; set; }

    public virtual AdditionalField Addf { get; set; } = null!;
}
