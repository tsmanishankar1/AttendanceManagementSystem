namespace AttendanceManagement.Domain.Entities.Attendance;
public partial class AppraisalAnnexureA
{
    public int Id { get; set; }

    public string EmployeeCode { get; set; } = null!;

    public decimal Basic { get; set; }

    public decimal Hra { get; set; }

    public decimal Conveyance { get; set; }

    public decimal MedicalAllowance { get; set; }

    public decimal SpecialAllowance { get; set; }

    public decimal EmployerPfContribution { get; set; }

    public decimal EmployerEsiContribution { get; set; }

    public decimal EmployerGroupMedicalInsurance { get; set; }

    public decimal GroupPersonalAccident { get; set; }

    public decimal EmployeePfContribution { get; set; }

    public decimal EmployeeEsiContribution { get; set; }

    public decimal ProfessionalTax { get; set; }

    public decimal EmployeeGroupMedicalInsurance { get; set; }

    public decimal AppraisalAmount { get; set; }

    public int AppraisalYear { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
