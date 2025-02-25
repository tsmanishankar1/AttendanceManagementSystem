using System;
using System.Collections.Generic;

namespace AttendanceManagement.SmaxModels;

public partial class SmxConfiguration
{
    public decimal CfId { get; set; }

    public short? CfHoursettings { get; set; }

    public bool? CfAccessvalidation { get; set; }

    public short? CfMaxFlag { get; set; }

    public TimeOnly? CfStarttime { get; set; }

    public TimeOnly? CfEndtime { get; set; }
}
