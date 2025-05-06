namespace AttendanceManagement.Input_Models
{
    public class GradeMasterRequest
    {
        public string FullName { get; set; } = null!;
        public string? ScreenOption { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class GradeMasterResponse
    {
        public int GradeMasterId { get; set; }
        public string FullName { get; set; } = null!;
        public string? ScreenOption { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateGradeMaster
    {
        public int GradeMasterId { get; set; }
        public string FullName { get; set; } = null!;
        public string? ScreenOption { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}