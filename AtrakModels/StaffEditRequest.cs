using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class StaffEditRequest
{
    public int Id { get; set; }

    public string RequestId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string? Staff { get; set; }

    public string? StaffOfficial { get; set; }

    public string? StaffPersonal { get; set; }

    public DateTime Createdon { get; set; }

    public string Createdby { get; set; } = null!;

    public string? AdditionalFieldValue { get; set; }
}
