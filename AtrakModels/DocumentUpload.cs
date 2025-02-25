using System;
using System.Collections.Generic;

namespace AttendanceManagement.AtrakModels;

public partial class DocumentUpload
{
    public int Id { get; set; }

    public string? ParentId { get; set; }

    public byte[]? FileContent { get; set; }

    public bool IsActive { get; set; }

    public string? TypeOfDocument { get; set; }

    public DateTime CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public bool IsApplicableForUserView { get; set; }

    public virtual RequestApplication? Parent { get; set; }
}
