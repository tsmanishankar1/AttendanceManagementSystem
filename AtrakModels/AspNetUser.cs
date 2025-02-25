using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class AspNetUser
{
    public string Id { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? StaffId { get; set; }

    public string? UserFullName { get; set; }

    public string? UserRole { get; set; }

    public string Discriminator { get; set; } = null!;

    public bool? IsActive { get; set; }

    public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

    public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();

    public virtual ICollection<AspNetRole> Roles { get; set; } = new List<AspNetRole>();
}
