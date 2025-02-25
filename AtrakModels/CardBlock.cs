using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class CardBlock
{
    public int Id { get; set; }

    public string StaffId { get; set; } = null!;

    public DateTime DateFrom { get; set; }

    public DateTime DateTo { get; set; }

    public int AbsentCount { get; set; }

    public bool IsCardBlocked { get; set; }

    public DateTime CardBlockedOn { get; set; }

    public bool IsCardOpened { get; set; }

    public DateTime CardOpenedOn { get; set; }

    public DateTime CreatedOn { get; set; }
}
