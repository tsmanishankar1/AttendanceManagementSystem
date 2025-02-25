using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class SmxSwipedatum
{
    public DateTime? TrDate { get; set; }

    public DateTime? TrTime { get; set; }

    public decimal? TrTtype { get; set; }

    public string? TrMessage { get; set; }

    public decimal? TrNodeId { get; set; }

    public decimal? TrCardNumber { get; set; }

    public decimal? TrTrackCard { get; set; }

    public decimal? TrReason { get; set; }

    public string? TrIpAddress { get; set; }
}
