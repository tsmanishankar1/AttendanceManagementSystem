using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ReportToBeSentTxn
{
    public int Id { get; set; }

    public string StaffId { get; set; } = null!;

    public int ReportId { get; set; }

    public DateTime? LastRunTime { get; set; }

    public DateTime? NextRunTime { get; set; }

    public int OffSet { get; set; }

    public string Duration { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public string? QueryString { get; set; }
}
