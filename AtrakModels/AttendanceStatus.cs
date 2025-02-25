using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class AttendanceStatus
{
    public int Id { get; set; }

    public string? StatusName { get; set; }

    public string? StatusShortName { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public bool? ConsiderAsPresent { get; set; }

    public string? ColorCode { get; set; }

    public string? FhcolorCode { get; set; }
}
