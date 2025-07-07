namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class LeaveType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string ShortName { get; set; } = null!;

    public bool Accountable { get; set; }

    public bool Encashable { get; set; }

    public bool PaidLeave { get; set; }

    public bool CommonType { get; set; }

    public bool PermissionType { get; set; }

    public bool CarryForward { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual ICollection<AssignLeaveType> AssignLeaveTypes { get; set; } = new List<AssignLeaveType>();

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<IndividualLeaveCreditDebit> IndividualLeaveCreditDebits { get; set; } = new List<IndividualLeaveCreditDebit>();

    public virtual ICollection<LeaveGroupConfiguration> LeaveGroupConfigurations { get; set; } = new List<LeaveGroupConfiguration>();

    public virtual ICollection<LeaveGroupTransaction> LeaveGroupTransactions { get; set; } = new List<LeaveGroupTransaction>();

    public virtual ICollection<LeaveRequisition> LeaveRequisitions { get; set; } = new List<LeaveRequisition>();

    public virtual ICollection<MyApplication> MyApplications { get; set; } = new List<MyApplication>();

    public virtual ICollection<PrefixAndSuffix> PrefixAndSuffixes { get; set; } = new List<PrefixAndSuffix>();

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
