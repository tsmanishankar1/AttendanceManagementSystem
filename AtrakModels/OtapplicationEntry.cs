using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class OtapplicationEntry
{
    public int Id { get; set; }

    public string StaffId { get; set; } = null!;

    public string StaffName { get; set; } = null!;

    public DateTime FromDate { get; set; }

    public DateTime ToDate { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;
}
