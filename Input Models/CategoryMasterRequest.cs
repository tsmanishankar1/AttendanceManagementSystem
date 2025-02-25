namespace AttendanceManagement.Input_Models
{
    public class CategoryMasterRequest
    {
        public string FullName { get; set; } = null!;

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
        public string FullName { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }

    }
}
