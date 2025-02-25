using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class VwSmxCardAccesslevelAutoDelete
{
    public decimal ChId { get; set; }

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

    public decimal? AdAlId { get; set; }

    public bool? AdDeleted { get; set; }

    public bool? AdDwstatus { get; set; }

    public decimal AldTzId { get; set; }

    public string AldReaderIpaddress { get; set; } = null!;

    public int AldLnId { get; set; }
}
