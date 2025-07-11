namespace AttendanceManagement.Application.Dtos.Attendance
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

    public class MonthlyPerformanceResponse
    {
        public string EmployeeCode { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public decimal ProductivityScore { get; set; }
        public decimal QualityScore { get; set; }
        public decimal PresentScore { get; set; }
        public decimal TotalScore { get; set; }
        public decimal ProductivityPercentage { get; set; }
        public decimal QualityPercentage { get; set; }
        public decimal PresentPercentage { get; set; }
        public decimal FinalPercentage { get; set; }
        public string Grade { get; set; } = null!;
        public decimal TotalAbsents { get; set; }
        public string ReportingHead { get; set; } = null!;
        public string? TenureYears { get; set; }
        public string HrComments { get; set; } = null!;
        public string PerformanceType { get; set; } = null!;
        public string Month { get; set; } = null!;
        public int Year { get; set; }
    }

    public class QuarterlyPerformanceResponse
    {
        public string EmployeeCode { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public string? TenureYears { get; set; }
        public decimal ProductivityPercentage { get; set; }
        public decimal QualityPercentage { get; set; }
        public decimal PresentPercentage { get; set; }
        public decimal FinalPercentage { get; set; }
        public string Grade { get; set; } = null!;
        public decimal AbsentDays { get; set; }
        public string HrComments { get; set; } = null!;
        public string PerformanceType { get; set; } = null!;
        public string Quarter { get; set; } = null!;
        public int Year { get; set; }
    }

    public class YearlyPerformanceResponse
    {
        public string EmployeeCode { get; set; } = null!;
        public string EmployeeName { get; set; } = null!;
        public string Designation { get; set; } = null!;
        public string? TenureYears { get; set; }
        public decimal ProductivityPercentage { get; set; }
        public decimal QualityPercentage { get; set; }
        public decimal PresentPercentage { get; set; }
        public decimal FinalPercentage { get; set; }
        public string Grade { get; set; } = null!;
        public decimal AbsentDays { get; set; }
        public string HrComments { get; set; } = null!;
        public string PerformanceType { get; set; } = null!;
        public int Year { get; set; }
    }
}
