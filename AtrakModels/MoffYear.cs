using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class MoffYear
{
    public int Id { get; set; }

    public DateTime MoffStartDate { get; set; }

    public DateTime MoffEndDate { get; set; }
}
