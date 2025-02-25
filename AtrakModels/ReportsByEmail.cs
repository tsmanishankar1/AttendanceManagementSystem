using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ReportsByEmail
{
    public int Id { get; set; }

    public string ReportDescription { get; set; } = null!;

    public string ReportSubject { get; set; } = null!;

    public string ReportPara1 { get; set; } = null!;

    public string ReportPara2 { get; set; } = null!;

    public string ReportPara3 { get; set; } = null!;

    public string FunctionName { get; set; } = null!;

    public string Fields { get; set; } = null!;

    public string ParameterList { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }
}
