namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class PaySlip
{
    public int Id { get; set; }

    public int StaffId { get; set; }

    public decimal Basic { get; set; }

    public decimal? Hra { get; set; }

    public decimal? Da { get; set; }

    public decimal? OtherAllowance { get; set; }

    public decimal? SpecialAllowance { get; set; }

    public decimal? Conveyance { get; set; }

    public decimal? Tds { get; set; }

    public decimal? Esic { get; set; }

    public decimal Pf { get; set; }

    public decimal? Pt { get; set; }

    public decimal? Othours { get; set; }

    public decimal? OtperHour { get; set; }

    public decimal? TotalOt { get; set; }

    public decimal? LopperDay { get; set; }

    public decimal? AbsentDays { get; set; }

    public decimal? Lwopdays { get; set; }

    public decimal? TotalLop { get; set; }

    public bool IsFreezed { get; set; }

    public decimal? EsicemployerContribution { get; set; }

    public decimal PfemployerContribution { get; set; }

    public string SalaryMonth { get; set; } = null!;

    public int SalaryYear { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<PaySlipComponent> PaySlipComponents { get; set; } = new List<PaySlipComponent>();

    public virtual StaffCreation Staff { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
