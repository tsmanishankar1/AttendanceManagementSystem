namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class PaySheet
{
    public int Id { get; set; }

    public string StaffId { get; set; } = null!;

    public string EmployeeName { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public string DisplayNameInReports { get; set; } = null!;

    public DateOnly DateOfJoining { get; set; }

    public int EmployeeNumber { get; set; }

    public int DesignationId { get; set; }

    public int DepartmentId { get; set; }

    public string Location { get; set; } = null!;

    public string Gender { get; set; } = null!;

    public DateOnly DateOfBirth { get; set; }

    public string FatherOrMotherName { get; set; } = null!;

    public string? SpouseName { get; set; }

    public string Address { get; set; } = null!;

    public string Email { get; set; } = null!;

    public long PhoneNo { get; set; }

    public string BankName { get; set; } = null!;

    public long AccountNo { get; set; }

    public string IfscCode { get; set; } = null!;

    public string PfAccountNo { get; set; } = null!;

    public long Uan { get; set; }

    public string Pan { get; set; } = null!;

    public long AadhaarNo { get; set; }

    public long EsiNo { get; set; }

    public DateOnly SalaryEffectiveFrom { get; set; }

    public decimal BasicActual { get; set; }

    public decimal HraActual { get; set; }

    public decimal ConveActual { get; set; }

    public decimal MedAllowActual { get; set; }

    public decimal SplAllowActual { get; set; }

    public decimal LopDays { get; set; }

    public decimal StdDays { get; set; }

    public decimal WrkDays { get; set; }

    public string PfAdmin { get; set; } = null!;

    public decimal BasicEarned { get; set; }

    public decimal BasicArradj { get; set; }

    public decimal HraEarned { get; set; }

    public decimal HraArradj { get; set; }

    public decimal ConveEarned { get; set; }

    public decimal ConveArradj { get; set; }

    public decimal MedAllowEarned { get; set; }

    public decimal MedAllowArradj { get; set; }

    public decimal SplAllowEarned { get; set; }

    public decimal SplAllowArradj { get; set; }

    public decimal OtherAll { get; set; }

    public decimal GrossEarn { get; set; }

    public decimal Pf { get; set; }

    public decimal Esi { get; set; }

    public decimal Lwf { get; set; }

    public decimal Pt { get; set; }

    public decimal It { get; set; }

    public decimal MedClaim { get; set; }

    public decimal OtherDed { get; set; }

    public decimal GrossDed { get; set; }

    public decimal NetPay { get; set; }

    public int Month { get; set; }

    public int Year { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual DepartmentMaster Department { get; set; } = null!;

    public virtual DesignationMaster Designation { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
