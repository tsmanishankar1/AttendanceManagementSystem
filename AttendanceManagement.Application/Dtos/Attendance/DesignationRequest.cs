using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Application.Dtos.Attendance
{
    public class DesignationRequest
    {
        [MaxLength(50)]
        public string FullName { get; set; } = null!;
        [MaxLength(50)]
        public string? ShortName { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class DesignationResponse
    {
        public int DesignationMasterId { get; set; }
        public string FullName { get; set; } = null!;
        public string? ShortName { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateDesignation
    {
        public int DesignationMasterId { get; set; }
        [MaxLength(50)]
        public string FullName { get; set; } = null!;
        [MaxLength(50)]
        public string? ShortName { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}