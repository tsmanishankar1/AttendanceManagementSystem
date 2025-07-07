using Microsoft.AspNetCore.Http;

namespace AttendanceManagement.Application.Dtos.Attendance
{
    public class SalaryStructureRequest
    {
        public IFormFile File { get; set; } = null!;
        public int CreatedBy { get; set; }
    }

    public class PaySlipGenerate
    {
        public int StaffId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int CreatedBy { get; set; }
    }

    public class PayslipResponse
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string StaffName { get; set; } = null!;
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
        public int CreatedBy { get; set; }
        public List<PaySlipComponentResponse>? PaySlipComponents { get; set; }
    }

    public class PaySlipComponentResponse
    {
        public int Id { get; set; }
        public string? ComponentType { get; set; }
        public string? ComponentName { get; set; }
        public string? Amount { get; set; }
        public bool? IsTaxable { get; set; }
        public string? Remarks { get; set; }
    }

    public class SalaryStructureResponse
    {
        public int Id { get; set; }
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string StaffName { get; set; } = null!;
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
        public List<PaySlipComponentResponse>? SalaryComponents { get; set; }
    }

    public class PaysheetResponse
    {
        public string StaffId { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public string GroupName { get; set; } = null!;
        public string DisplayNameInReports { get; set; } = null!;
        public string Month { get; set; } = null!;
        public int Year { get; set; }
        public DateOnly DateOfJoining { get; set; }
        public int EmployeeNumber { get; set; }
        public string Designation { get; set; } = null!;
        public string Department { get; set; } = null!;
        public string Location { get; set; } = null!;
        public string Gender { get; set; } = null!;
        public DateOnly DateOfBirth { get; set; }
        public string FatherOrMotherName { get; set; } = null!;
        public string? SpouseName { get; set; }
        public string Address { get; set; } = null!;
        public string Email { get; set; } = null!;
        public long PhoneNo { get; set; }
        public string BankName { get; set; } = null!;
        public long AccountNo { get; set; }
        public string IfscCode { get; set; } = null!;
        public string PfAccountNo { get; set; } = null!;
        public long Uan { get; set; }
        public string Pan { get; set; } = null!;
        public long AadhaarNo { get; set; }
        public long EsiNo { get; set; }
        public DateOnly SalaryEffectiveFrom { get; set; }
        public decimal BasicActual { get; set; }
        public decimal HraActual { get; set; }
        public decimal ConveActual { get; set; }
        public decimal MedAllowActual { get; set; }
        public decimal SplAllowActual { get; set; }
        public decimal LopDays { get; set; }
        public decimal StdDays { get; set; }
        public decimal WrkDays { get; set; }
        public string PfAdmin { get; set; } = null!;
        public decimal BasicEarned { get; set; }
        public decimal BasicArradj { get; set; }
        public decimal HraEarned { get; set; }
        public decimal HraArradj { get; set; }
        public decimal ConveEarned { get; set; }
        public decimal ConveArradj { get; set; }
        public decimal MedAllowEarned { get; set; }
        public decimal MedAllowArradj { get; set; }
        public decimal SplAllowEarned { get; set; }
        public decimal SplAllowArradj { get; set; }
        public decimal OtherAll { get; set; }
        public decimal GrossEarn { get; set; }
        public decimal Pf { get; set; }
        public decimal Esi { get; set; }
        public decimal Lwf { get; set; }
        public decimal Pt { get; set; }
        public decimal It { get; set; }
        public decimal MedClaim { get; set; }
        public decimal OtherDed { get; set; }
        public decimal GrossDed { get; set; }
        public decimal NetPay { get; set; }
    }

    public class AppraisalLetterModel
    {
        public string EmployeeName { get; set; } = null!;
        public string EmployeeCode { get; set; } = null!;
        public string NewDesignation { get; set; } = null!;
        public decimal IncrementAmount { get; set; }
        public string IncrementAmountInWords { get; set; } = null!;
        public DateTime EffectiveDate { get; set; }
        public DateTime LetterDate { get; set; }
        public string NextAppraisalMonthYear { get; set; } = null!;
        public string CurrentBasic { get; set; } = null!;
        public string NewBasic { get; set; } = null!;
        public string CurrentHRA { get; set; } = null!;
        public string NewHRA { get; set; } = null!;
        public string CurrentSpecialAllowance { get; set; } = null!;
        public string NewSpecialAllowance { get; set; } = null!;
        public string CurrentGross { get; set; } = null!;
        public string NewGross { get; set; } = null!;
    }

    public class AppraisalAnnexureResponse
    {
        public string EmployeeCode { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public bool IsDesignationChange { get; set; }
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
        public decimal GrossMonthCurrent {  get; set; }
        public decimal GrossPerAnnumCurrent { get; set; }
        public decimal GrossMonthAfterApp {  get; set; }
        public decimal GrossPerAnnumAfterApp { get; set; }
        public decimal? EmployerPfcontributionPerAnnum { get; set; }
        public decimal? EmployerPfcontributionPerMonth { get; set; }
        public decimal? EmployerPfcontributionPerAnnumAfterApp { get; set; }
        public decimal? EmployerPfcontributionPerMonthAfterApp { get; set; }
        public decimal? EmployerEsicontributionPerAnnum { get; set; }
        public decimal? EmployerEsicontributionPerMonth { get; set; }
        public decimal? EmployerEsicontributionPerAnnumAfterApp { get; set; }
        public decimal? EmployerEsicontributionPerMonthAfterApp { get; set; }
        public decimal? EmployerGmcperAnnum { get; set; }
        public decimal? EmployerGmcperMonth { get; set; }
        public decimal? EmployerGmcperAnnumAfterApp { get; set; }
        public decimal? EmployerGmcperMonthAfterApp { get; set; }
        public decimal? GroupPersonalAccidentPerAnnum { get; set; }
        public decimal? GroupPersonalAccidentPerMonth { get; set; }
        public decimal? GroupPersonalAccidentPerAnnumAfterApp { get; set; }
        public decimal? GroupPersonalAccidentPerMonthAfterApp { get; set; }
        public decimal CtcMonthCurrent { get; set; }
        public decimal CtcPerAnnumCurrent {  get; set; }
        public decimal CtcMonthAfterApp { get; set; }
        public decimal CtcPerAnnumAfterApp { get; set; }
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
        public decimal? GmcperAnnum { get; set; }
        public decimal? GmcperMonth { get; set; }
        public decimal? GmcperAnnumAfterApp { get; set; }
        public decimal? GmcperMonthAfterApp { get; set; }
        public decimal NetTakeHomeMonthCurrent { get; set; }
        public decimal NetTakeHomePerAnnumCurrent { get; set; }
        public decimal NetTakeHomeMonthAfterApp { get; set; }
        public decimal NetTakeHomePerAnnumAfterApp { get; set; }
        public decimal TotalAppraisal { get; set; }
    }

    /*    public class PreviousYearAppraisal
        {
            public decimal Basic { get; set; }
            public decimal Hra { get; set; }
            public decimal Conveyance { get; set; }
            public decimal MedicalAllowance { get; set; }
            public decimal SpecialAllowance { get; set; }
            public decimal Gross { get; set; }
            public decimal EmployerPfContribution { get; set; }
            public decimal EmployerEsiContribution { get; set; }
            public decimal EmployerGroupMedicalInsurance { get; set; }
            public decimal GroupPersonalAccident { get; set; }
            public decimal Ctc { get; set; }
            public decimal EmployeePfContribution { get; set; }
            public decimal EmployeeEsiContribution { get; set; }
            public decimal ProfessionalTax { get; set; }
            public decimal EmployeeGroupMedicalInsurance { get; set; }
            public decimal NetTakeHome { get; set; }
            public decimal AppraisalAmount { get; set; }
            public int AppraisalYear { get; set; }
        }

        public class CurrentYearAppraisal
        {
            public decimal Basic { get; set; }
            public decimal Hra { get; set; }
            public decimal Conveyance { get; set; }
            public decimal MedicalAllowance { get; set; }
            public decimal SpecialAllowance { get; set; }
            public decimal Gross { get; set; }
            public decimal EmployerPfContribution { get; set; }
            public decimal EmployerEsiContribution { get; set; }
            public decimal EmployerGroupMedicalInsurance { get; set; }
            public decimal GroupPersonalAccident { get; set; }
            public decimal Ctc { get; set; }
            public decimal EmployeePfContribution { get; set; }
            public decimal EmployeeEsiContribution { get; set; }
            public decimal ProfessionalTax { get; set; }
            public decimal EmployeeGroupMedicalInsurance { get; set; }
            public decimal NetTakeHome { get; set; }
            public decimal AppraisalAmount { get; set; }
            public int AppraisalYear { get; set; }
        }
    */
    public class GeneratePaySheetRequest
    {
        public int StaffId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int CreatedBy { get; set; }
    }

    public class GenerateAppraisalLetterRequest
    {
        public List<string> StaffId { get; set; } = null!;
        public int CreatedBy { get; set; }
    }
}