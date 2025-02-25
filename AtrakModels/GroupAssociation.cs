using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class GroupAssociation
{
    public string Id { get; set; } = null!;

    public string? ParentId { get; set; }

    public string? GroupType { get; set; }

    public string? GroupId { get; set; }

    public string? EmployeeId { get; set; }

    public bool IsActive { get; set; }

    public string? CreatedDate { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedDate { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual Staff? Employee { get; set; }
}
