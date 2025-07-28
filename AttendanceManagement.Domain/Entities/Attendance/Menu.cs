using System;
using System.Collections.Generic;

namespace AttendanceManagement.Domain.Entities.Attendance;

public partial class Menu
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int? ParentMenuId { get; set; }

    public bool IsActive { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<Menu> InverseParentMenu { get; set; } = new List<Menu>();

    public virtual Menu? ParentMenu { get; set; }

    public virtual ICollection<RoleMenuMapping> RoleMenuMappings { get; set; } = new List<RoleMenuMapping>();

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
