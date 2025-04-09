namespace AttendanceManagement.Input_Models
{
    public class ReaderConfigurationRequest
    {
        public string ReaderName { get; set; } = null!;
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
        public string StatusName { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string ColourCode { get; set; } = null!;
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }
    public class UpdateAttendanceStatusRequest
    {
        public List<int> StaffIds { get; set; } = new List<int>(); 
        public int StatusId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string? Remarks { get; set; }
        public int? DurationId { get; set; }
        public int CreatedBy { get; set; } 
    }
}
