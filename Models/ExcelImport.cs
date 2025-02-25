using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class ExcelImport
{
    public int Id { get; set; }

    public string ImportName { get; set; } = null!;
}
