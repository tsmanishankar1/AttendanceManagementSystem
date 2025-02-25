using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ShiftParentTxn
{
    public int Id { get; set; }

    public string ShiftId { get; set; } = null!;

    public string ParentId { get; set; } = null!;

    public string CompanyId { get; set; } = null!;

    public string BranchId { get; set; } = null!;

    public string DepartmentId { get; set; } = null!;

    public string DivisionId { get; set; } = null!;

    public string DesignationId { get; set; } = null!;

    public string GradeId { get; set; } = null!;

    public string CategoryId { get; set; } = null!;

    public string CostCentreId { get; set; } = null!;

    public string LocationId { get; set; } = null!;

    public string ParentType { get; set; } = null!;

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public string? VolumeId { get; set; }
}
