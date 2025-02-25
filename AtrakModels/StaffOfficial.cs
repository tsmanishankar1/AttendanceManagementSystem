using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class StaffOfficial
{
    public string StaffId { get; set; } = null!;

    public string? CompanyId { get; set; }

    public string? LocationId { get; set; }

    public string? BranchId { get; set; }

    public string? DepartmentId { get; set; }

    public string? DivisionId { get; set; }

    public string? DesignationId { get; set; }

    public string? GradeId { get; set; }

    public string? LeaveGroupId { get; set; }

    public string? WeeklyOffId { get; set; }

    public int HolidayGroupId { get; set; }

    public int PolicyId { get; set; }

    public string? CategoryId { get; set; }

    public string? CostCentreId { get; set; }

    public int SecurityGroupId { get; set; }

    public DateTime? DateOfJoining { get; set; }

    public DateTime? ResignationDate { get; set; }

    public bool IsConfirmed { get; set; }

    public DateTime? ConfirmationDate { get; set; }

    public string? Phone { get; set; }

    public string? Fax { get; set; }

    public string? Email { get; set; }

    public int ExtensionNo { get; set; }

    public int WorkingDayPatternId { get; set; }

    public string? ReportingManager { get; set; }

    public bool Canteen { get; set; }

    public bool Travel { get; set; }

    public bool IsWorkingDayPatternLocked { get; set; }

    public bool IsLeaveGroupLocked { get; set; }

    public bool IsHolidayGroupLocked { get; set; }

    public bool IsWeeklyOffLocked { get; set; }

    public bool IsPolicyLocked { get; set; }

    public int SalaryDay { get; set; }

    public string? Pfno { get; set; }

    public string? Esino { get; set; }

    public string? DomainId { get; set; }

    public string? VolumeId { get; set; }

    public bool Interimhike { get; set; }

    public decimal Tenure { get; set; }

    public DateTime? DateOfRelieving { get; set; }

    public bool AutoShift { get; set; }

    public bool GeneralShift { get; set; }

    public bool ShiftPattern { get; set; }

    public string? ShiftId { get; set; }

    public int ShiftPatternId { get; set; }

    public bool ManualShift { get; set; }

    public bool IsFlexi { get; set; }

    public string? Approver2 { get; set; }

    public string? WorkStationId { get; set; }

    public int ApproverLevel { get; set; }

    public bool IsOteligible { get; set; }

    public bool IsMobileAppEligible { get; set; }

    public int GeoStatus { get; set; }

    public virtual Branch? Branch { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Company? Company { get; set; }

    public virtual CostCentre? CostCentre { get; set; }

    public virtual Department? Department { get; set; }

    public virtual Designation? Designation { get; set; }

    public virtual Division? Division { get; set; }

    public virtual Grade? Grade { get; set; }

    public virtual HolidayZone HolidayGroup { get; set; } = null!;

    public virtual LeaveGroup? LeaveGroup { get; set; }

    public virtual Location? Location { get; set; }

    public virtual RuleGroup Policy { get; set; } = null!;

    public virtual SecurityGroup SecurityGroup { get; set; } = null!;

    public virtual Staff Staff { get; set; } = null!;

    public virtual Volume? Volume { get; set; }

    public virtual WorkingDayPattern WorkingDayPattern { get; set; } = null!;
}
