using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class AtrakUserDetail
{
    public int Id { get; set; }

    public string? StaffId { get; set; }

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }
}
