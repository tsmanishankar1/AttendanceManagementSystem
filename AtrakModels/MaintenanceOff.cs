using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class MaintenanceOff
{
    public string Id { get; set; } = null!;

    public string StaffId { get; set; } = null!;

    public DateTime DateFrom { get; set; }

    public DateTime DateTo { get; set; }

    public string Reason { get; set; } = null!;

    public string? ContactNumber { get; set; }

    public bool IsCancelled { get; set; }

    public DateTime? ApplicationDate { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public bool IsFlexible { get; set; }

    public int MoffYear { get; set; }

    public virtual Staff Staff { get; set; } = null!;
}
