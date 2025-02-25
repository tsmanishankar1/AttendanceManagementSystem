using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class LaterOffDate
{
    public int Id { get; set; }

    public DateTime ActionDate { get; set; }

    public int Validity { get; set; }

    public string CompanyId { get; set; } = null!;

    public string? CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }
}
