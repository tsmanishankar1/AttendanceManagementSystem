namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class StatutoryReport
{
    public int Id { get; set; }

    public DateOnly FromDate { get; set; }

    public DateOnly ToDate { get; set; }

    public int CompanyMasterId { get; set; }

    public int CategoryMasterId { get; set; }

    public int? SubVolumeId { get; set; }

    public int CostCentreMasterId { get; set; }

    public int BranchMasterId { get; set; }

    public int DesignationMasterId { get; set; }

    public int LocationMasterId { get; set; }

    public int? MainVolumeId { get; set; }

    public int GradeMasterId { get; set; }

    public int UserManagementId { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual BranchMaster BranchMaster { get; set; } = null!;

    public virtual CategoryMaster CategoryMaster { get; set; } = null!;

    public virtual CompanyMaster CompanyMaster { get; set; } = null!;

    public virtual CostCentreMaster CostCentreMaster { get; set; } = null!;

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual DesignationMaster DesignationMaster { get; set; } = null!;

    public virtual GradeMaster GradeMaster { get; set; } = null!;

    public virtual LocationMaster LocationMaster { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }

    public virtual UserManagement UserManagement { get; set; } = null!;
}
