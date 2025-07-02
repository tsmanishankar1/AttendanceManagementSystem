using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class StaffSalary
{
    public int Id { get; set; }

    public int StaffId { get; set; }

    public decimal BasicSalary { get; set; }

    public int TotalDays { get; set; }

    public int DaysPaid { get; set; }

    public decimal? Lopdays { get; set; }

    public decimal? ArrearDays { get; set; }

    public decimal TotalEarnings { get; set; }

    public decimal TotalDeductions { get; set; }

    public decimal NetSalary { get; set; }

    public DateOnly SalaryMonth { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<PaymentDetail> PaymentDetails { get; set; } = new List<PaymentDetail>();

    public virtual StaffCreation Staff { get; set; } = null!;

    public virtual ICollection<StaffSalaryBreakdown> StaffSalaryBreakdowns { get; set; } = new List<StaffSalaryBreakdown>();

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
