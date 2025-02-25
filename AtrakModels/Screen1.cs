using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class Screen1
{
    public int Id { get; set; }

    public string ScreenName { get; set; } = null!;

    public string ControllerName { get; set; } = null!;

    public string ActionName { get; set; } = null!;

    public string? MenuIcon { get; set; }

    public decimal RankingId { get; set; }

    public bool IsActive { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? ModifiedOn { get; set; }

    public string? ModifiedBy { get; set; }

    public int? MenuTypeId { get; set; }

    public virtual ICollection<ScreenTxn> ScreenTxns { get; set; } = new List<ScreenTxn>();
}
