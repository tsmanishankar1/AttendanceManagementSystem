using System;
using System.Collections.Generic;

namespace AttendanceManagement.Models;

public partial class AcademicDetail
{
    public int Id { get; set; }

    public string? Qualification { get; set; }

    public string? Specialization { get; set; }

    public string? University { get; set; }

    public string? Institute { get; set; }

    public string? MediumOfInstruction { get; set; }

    public string? CourseType { get; set; }

    public int? YearOfPassing { get; set; }

    public string? CourseOfAppraisal { get; set; }

    public string? Board { get; set; }

    public int StaffId { get; set; }

    public int CreatedBy { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime CreatedUtc { get; set; }

    public DateTime? UpdatedUtc { get; set; }

    public bool? IsActive { get; set; }

    public virtual StaffCreation CreatedByNavigation { get; set; } = null!;

    public virtual StaffCreation Staff { get; set; } = null!;

    public virtual StaffCreation? UpdatedByNavigation { get; set; }
}
