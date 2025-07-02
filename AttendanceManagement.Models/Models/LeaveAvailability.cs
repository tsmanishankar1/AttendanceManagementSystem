using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class LeaveAvailability
{
    public int Id { get; set; }

    public int UserManagementId { get; set; }

    public decimal? NonConfirmedLeave { get; set; }

    public decimal? SickLeave { get; set; }

    public decimal? CasualLeave { get; set; }

    public decimal? MarriageLeave { get; set; }

    public decimal? CompOff { get; set; }

    public decimal? OnDuty { get; set; }

    public decimal? BusinessTravel { get; set; }

    public decimal? WorkFromHome { get; set; }

    public decimal? WeeklyOff { get; set; }

    public decimal? MaternityLeave { get; set; }

    public decimal? UnProcessed { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<MyApplication> MyApplications { get; set; } = new List<MyApplication>();

    public virtual StaffCreation? UpdatedByNavigation { get; set; }

    public virtual UserManagement UserManagement { get; set; } = null!;
}
