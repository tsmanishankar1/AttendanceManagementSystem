namespace AttendanceManagement.Input_Models
{
    public class AppraisalDto
    {
        public int StaffId { get; set; }
        public string EmpId { get; set; } = null!;
        public string EmpName { get; set; } = null!;
        public decimal Tenure { get; set; }
        public string ReportingManagers { get; set; } = null!;
        public string Division { get; set; } = null!;
        public string Department { get; set; } = null!;
        public bool? IsCompleted { get; set; }
    }

    public class ProbationDto
    {
        public int StaffId { get; set; }
        public string EmpId { get; set; } = null!;
        public string EmpName { get; set; } = null!;
        public decimal Tenure { get; set; }
        public string ReportingManagers { get; set; } = null!;
        public string Division { get; set; } = null!;
        public string Department { get; set; } = null!;
        public bool? IsCompleted { get; set; }
    }

    /*    public class SelectedStaffResponse
        {
            public List<int> staffIds { get; set; } = new List<int>();
        }
    */
    public class SelectedEmployeesRequest
    {
        public int AppraisalId { get; set; }
        public List<SelectedEmployeesRequestSelectedRows> SelectedRows { get; set; } = new List<SelectedEmployeesRequestSelectedRows>();
        public int CreatedBy { get; set; }
    }

    public class SelectedEmployeesRequestSelectedRows
    {
        public string EmpId { get; set; } = null!;
        public string EmpName { get; set; } = null!;
        public decimal TenureInYears { get; set; }
        public string ReportingManagers { get; set; } = null!;
        public string Division { get; set; } = null!;
        public string Department { get; set; } = null!;
    }
    public class SelectedEmployeesResponseSelectedRows
    {
        public string EmpId { get; set; } = null!;
        public string EmpName { get; set; } = null!;
        public decimal TenureInYears { get; set; }
        public string ReportingManagers { get; set; } = null!;
        public string Division { get; set; } = null!;
        public string Department { get; set; } = null!;
        public bool? IsCompleted { get; set; }
    }


    public class UploadMisSheetRequest
    {
        public int AppraisalId { get; set; }
        public IFormFile File { get; set; } = null!;
        public int CreatedBy { get; set; }
    }

    public class PerformanceReviewResponse
    {
        public int Id { get; set; }
        public string EmpId { get; set; } = null!;
        public string EmpName { get; set; } = null!;
        public decimal TenureInYears { get; set; }
        public string ReportingManagers { get; set; } = null!;
        public string Division { get; set; } = null!;
        public string Department { get; set; } = null!;
        public decimal ProductivityPercentage { get; set; }
        public decimal QualityPercentage { get; set; }
        public decimal PresentPercentage { get; set; }
        public decimal FinalPercentage { get; set; }
        public string Grade { get; set; } = null!;
        public decimal AbsentDays { get; set; }
        public bool? IsCompleted { get; set; }
    }

    public class AgmApprovalRequest
    {
        public List<SelectedAgmApproval> SelectedRows { get; set; } = new List<SelectedAgmApproval>();
        public bool IsApproved { get; set; }
        public int ApprovedBy { get; set; }
    }

    public class AgmDetails
    {
        public int Id {  get; set; }
        public string Name { get; set; } = null!;
    }

    public class AgmApprovalTab
    {
        public List<SelectedAgmApproval> SelectedRows { get; set; } = new List<SelectedAgmApproval>();
        public int AgmId { get; set; }
        public int CreatedBy { get; set; }
    }

    public class SelectedAgmApproval
    {
        public int Id { get; set; }
    }

    public class LetterAcceptance
    {
        public int Id { get; set; }
        public bool IsAccepted { get; set; }
        public int AcceptedBy { get; set; }
    }

    public class LetterAcceptanceResponse
    {
        public string EmpId { get; set; } = null!;
        public string EmpName { get; set; } = null!;
        public string Division { get; set; } = null!;
        public string Department { get; set; } = null!;
        public bool IsAccepted { get; set; }
    }

    public class HrUploadResponse
    {
        public int Id { get; set; }
        public string EmpId { get; set; } = null!;
        public string EmpName { get; set; } = null!;
        public decimal TenureInYears { get; set; }
        public string ReportingManagers { get; set; } = null!;
        public string Division { get; set; } = null!;
        public string Department { get; set; } = null!;
        public decimal FinalAverageKraGrade { get; set; }
        public decimal AbsentDays { get; set; }
        public string? HrComments { get; set; }
        public string AppraisalType { get; set; } = null!;
        public bool? IsCompleted { get; set; }
        public int CreatedBy { get; set; }
    }
}
