using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VaccinationDetailsDump
{
    public long Id { get; set; }

    public string StaffId { get; set; } = null!;

    public int VaccinationNumber { get; set; }

    public DateTime VaccinatedDate { get; set; }

    public DateTime CreatedOn { get; set; }

    public string CreatedBy { get; set; } = null!;

    public bool? IsError { get; set; }

    public string? ErrorMessage { get; set; }

    public bool IsProcessed { get; set; }

    public string? ExcelFileName { get; set; }

    public bool IsExempted { get; set; }

    public string? Comments { get; set; }

    public virtual Staff Staff { get; set; } = null!;
}
