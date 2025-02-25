using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class MenuType
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsReportMenu { get; set; }
}
