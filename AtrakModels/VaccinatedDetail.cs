using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class VaccinatedDetail
{
    public string StaffId { get; set; } = null!;

    public bool IsFirstVaccination { get; set; }

    public DateTime FirstVaccinatedDate { get; set; }

    public bool IsSecondVaccination { get; set; }

    public DateTime? SecondVaccinatedDate { get; set; }

    public DateTime UploadedOn { get; set; }

    public string UploadedBy { get; set; } = null!;

    public bool IsExempted { get; set; }

    public string? Comments { get; set; }

    public virtual Staff Staff { get; set; } = null!;
}
