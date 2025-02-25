using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class CompensatoryWorking
{
    public int Id { get; set; }

    public DateTime LeaveDate { get; set; }

    public DateTime CompensatoryWorkingDate { get; set; }

    public string? Reason { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }
}
