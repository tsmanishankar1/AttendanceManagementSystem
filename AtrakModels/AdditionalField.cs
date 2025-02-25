using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class AdditionalField
{
    public int Id { get; set; }

    public string ScreenName { get; set; } = null!;

    public string ColumnName { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string Access { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public string Createdby { get; set; } = null!;

    public DateTime? ModifiedOn { get; set; }

    public string? Modifiedby { get; set; }

    public virtual ICollection<AdditionalFieldValue> AdditionalFieldValues { get; set; } = new List<AdditionalFieldValue>();
}
