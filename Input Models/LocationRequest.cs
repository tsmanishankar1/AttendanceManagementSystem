namespace AttendanceManagement.Input_Models
{
    public class LocationRequest
    {
        public string FullName { get; set; } = null!;

        public string ShortName { get; set; } = null!;

        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class LocationResponse
    {
        public int LocationMasterId { get; set; }

        public string FullName { get; set; } = null!;

        public string ShortName { get; set; } = null!;
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateLocation
    {
        public int LocationMasterId { get; set; }

        public string FullName { get; set; } = null!;

        public string ShortName { get; set; } = null!;

        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
