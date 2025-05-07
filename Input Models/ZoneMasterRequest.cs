using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Input_Models
{
    public class ZoneMasterRequest
    {
        [MaxLength(50)]
        public string FullName { get; set; } = null!;
        [MaxLength(5)]
        public string ShortName { get; set; } = null!;
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class ZoneMasterResponse
    {
        public int ZoneMasterId { get; set; }
        public string FullName { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateZoneMaster
    {
        public int ZoneMasterId { get; set; }
        [MaxLength(50)]
        public string FullName { get; set; } = null!;
        [MaxLength(5)]
        public string ShortName { get; set; } = null!;
        public bool IsActive { get; set; }
        public int UpdatedBy { get; set; }
    }
}