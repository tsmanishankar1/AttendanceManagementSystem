using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ReportToBeSent
{
    public string StaffId { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }
}
