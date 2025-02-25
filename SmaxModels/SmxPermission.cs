using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxPermission
{
    public decimal PrId { get; set; }

    public string PrChId { get; set; } = null!;

    public DateTime PrDate { get; set; }

    public DateTime PrStartTime { get; set; }

    public DateTime PrEndTime { get; set; }

    public int? FkDpId { get; set; }

    public string? PrDescription { get; set; }

    public string? PrPermission { get; set; }

    public string? PrUpdatedBy { get; set; }

    public DateTime? PrCreatedDate { get; set; }

    public DateTime? PrModifiedDate { get; set; }
}
