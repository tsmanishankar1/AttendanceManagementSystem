using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class FkEntry
{
    public string? PktableQualifier { get; set; }

    public string? PktableOwner { get; set; }

    public string? PktableName { get; set; }

    public string? PkcolumnName { get; set; }

    public string? FktableQualifier { get; set; }

    public string? FktableOwner { get; set; }

    public string? FktableName { get; set; }

    public string? FkcolumnName { get; set; }

    public int? KeySeq { get; set; }

    public int? UpdateRule { get; set; }

    public int? DeleteRule { get; set; }

    public string? FkName { get; set; }

    public string? PkName { get; set; }

    public int? Deferrability { get; set; }

    public byte? ProcessId { get; set; }
}
