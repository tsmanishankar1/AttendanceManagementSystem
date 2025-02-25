using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxExceptionCardHistory
{
    public decimal Id { get; set; }

    public string? ExpUserId { get; set; }

    public decimal? ExpCardNo { get; set; }

    public string? ExpStatus { get; set; }

    public DateOnly? ExpCreatedDate { get; set; }

    public string? ExpComments { get; set; }
}
