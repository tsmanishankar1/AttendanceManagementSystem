namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class SalaryComponentType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<SalaryComponent> SalaryComponents { get; set; } = new List<SalaryComponent>();
}
