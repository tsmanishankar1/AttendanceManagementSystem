using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class VwSmxCardAccesslevel
{
    public string? ChCardNo { get; set; }

    public string ChEmpId { get; set; } = null!;

    public string ChFname { get; set; } = null!;

    public int? ChPinNo { get; set; }

    public DateTime? ChValidTo { get; set; }

    public int? ChMsId { get; set; }

    public bool ChIsbio { get; set; }

    public bool ChIscard { get; set; }

    public bool ChIsCardBio { get; set; }

    public bool ChIspin { get; set; }

    public bool ChAntiPassBack { get; set; }

    public bool? ChAccessValidation { get; set; }

    public decimal? CalAlId { get; set; }

    public bool? CalDeleted { get; set; }

    public bool? CalDwstatus { get; set; }

    public decimal AldTzId { get; set; }

    public string AldReaderIpaddress { get; set; } = null!;

    public int AldLnId { get; set; }
}
