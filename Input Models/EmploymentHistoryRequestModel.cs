namespace AttendanceManagement.Input_Models
{
    public class EmploymentHistoryRequestModel
    {

        public string CompanyName { get; set; } = null!;
        public string JobTitle { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public decimal? LastDrawnSalary { get; set; }
        public string? JobLocation { get; set; }
        public string EmploymentType { get; set; } = null!;
        public string? ReasonForLeaving { get; set; }
        public string? ReferenceContact { get; set; }
        public int StaffCreationId { get; set; }

        public int CreatedBy { get; set; }
    }

    public class EmploymentHistoryUpdateModel
    {
        public int EmployeeHistoryId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string JobTitle { get; set; } = null!;
        public int StaffCreationId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public decimal? LastDrawnSalary { get; set; }
        public string? JobLocation { get; set; }
        public string EmploymentType { get; set; } = null!;
        public string? ReasonForLeaving { get; set; }
        public string? ReferenceContact { get; set; }
        public bool IsActive { get; set; }
        public int UpdatedBy { get; set; }
    }

    public class EmploymentHistoryResponseModel
    {
        public int EmployeeHistoryId { get; set; }
        public int StaffCreationId { get; set; }
        public string CompanyName { get; set; } = null!;
        public string JobTitle { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public decimal? LastDrawnSalary { get; set; }
        public string? JobLocation { get; set; }
        public string EmploymentType { get; set; } = null!;
        public string? ReasonForLeaving { get; set; }
        public string? ReferenceContact { get; set; }
    }
}
