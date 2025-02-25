using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class FinancialYear
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime From { get; set; }

    public DateTime To { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }
}
