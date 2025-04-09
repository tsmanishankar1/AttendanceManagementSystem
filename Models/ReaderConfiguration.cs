using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class ReaderConfiguration
{
    public int Id { get; set; }

    public string ReaderName { get; set; } = null!;

    public string ReaderIpAddress { get; set; } = null!;

    public bool IsAttendanceReader { get; set; }

    public bool IsAccessReader { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public int ReaderTypeId { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ReaderType ReaderType { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
