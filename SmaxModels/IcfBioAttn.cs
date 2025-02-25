using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class IcfBioAttn
{
    public long IbaId { get; set; }

    public string? IbaReader { get; set; }

    public string? IbaEmpno { get; set; }

    public string? IbaCode { get; set; }

    public string? IbaRemarks { get; set; }

    public DateTime? IbaIntime { get; set; }

    public DateTime? IbaOuttime { get; set; }

    public string? IbaShiftcode { get; set; }

    public DateTime? IbaCreatedOn { get; set; }

    public DateOnly? IbaDate { get; set; }

    public string? IbaBillunit { get; set; }

    public string? IbaEmpsec { get; set; }
}
