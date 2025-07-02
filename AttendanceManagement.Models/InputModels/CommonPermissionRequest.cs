using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.InputModels
{
    public partial class CommonPermissionRequest
    {
        public TimeOnly StartTime { get; set; }
        public int? StaffId { get; set; }
        public TimeOnly EndTime { get; set; }
        public int ApplicationTypeId { get; set; }
        public DateOnly PermissionDate { get; set; }
        [MaxLength(50)]
        public string PermissionType { get; set; } = null!;
        [MaxLength(255)]
        public string Remarks { get; set; } = null!;
        public int CreatedBy { get; set; }
    }

    public class CommonPermissionResponse
    {
        public int PermissionId { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string TotalHours { get; set; } = null!;
        public DateOnly PermissionDate { get; set; }
        public string PermissionType { get; set; } = null!;
        public bool? Status { get; set; }
        public string? Remarks { get; set; }
        public string StaffName { get; set; } = null!;
        public int CreatedBy { get; set; }
    }

    public class StaffPermissionResponse
    {
        public int Id { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string TotalHours { get; set; } = null!;
        public DateOnly PermissionDate { get; set; }
        public string PermissionType { get; set; } = null!;
        public bool? Status { get; set; }
        public string? Remarks { get; set; }
        public int? StaffId { get; set; }
        public int? ApplicationTypeId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }

    }

    public class UpdateCommonPermission
    {
        public string Status { get; set; } = null!;
        public int UpdatedBy { get; set; }
        public IEnumerable<ApprovePermission> SelectedRows { get; set; } = null!;
    }

    public class ApprovePermission
    {
        public int PermissionId { get; set; }
    }

    public class GetCommonStaff
    {
        public string? CompanyName { get; set; }
        public string? CategoryName { get; set; }
        public string? CostCentreName { get; set; }
        public string? BranchName { get; set; }
        public string? DepartmentName { get; set; }
        public string? DesignationName { get; set; }
        public string? StaffName { get; set; }
        public string? LocationName { get; set; }
        public string? OrganizationTypeName { get; set; }
        public string? GradeName { get; set; }
    }

    public class GetStaffByApproverRequest
    {
        public int ApproverId { get; set; }
        public string? CompanyName { get; set; }
        public string? CategoryName { get; set; }
        public string? CostCentreName { get; set; }
        public string? BranchName { get; set; }
        public string? DepartmentName { get; set; }
        public string? DesignationName { get; set; }
        public string? StaffName { get; set; }
        public string? LocationName { get; set; }
        public string? GradeName { get; set; }
        public string? OrganizationTypeName { get; set; }
        public string? ShiftName { get; set; }
        public string? Status { get; set; }
    }
}