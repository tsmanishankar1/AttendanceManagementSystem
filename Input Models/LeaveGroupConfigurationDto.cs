namespace AttendanceManagement.Input_Models
{
    public class LeaveGroupConfigurationDto
    {
        public int LeaveGroupConfigurationId { get; set; }
        public string? LeaveGroupConfigurationName { get; set; }
        public int LeaveTypeId { get; set; }
        public string? LeaveTypeName { get; set; }
        public bool? PaidLeave { get; set; }
        public bool? Accountable { get; set; }
        public bool? CarryForward { get; set; }
        public int? MaxAccountDays { get; set; }
        public int? MaxAccountYears { get; set; }
        public int? MaxDaysPerReq { get; set; }
        public int? MinDaysPerReq { get; set; }
        public bool? ProRata { get; set; }
        public int? ElgInMonths { get; set; }
        public bool? IsCalcToWorkingDays { get; set; }
        public int? ClacToWorkingDays { get; set; }
        public bool? ConsiderWo { get; set; }
        public bool? ConsiderPh { get; set; }
        public bool? IsExcessEligibleAllowed { get; set; }
        public bool? IsHalfDayApplicable { get; set; }
        public bool? IsEncashmentAllowed { get; set; }
        public int? CreditFreq { get; set; }
        public int? CreditDays { get; set; }
        public int? RoundOffTo { get; set; }
        public decimal? RoundOffValue { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class LeaveGroupConfigurationRequest
    {
        public string LeaveGroupConfigurationName { get; set; } = null!;
        public int LeaveTypeId { get; set; }
        public bool PaidLeave { get; set; }
        public bool Accountable { get; set; }
        public bool CarryForward { get; set; }
        public int MaxAccountDays { get; set; }
        public int MaxAccountYears { get; set; }
        public int MaxDaysPerReq { get; set; }
        public int MinDaysPerReq { get; set; }
        public bool ProRata { get; set; }
        public int ElgInMonths { get; set; }
        public bool IsCalcToWorkingDays { get; set; }
        public int ClacToWorkingDays { get; set; }
        public bool ConsiderWo { get; set; }
        public bool ConsiderPh { get; set; }
        public bool IsExcessEligibleAllowed { get; set; }
        public bool IsHalfDayApplicable { get; set; }
        public bool IsEncashmentAllowed { get; set; }
        public int CreditFreq { get; set; }
        public int CreditDays { get; set; }
        public int RoundOffTo { get; set; }
        public decimal RoundOffValue { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class UpdateLeaveGroupConfiguration
    {
        public int LeaveGroupConfigurationId { get; set; }
        public string? LeaveGroupConfigurationName { get; set; }
        public int? LeaveTypeId { get; set; }
        public bool? PaidLeave { get; set; }
        public bool? Accountable { get; set; }
        public bool? CarryForward { get; set; }
        public int? MaxAccountDays { get; set; }
        public int? MaxAccountYears { get; set; }
        public int? MaxDaysPerReq { get; set; }
        public int? MinDaysPerReq { get; set; }
        public bool? ProRata { get; set; }
        public int? ElgInMonths { get; set; }
        public bool? IsCalcToWorkingDays { get; set; }
        public int? ClacToWorkingDays { get; set; }
        public bool? ConsiderWo { get; set; }
        public bool? ConsiderPh { get; set; }
        public bool? IsExcessEligibleAllowed { get; set; }
        public bool? IsHalfDayApplicable { get; set; }
        public bool? IsEncashmentAllowed { get; set; }
        public int? CreditFreq { get; set; }
        public int? CreditDays { get; set; }
        public int? RoundOffTo { get; set; }
        public decimal? RoundOffValue { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}