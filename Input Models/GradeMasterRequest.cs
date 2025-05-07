using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Input_Models
{
    public class GradeMasterRequest
    {
        [MaxLength(50)]
        public string FullName { get; set; } = null!;
        [MaxLength(50)]
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
        [MaxLength(50)]
        public string FullName { get; set; } = null!;
        [MaxLength(50)]
        public string? ScreenOption { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}