namespace AttendanceManagement.Input_Models
{
    public class SalaryStructureRequest
    {
        public IFormFile File { get; set; } = null!;
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
}