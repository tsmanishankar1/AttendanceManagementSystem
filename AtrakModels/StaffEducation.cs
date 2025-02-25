using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class StaffEducation
{
    public string Id { get; set; } = null!;

    public string? StaffId { get; set; }

    public string? CourseName { get; set; }

    public string? University { get; set; }

    public bool Completed { get; set; }

    public int CompletionYear { get; set; }

    public decimal Percentage { get; set; }

    public string? Grade { get; set; }

    public virtual Staff? Staff { get; set; }
}
