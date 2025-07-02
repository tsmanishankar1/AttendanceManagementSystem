using System.ComponentModel.DataAnnotations;

namespace AttendanceManagement.InputModels
{
    public class SubFunctionRequest
    {
        [MaxLength(50)]
        public string FullName { get; set; } = null!;
        [MaxLength(5)]
        public string ShortName { get; set; } = null!;
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
    }

    public class SubFunctionResponse
    {
        public int SubFunctionMasterId { get; set; }
        public string FullName { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public bool  IsActive{ get; set; }
        public int CreatedBy { get; set; }
    }

    public class UpdateSubFunction
    {
        public int SubFunctionMasterId { get; set; }
        [MaxLength(50)]
        public string FullName { get; set; } = null!;
        [MaxLength(5)]
        public string ShortName { get; set; } = null!;
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
    }
}