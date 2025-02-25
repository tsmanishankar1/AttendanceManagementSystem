using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ShiftPostingPattern
{
    public int Id { get; set; }

    public int? PatternId { get; set; }

    public string? Sunday { get; set; }

    public string? Monday { get; set; }

    public string? Tuesday { get; set; }

    public string? Wednesday { get; set; }

    public string? Thursday { get; set; }

    public string? Friday { get; set; }

    public string? Saturday { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }
}
