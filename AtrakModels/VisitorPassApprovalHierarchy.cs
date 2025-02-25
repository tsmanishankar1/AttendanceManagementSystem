using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VisitorPassApprovalHierarchy
{
    public int Id { get; set; }

    public string GradeId { get; set; } = null!;

    public bool SendForApproval { get; set; }

    public DateTime? CreateOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }
}
