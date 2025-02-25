using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class UploadControlTable
{
    public int Id { get; set; }

    public string? Filename { get; set; }

    public string TypeOfData { get; set; } = null!;

    public DateTime UploadedOn { get; set; }

    public string UploadedBy { get; set; } = null!;

    public bool IsProcessed { get; set; }

    public DateTime? ProcessedOn { get; set; }

    public string? ProcessStatus { get; set; }

    public bool IsError { get; set; }

    public string? ErrorMessage { get; set; }
}
