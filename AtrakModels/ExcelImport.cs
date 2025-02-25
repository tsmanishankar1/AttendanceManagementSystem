using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class ExcelImport
{
    public int Id { get; set; }

    public string? EmpNo { get; set; }

    public string? Name { get; set; }

    public string? DateOfBirth { get; set; }

    public string? DateOfJoining { get; set; }

    public string? Gender { get; set; }

    public string? FatherName { get; set; }

    public string? WorkWeekPattern { get; set; }

    public string? Company { get; set; }

    public string? BussinessArea { get; set; }

    public string? Grade { get; set; }

    public string? Designation { get; set; }

    public string? CostCenter { get; set; }

    public string? Department { get; set; }

    public string? Team { get; set; }

    public string? ImportFileName { get; set; }

    public string? ImportLine { get; set; }

    public string? DataOrigin { get; set; }

    public DateTime? CreatedOn { get; set; }

    public bool IsProcessed { get; set; }
}
