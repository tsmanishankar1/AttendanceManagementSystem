using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class SmaxTransaction
{
    public long Id { get; set; }

    public DateTime? TrDate { get; set; }

    public DateTime? TrTime { get; set; }

    public int TrTtype { get; set; }

    public string? TrMessage { get; set; }

    public int TrNodeId { get; set; }

    public string? TrOpName { get; set; }

    public string? TrCardNumber { get; set; }

    public int TrTrackCard { get; set; }

    public int TrReason { get; set; }

    public int? TrLnId { get; set; }

    public string? TrIpaddress { get; set; }

    public string? TrChId { get; set; }

    public int? TrUnit { get; set; }

    public long? SmaxId { get; set; }

    public string? DeName { get; set; }

    public string? DeReadertype { get; set; }

    public long? TrId { get; set; }

    public DateTime? TrCreated { get; set; }

    public bool Isprocessed { get; set; }

    public DateTime? TrSourceCreatedOn { get; set; }
}
