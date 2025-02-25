using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class SmxReader
{
    public int DeId { get; set; }

    public string DeIpaddress { get; set; } = null!;

    public int DeNodeid { get; set; }

    public string DeName { get; set; } = null!;

    public int DeLnId { get; set; }

    public string? DeMessage { get; set; }

    public string DeReadertype { get; set; } = null!;

    public int? DeRelaytime { get; set; }

    public int? DeDotl { get; set; }

    public decimal? DeDotz { get; set; }

    public string? DeIp1Name { get; set; }

    public string? DeIp2Name { get; set; }

    public string? DeIp1Nonc { get; set; }

    public string? DeIp2Nonc { get; set; }

    public string? DeMemory { get; set; }

    public string? DeOperational { get; set; }

    public string? DeModel { get; set; }

    public string? DeFirmware { get; set; }

    public string? DeReadermode { get; set; }

    public int? DeFirealarm { get; set; }

    public DateTime? DeCreated { get; set; }

    public DateTime? DeModified { get; set; }

    public string? DeModifiedby { get; set; }

    public int? DeIntzid { get; set; }

    public string? DeRemarks { get; set; }
}
