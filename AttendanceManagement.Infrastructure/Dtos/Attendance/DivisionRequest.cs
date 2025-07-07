using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Application.Dtos.Attendance
{
    public class DivisionRequest
    {
        [MaxLength(50)]
        public string FullName { get; set; } = null!;
        [MaxLength(5)]
        public string ShortName { get; set; } = null!;
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class DivisionResponse
    {
        public int DivisionMasterId { get; set; }
        public string FullName { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateDivision
    {
        public int DivisionMasterId { get; set; }
        [MaxLength(50)]
        public string FullName { get; set; } = null!;
        [MaxLength(5)]
        public string ShortName { get; set; } = null!;  
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}