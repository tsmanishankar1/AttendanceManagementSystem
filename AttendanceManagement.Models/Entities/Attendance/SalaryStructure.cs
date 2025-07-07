namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class SalaryStructure
{
    public int Id { get; set; }

    public int StaffId { get; set; }

    public decimal Basic { get; set; }

    public decimal? Hra { get; set; }

    public decimal? Da { get; set; }

    public decimal? OtherAllowance { get; set; }

    public decimal? SpecialAllowance { get; set; }

    public decimal? Conveyance { get; set; }

    public bool Tdsapplicable { get; set; }

    public decimal? Tds { get; set; }

    public bool Esicapplicable { get; set; }

    public decimal? Esic { get; set; }

    public decimal? EsicemployerContribution { get; set; }

    public bool Pfapplicable { get; set; }

    public decimal Pf { get; set; }

    public decimal PfemployerContribution { get; set; }

    public bool Ptapplicable { get; set; }

    public decimal? Pt { get; set; }

    public bool Otapplicable { get; set; }

    public decimal? OtperHour { get; set; }

    public bool Lopapplicable { get; set; }

    public decimal? Lop { get; set; }

    public bool IsLopfixed { get; set; }

    public bool IsPffloating { get; set; }

    public bool IsEsicfloating { get; set; }

    public bool IsPtfloating { get; set; }

    public int SalaryStructureYear { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<SalaryComponent> SalaryComponents { get; set; } = new List<SalaryComponent>();

    public virtual StaffCreation Staff { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
