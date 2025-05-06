namespace AttendanceManagement.Input_Models
{
    public class DesignationRequest
    {
        public string FullName { get; set; } = null!;
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
        public string FullName { get; set; } = null!;
        public string? ShortName { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}