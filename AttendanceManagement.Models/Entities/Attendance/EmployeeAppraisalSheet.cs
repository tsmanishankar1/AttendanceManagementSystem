namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class EmployeeAppraisalSheet
{
    public int Id { get; set; }

    public string EmployeeCode { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

    public string Designation { get; set; } = null!;

    public string? RevisedDesignation { get; set; }

    public string Department { get; set; } = null!;

    public int AppraisalYear { get; set; }

    public decimal BasicCurrentPerAnnum { get; set; }

    public decimal BasicCurrentPerMonth { get; set; }

    public decimal BasicCurrentPerAnnumAfterApp { get; set; }

    public decimal BasicCurrentPerMonthAfterApp { get; set; }

    public decimal? HraperAnnum { get; set; }

    public decimal? HraperMonth { get; set; }

    public decimal? HraperAnnumAfterApp { get; set; }

    public decimal? HraperMonthAfterApp { get; set; }

    public decimal? ConveyancePerAnnum { get; set; }

    public decimal? ConveyancePerMonth { get; set; }

    public decimal? ConveyancePerAnnumAfterApp { get; set; }

    public decimal? ConveyancePerMonthAfterApp { get; set; }

    public decimal? MedicalAllowancePerAnnum { get; set; }

    public decimal? MedicalAllowancePerMonth { get; set; }

    public decimal? MedicalAllowancePerAnnumAfterApp { get; set; }

    public decimal? MedicalAllowancePerMonthAfterApp { get; set; }

    public decimal? SpecialAllowancePerAnnum { get; set; }

    public decimal? SpecialAllowancePerMonth { get; set; }

    public decimal? SpecialAllowancePerAnnumAfterApp { get; set; }

    public decimal? SpecialAllowancePerMonthAfterApp { get; set; }

    public decimal? EmployerPfcontributionPerAnnum { get; set; }

    public decimal? EmployerPfcontributionPerMonth { get; set; }

    public decimal? EmployerPfcontributionPerAnnumAfterApp { get; set; }

    public decimal? EmployerPfcontributionPerMonthAfterApp { get; set; }

    public decimal? EmployerEsicontributionPerAnnum { get; set; }

    public decimal? EmployerEsicontributionPerMonth { get; set; }

    public decimal? EmployerEsicontributionPerAnnumAfterApp { get; set; }

    public decimal? EmployerEsicontributionPerMonthAfterApp { get; set; }

    public decimal? GroupPersonalAccidentPerAnnum { get; set; }

    public decimal? GroupPersonalAccidentPerMonth { get; set; }

    public decimal? GroupPersonalAccidentPerAnnumAfterApp { get; set; }

    public decimal? GroupPersonalAccidentPerMonthAfterApp { get; set; }

    public decimal? EmployeePfcontributionPerAnnum { get; set; }

    public decimal? EmployeePfcontributionPerMonth { get; set; }

    public decimal? EmployeePfcontributionPerAnnumAfterApp { get; set; }

    public decimal? EmployeePfcontributionPerMonthAfterApp { get; set; }

    public decimal? EmployeeEsicontributionPerAnnum { get; set; }

    public decimal? EmployeeEsicontributionPerMonth { get; set; }

    public decimal? EmployeeEsicontributionPerAnnumAfterApp { get; set; }

    public decimal? EmployeeEsicontributionPerMonthAfterApp { get; set; }

    public decimal? ProfessionalTaxPerAnnum { get; set; }

    public decimal? ProfessionalTaxPerMonth { get; set; }

    public decimal? ProfessionalTaxPerAnnumAfterApp { get; set; }

    public decimal? ProfessionalTaxPerMonthAfterApp { get; set; }

    public string EmployeeSalutation { get; set; } = null!;

    public decimal TotalAppraisal { get; set; }

    public decimal? GmcperAnnum { get; set; }

    public decimal? GmcperMonth { get; set; }

    public decimal? GmcperAnnumAfterApp { get; set; }

    public decimal? GmcperMonthAfterApp { get; set; }

    public decimal? EmployerGmcperAnnum { get; set; }

    public decimal? EmployerGmcperMonth { get; set; }

    public decimal? EmployerGmcperAnnumAfterApp { get; set; }

    public decimal? EmployerGmcperMonthAfterApp { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
