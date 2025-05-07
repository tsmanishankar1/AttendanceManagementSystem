using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Input_Models
{
    public class DepartmentRequest
    {
        [MaxLength(50)]
        public string FullName { get; set; } = null!;
        [MaxLength(50)]
        public string ShortName { get; set; } = null!;      
        public long Phone { get; set; }
        [MaxLength(100)]
        public string? Fax { get; set; }
        [MaxLength(100)]
        public string? Email { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class DepartmentResponse
    {
        public int DepartmentMasterId { get; set; }
        public string FullName { get; set; } = null!;
        public string? ShortName { get; set; }
        public long Phone { get; set; }
        public string? Fax { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateDepartment
    {
        public int DepartmentMasterId { get; set; }
        [MaxLength(50)]
        public string FullName { get; set; } = null!;
        [MaxLength(50)]
        public string ShortName { get; set; } = null!;
        public long Phone { get; set; }
        [MaxLength(100)]
        public string? Fax { get; set; }
        [MaxLength(100)]
        public string? Email { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}