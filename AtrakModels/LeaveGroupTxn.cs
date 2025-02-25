using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class LeaveGroupTxn
{
    public int Id { get; set; }

    public string? LeaveGroupId { get; set; }

    public string? LeaveTypeId { get; set; }

    public int LeaveCount { get; set; }

    public int MaxSeqLeaves { get; set; }

    public bool IsActive { get; set; }

    public bool PaidLeave { get; set; }

    public bool Accountable { get; set; }

    public bool CarryForward { get; set; }

    public decimal MaxAccDays { get; set; }

    public decimal MaxAccYears { get; set; }

    public decimal MaxDaysPerReq { get; set; }

    public decimal MinDaysPerReq { get; set; }

    public int ElgInMonths { get; set; }

    public bool IsCalcToWorkingDays { get; set; }

    public decimal CalcToWorkingDays { get; set; }

    public bool ConsiderWo { get; set; }

    public bool ConsiderPh { get; set; }

    public bool IsExcessEligibleAllowed { get; set; }

    public bool IsEnCashmentAllowed { get; set; }

    public decimal EncashmentLimit { get; set; }

    public decimal CreditFreq { get; set; }

    public decimal CreditDays { get; set; }

    public bool ProRata { get; set; }

    public string? RoundOffTo { get; set; }

    public int RoundOffValue { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public string? LeavePayType { get; set; }

    public bool IsHalfDayApplicable { get; set; }

    public bool CheckBalance { get; set; }

    public int ComponentId { get; set; }

    public string? Lcafor { get; set; }

    public virtual LeaveGroup? LeaveGroup { get; set; }

    public virtual LeaveType? LeaveType { get; set; }
}
