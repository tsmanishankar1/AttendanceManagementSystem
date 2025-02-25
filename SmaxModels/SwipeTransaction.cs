using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SwipeTransaction
{
    public decimal TrId { get; set; }

    public DateTime? TrDate { get; set; }

    public DateTime? TrTime { get; set; }

    public decimal? TrTtype { get; set; }

    public decimal? TrNodeId { get; set; }

    public decimal? TrCardNumber { get; set; }

    public decimal? TrReason { get; set; }

    public string? TrIpAddress { get; set; }

    public string? TrMessage { get; set; }

    public decimal? TrTrackCard { get; set; }

    public string TrChid { get; set; } = null!;

    public string DeName { get; set; } = null!;

    public string DeReaderType { get; set; } = null!;
}
