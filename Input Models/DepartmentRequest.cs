namespace AttendanceManagement.Input_Models
{
    public class DepartmentRequest
    {
        public string FullName { get; set; } = null!;

        public string ShortName { get; set; } = null!;      

        public long Phone { get; set; }

        public string? Fax { get; set; }

        public string? Email { get; set; }

        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class DepartmentResponse
    {
        public int DepartmentMasterId { get; set; }

        public string FullName { get; set; } = null!;

        public string ShortName { get; set; } = null!;

        public long Phone { get; set; }

        public string? Fax { get; set; }

        public string? Email { get; set; }

        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateDepartment
    {
        public int DepartmentMasterId { get; set; }

        public string FullName { get; set; } = null!;

        public string ShortName { get; set; } = null!;

        public long Phone { get; set; }

        public string? Fax { get; set; }

        public string? Email { get; set; }

        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
