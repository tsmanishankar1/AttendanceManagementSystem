using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Screen
{
    public int Id { get; set; }

    public string ScreenName { get; set; } = null!;

    public short ScreenOption { get; set; }

    public short Level { get; set; }

    public short ParentId { get; set; }
}
