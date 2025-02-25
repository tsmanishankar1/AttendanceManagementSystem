using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ShiftPattern
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public bool IsRotational { get; set; }

    public bool IsLifeTime { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime? UpdatedUntil { get; set; }

    public int DayPattern { get; set; }

    public DateTime WostartDate { get; set; }

    public int WodayOffSet { get; set; }

    public DateTime? WolastUpdatedDate { get; set; }

    public bool IsActive { get; set; }

    public bool UsedAsGeneralShift { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public virtual ICollection<EmployeeGroupShiftPatternTxn> EmployeeGroupShiftPatternTxns { get; set; } = new List<EmployeeGroupShiftPatternTxn>();

    public virtual ICollection<ShiftPatternTxn> ShiftPatternTxns { get; set; } = new List<ShiftPatternTxn>();
}
