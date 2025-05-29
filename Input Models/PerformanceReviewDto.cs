namespace AttendanceManagement.Input_Models
{
    public class PerformanceReviewDto
    {
        public int Id { get; set; }

        public string From { get; set; } = null!;

        public string To { get; set; } = null!;

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }
    }

    public class PerformanceReviewRequest
    {
        public string From { get; set; } = null!;

        public string To { get; set; } = null!;

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }
    }

    public class UpdatePerformanceReview
    {
        public int Id { get; set; }

        public string From { get; set; } = null!;

        public string To { get; set; } = null!;

        public bool IsActive { get; set; }

        public int UpdatedBy { get; set; }
    }

    public class EligibleEmployeeResponse
    {
        public int Id { get; set; }

        public string StaffName { get; set; } = null!;
        public string PerformanceCycle { get; set; } = null!;

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }
    }

    public class EligibleEmployeeRequest
    {
        public int StaffId { get; set; }

        public int PerformanceCycleId { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }
    }

    public class EligibleEmployeeUpdate
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsActive { get; set; }
        public int UpdatedBy { get; set; }
    }
}
