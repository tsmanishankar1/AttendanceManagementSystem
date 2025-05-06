using AttendanceManagement.Models;

namespace AttendanceManagement.Input_Models
{
    public class ProbationRequest
    {
        public int StaffId { get; set; }
        public DateOnly ProbationStartDate { get; set; }
        public DateOnly ProbationEndDate { get; set; }
        public int CreatedBy { get; set; }
    }

    public class AssignManagerRequest
    {
        public int ProbationId { get; set; }
        public int ManagerId { get; set; }
        public int CreatedBy { get; set; }
    }

    public class ProbationResponse
    {
        public int ProbationId { get; set; }
        public int StaffId { get; set; }
        public string StaffCreationId { get; set; } = null!;
        public string StaffName { get; set; } = null!;
        public string DepartmentName { get; set; } = null!;
        public DateOnly ProbationStartDate { get; set; }
        public DateOnly ProbationEndDate { get; set; }
        public int CreatedBy { get; set; }
        public ProbationReport? ProbationReport { get; set; }
    }

    public class UpdateProbation
    {
        public int ProbationId { get; set; }
        public int StaffId { get; set; }
        public DateOnly ProbationStartDate { get; set; }
        public DateOnly ProbationEndDate { get; set; }
        public bool? IsCompleted { get; set; }
        public int UpdatedBy { get; set; }
    }
}