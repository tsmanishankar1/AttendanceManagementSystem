using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Application.Dtos.Attendance
{
    public class ReaderConfigurationRequest
    {
        [MaxLength(100)]
        public string ReaderName { get; set; } = null!;
        [MaxLength(100)]
        public string ReaderIpAddress { get; set; } = null!;
        public bool IsAttendanceReader { get; set; }
        public bool IsAccessReader { get; set; }
        public bool IsActive { get; set; } = true;  
        public int CreatedBy { get; set; }
        public int ReaderTypeId { get; set; }
    }

    public class ReaderConfigurationResponse
    {
        public int Id { get; set; }
        public string ReaderName { get; set; } = null!;
        public string ReaderIpAddress { get; set; } = null!;
        public bool IsAttendanceReader { get; set; }
        public bool IsAccessReader { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedUtc { get; set; }
        public int ReaderTypeId { get; set; }
        public string ReaderTypeName { get; set; } = null!;
    }

    public class AttendanceStatusColorDto
    {
        [MaxLength(100)]
        public string StatusName { get; set; } = null!;
        [MaxLength(50)]
        public string ShortName { get; set; } = null!;
        [MaxLength(100)]
        public string ColourCode { get; set; } = null!;
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateAttendanceStatusRequest
    {
        public List<int> StaffIds { get; set; } = new List<int>(); 
        public int StatusId { get; set; }
        public DateOnly FromDate { get; set; }
        public DateOnly ToDate { get; set; }
        [MaxLength(255)]
        public string? Remarks { get; set; }
        public int? DurationId { get; set; }
        public int CreatedBy { get; set; } 
    }
}