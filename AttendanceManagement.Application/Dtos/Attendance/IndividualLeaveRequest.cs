using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Application.Dtos.Attendance
{
    public class IndividualLeaveRequest
    {
        public int LeaveTypeId { get; set; }
        public int DepartmentId { get; set; }
        public int StaffCreationId {  get; set; }
        public bool TransactionFlag { get; set; }
        [MaxLength(255)]
        public string LeaveReason { get; set; } = null!;
        [MaxLength(20)]
        public string Month { get; set; } = null!;
        public int Year { get; set; }
        public decimal LeaveCount { get; set; }
        public bool Isactive { get; set; }
        [MaxLength(255)]
        public string? Remarks { get; set; }
        public decimal? ActualBalance { get; set; }
        public decimal? AvailableBalance { get; set; }
        public int CreatedBy { get; set; }
    }

    public class LeaveCreditDebitRequest
    {
        public IEnumerable<SelectedStaff> SelectedRows { get; set; } = null!;
        public int LeaveTypeId { get; set; }
        public bool TransactionFlag { get; set; }
        [MaxLength(255)]
        public string LeaveReason { get; set; } = null!;
        [MaxLength(20)]
        public string Month { get; set; } = null!;
        public int Year { get; set; }
        public decimal LeaveCount { get; set; }
        [MaxLength(255)]
        public string? Remarks { get; set; }
        public int CreatedBy { get; set; }
    }

    public class SelectedStaff
    {
        public int StaffId { get; set; }
    }

    public class AssignLeaveTypeDTO
    {
        public int Id { get; set; }
        public int LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; } = null!;
        public int OrganizationTypeId { get; set; }
    }

    public class CreateAssignLeaveTypeDTO
    {
        public int LeaveTypeId { get; set; }
        public int OrganizationTypeId { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateAssignLeaveTypeDTO
    {
        public int Id { get; set; }
        public int LeaveTypeId { get; set; }
        public int OrganizationTypeId { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class StaffLeaveDto
    {
        public int StaffId { get; set; }
        public string StaffName { get; set; } = null!;
        public List<LeaveDetailDto> LeaveDetails { get; set; } = null!;
    }

    public class LeaveDetailDto
    {
        public int LeaveTypeId { get; set; }
        public string LeaveTypeName { get; set; } = null!;
        public decimal AvailableBalance { get; set; }
    }
}