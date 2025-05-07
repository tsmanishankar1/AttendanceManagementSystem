using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.Input_Models
{
    public class CategoryMasterRequest
    {
        [MaxLength(50)]
        public string FullName { get; set; } = null!;
        [MaxLength(5)]
        public string ShortName { get; set; } = null!;
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class CategoryMasterResponse
    {
        public int CategoryMasterId { get; set; }
        public string FullName { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public bool IsActive { get; set; }
    }

    public class UpdateCategory
    {
        public int CategoryMasterId { get; set; }
        [MaxLength(50)]
        public string FullName { get; set; } = null!;
        [MaxLength(5)]
        public string ShortName { get; set; } = null!;
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}