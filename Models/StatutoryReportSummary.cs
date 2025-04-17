using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class StatutoryReportSummary
{
    public int Id { get; set; }

    public int StaffId { get; set; }

    public decimal NoOfDaysInMonth { get; set; }

    public decimal Lop { get; set; }

    public decimal NumberOfDaysToBePaid { get; set; }

    public decimal NightShiftPresentDays { get; set; }

    public bool IsAttendanceBonus { get; set; }

    public int? AttendanceBonusMonths { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public DateOnly ReportFromDate { get; set; }

    public DateOnly ReportToDate { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation Staff { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
