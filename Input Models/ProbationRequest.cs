namespace AttendanceManagement.Input_Models
{
    public class ProbationRequest
    {
        public int StaffCreationId { get; set; }

        public DateOnly ProbationStartDate { get; set; }

        public DateOnly ProbationEndDate { get; set; }

        public bool? IsCompleted { get; set; }

        public int CreatedBy { get; set; }
    }

    public class ProbationResponse
    {
        public int ProbationId { get; set; }

        public int StaffCreationId { get; set; }

        public DateOnly ProbationStartDate { get; set; }

        public DateOnly ProbationEndDate { get; set; }

        public bool? IsCompleted { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateProbation
    {
        public int ProbationId { get; set; }

        public int StaffCreationId { get; set; }

        public DateOnly ProbationStartDate { get; set; }

        public DateOnly ProbationEndDate { get; set; }

        public bool? IsCompleted { get; set; }

        public int UpdatedBy { get; set; }
    }
}
