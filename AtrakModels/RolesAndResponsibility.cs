using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class RolesAndResponsibility
{
    public int Id { get; set; }

    public string StaffId { get; set; } = null!;

    public string? Roles { get; set; }

    public string? Responsibilities { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public string? Authorities { get; set; }

    public bool? IsActive { get; set; }
}
